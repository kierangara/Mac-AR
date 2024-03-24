using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class MultiplayerPuzzleManager : NetworkBehaviour
{
    [SerializeField] private List<NetworkObject> puzzles;
    public int activePuzzleBatchIndex = 0;
    public int activePuzzleIndex = 0;
    public Camera cam; 
    public List<NetworkObject> puzzleInstances = new List<NetworkObject>();
    List<int> spawnedPuzzles = new List<int>();

    //Start is called before the first frame update
    void Start()
    {
        Debug.Log("Multiplayer Puzzle Manager called");

        SpawnPuzzleBatch();
    }

    private void SpawnPuzzleBatch()
    {
        Debug.Log("Puzzle Batch Size: " + PuzzleConstants.puzzleBatches[activePuzzleBatchIndex].Count);
        foreach(var puzzle in PuzzleConstants.puzzleBatches[activePuzzleBatchIndex])
        {
            Debug.Log("Currently Spawning: " + puzzle.Item3);
            SpawnPuzzleServerRpc(puzzle.Item1, puzzle.Item2, puzzle.Item3);
        }
    }

    [ServerRpc]
    private void SpawnPuzzleServerRpc(int puzzleIndex, Vector3 puzzlePosition, Quaternion puzzleRotation)
    {
        
        if(!IsServer)
        {
            return;
        }
        if(spawnedPuzzles.Contains(puzzleIndex))
        {
            return;
        }
        
        Debug.Log("SpawnPuzzleServerRpcCalled");

        // Get Users
        /// TODO: Exists here in case of changes to list, check if can be moved to start and updated asyncronously 
        List<ulong> clients = new List<ulong>();
        foreach(NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
        {
            clients.Add(client.ClientId);
        }

        // Serialize
        byte[] bytes = ULongListToBytes(clients);

        // Instantiate 
        Debug.Log("Spawn: " + puzzleIndex);
        var puzzleInstance = Instantiate(puzzles[puzzleIndex], puzzlePosition, puzzleRotation); 

        // Spawn
        puzzleInstance.SpawnWithOwnership(OwnerClientId);

        // Save Instance
        puzzleInstances.Add(puzzleInstance);
        spawnedPuzzles.Add(puzzleIndex);

        // Initialize
        var seed = Guid.NewGuid().GetHashCode();
        InitializeClientRpc(puzzleInstance, bytes, seed);
    }

    [ClientRpc]
    private void InitializeClientRpc(NetworkObjectReference puzzleRef, byte[] clientBytes, int seed)
    {
        if (puzzleRef.TryGet(out NetworkObject puzzle))
        {
            // Deserialize
            List<ulong> clients = BytesToULongList(clientBytes);

            puzzle.GetComponentInChildren<PuzzleData>().cam = cam;
            puzzle.GetComponentInChildren<PuzzleData>().completePuzzle = this;

            // TODO: Check if there are any ref issues with passing in entire list at once, faster than 
            // one at a time
            foreach (ulong client in clients)
            {
                puzzle.GetComponentInChildren<PuzzleData>().connectedClients.Add(client);
            }

            var puzzleScript = puzzle.GetComponentInChildren<PuzzleBase>();

            puzzleScript.seed = seed;
            puzzleScript.InitializePuzzle();
        }
        
    }

    [ServerRpc(RequireOwnership = false)]
    public void SkipPuzzleServerRpc()
    {
        CompletePuzzleServerRpc(0, PuzzleConstants.puzzleBatches[activePuzzleBatchIndex][activePuzzleIndex].Item1);
    }

    [ServerRpc(RequireOwnership = false)]
    public void CompletePuzzleServerRpc(ulong clientId, int puzzleId)
    {
        // Make sure the user is attempting to skip the current puzzle
        if(puzzleId != PuzzleConstants.puzzleBatches[activePuzzleBatchIndex][activePuzzleIndex].Item1)
        {
            return;
        }

        puzzleInstances[activePuzzleIndex].GetComponentInChildren<PuzzleBase>().SetActive(false);
        activePuzzleIndex += 1;

        UpdateActivePuzzleClientRpc(activePuzzleIndex);

        if(activePuzzleIndex < PuzzleConstants.puzzleBatches[activePuzzleBatchIndex].Count)
        {
            puzzleInstances[activePuzzleIndex].GetComponentInChildren<PuzzleBase>().SetActive(true);
        }
        else 
        {
            CompletePuzzleBatchServerRpc();
        }
        
    }

    [ServerRpc]
    private void CompletePuzzleBatchServerRpc()
    {
        // Despawn all puzzles
        foreach(var puzzleInstance in puzzleInstances)
        {
            puzzleInstance.Despawn();
        }

        puzzleInstances.Clear();
        
        // Spawn next batch
        activePuzzleBatchIndex += 1;
        activePuzzleIndex = 0;

        UpdateActiveBatchClientRpc(activePuzzleBatchIndex);

        if(activePuzzleBatchIndex < PuzzleConstants.puzzleBatches.Count)
        {
            SpawnPuzzleBatch();
        }
        else 
        {
            ExitGameClientRpc();
        }
    }

    [ClientRpc]
    public void UpdateActivePuzzleClientRpc(int puzzleIndex)
    {
        activePuzzleIndex = puzzleIndex;
    }

    [ClientRpc]
    public void UpdateActiveBatchClientRpc(int batchIndex)
    {
        activePuzzleIndex = 0;
        activePuzzleBatchIndex = batchIndex;
    }


    [ClientRpc]
    public void ExitGameClientRpc()
    {
        ReturnToMain exitGame = new ReturnToMain();
        exitGame.returnToMain();
        PlayerPrefs.SetString("lobbyID", "");
    }

    [ServerRpc(RequireOwnership = false)]
    public void RefreshPuzzlePositionsServerRpc(ulong clientId)
    {
        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[]{clientId}
            }
        };

        foreach(var puzzle in puzzleInstances)
        {

            RefreshPuzzlePositionClientRpc(puzzle, clientRpcParams);
        }
    }

    [ClientRpc]
    private void RefreshPuzzlePositionClientRpc(NetworkObjectReference puzzleRef, ClientRpcParams clientRpcParams = default)
    {
        var posLookup = new Dictionary<int, Vector3>{
            {PuzzleConstants.ISO_ID, PuzzleConstants.ISO_SPAWN_POS},
            {PuzzleConstants.MAZE_ID, PuzzleConstants.MAZE_SPAWN_POS},
            {PuzzleConstants.WIRE_ID, PuzzleConstants.WIRE_SPAWN_POS},
            {PuzzleConstants.COMBINATION_ID, PuzzleConstants.COMBINATION_SPAWN_POS},
            {PuzzleConstants.SIMON_ID, PuzzleConstants.SIMON_SPAWN_POS}
        };

        if (puzzleRef.TryGet(out NetworkObject puzzle))
        {
            var puzzleId = puzzle.GetComponentInChildren<PuzzleBase>().puzzleId;

            puzzle.gameObject.transform.position = cam.transform.position + posLookup[puzzleId];
        }
    }

    public byte[] ULongListToBytes(List<ulong> clients) 
    {
        return clients
            .SelectMany(BitConverter.GetBytes)
            .ToArray();
    }

    public List<ulong> BytesToULongList(byte[] bytes) 
    {
        // TODO: Add ulong size check that changes Uint64/ UInt32
        var size = sizeof(ulong);
        return Enumerable.Range(0, bytes.Length / size)
                     .Select(i => BitConverter.ToUInt64(bytes, i * size))
                     .ToList();
    }
}
