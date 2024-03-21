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
    private int activePuzzleBatchIndex = 0;
    private int activePuzzleIndex = 0;
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
            Debug.Log("Currently Spawning: " + puzzle.Item1);
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
        byte[] bytes = ObjectToBytes(clients);

        // Instantiate 
        Debug.Log("Spawn: " + puzzleIndex);
        var puzzleInstance = Instantiate(puzzles[puzzleIndex], puzzlePosition, puzzleRotation); 

        // Spawn
        puzzleInstance.SpawnWithOwnership(OwnerClientId);

        // Save Instance
        puzzleInstances.Add(puzzleInstance);
        spawnedPuzzles.Add(puzzleIndex);

        // Initialize
        InitializeClientRpc(puzzleInstance, bytes);
    }

    [ClientRpc]
    private void InitializeClientRpc(NetworkObjectReference puzzleRef, byte[] clientBytes)
    {
        if (puzzleRef.TryGet(out NetworkObject puzzle))
        {
            // Deserialize
            List<ulong> clients = BytesToObject(clientBytes);

            puzzle.GetComponentInChildren<PuzzleData>().cam = cam;
            puzzle.GetComponentInChildren<PuzzleData>().completePuzzle = this;

            // TODO: Check if there are any ref issues with passing in entire list at once, faster than 
            // one at a time
            foreach (ulong client in clients)
            {
                puzzle.GetComponentInChildren<PuzzleData>().connectedClients.Add(client);
            }
            
            puzzle.GetComponentInChildren<PuzzleBase>().InitializePuzzle();
        }
        
    }

    public void SkipPuzzle()
    {
        CompletePuzzleServerRpc(0);
    }

    // TODO: Will need to take in puzzle ID too to allow anyone to call (not just host) while also
    // making sure to ignore duplicate requests to complete the same puzzle
    [ServerRpc]
    public void CompletePuzzleServerRpc(ulong clientId)
    {
        puzzleInstances[activePuzzleIndex].GetComponentInChildren<PuzzleBase>().active = false;
        activePuzzleIndex += 1;

        if(activePuzzleIndex < PuzzleConstants.puzzleBatches[activePuzzleBatchIndex].Count)
        {
            puzzleInstances[activePuzzleIndex].GetComponentInChildren<PuzzleBase>().active = true;
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

        if(activePuzzleBatchIndex < PuzzleConstants.puzzleBatches.Count)
        {
            SpawnPuzzleBatch();
        }
        else 
        {
            CompleGameClientRpc();
        }
    }

    [ClientRpc]
    private void CompleGameClientRpc()
    {
        SceneManager.LoadScene(0);
    }

    public byte[] ObjectToBytes(List<ulong> clients) 
    {
        return clients
            .SelectMany(BitConverter.GetBytes)
            .ToArray();
    }

    public List<ulong> BytesToObject(byte[] bytes) 
    {
        // TODO: Add ulong size check that changes Uint64/ UInt32
        var size = sizeof(ulong);
        return Enumerable.Range(0, bytes.Length / size)
                     .Select(i => BitConverter.ToUInt64(bytes, i * size))
                     .ToList();
    }
}
