using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Netcode;

public class MultiplayerPuzzleManager : NetworkBehaviour
{
    // private List<GameObject> puzzles = new List<GameObject>();
    [SerializeField] private NetworkObject puzzle;
    public Camera cam; 
    NetworkObject puzzleInstance;

    //Start is called before the first frame update
    void Start()
    {
        Debug.Log("Multiplayer Puzzle Manager called");

        SpawnPuzzleServerRpc();
    }

    [ServerRpc]
    private void SpawnPuzzleServerRpc()
    {
        
        if(!IsServer)
        {
            return;
        }
        if(puzzleInstance!=null)
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

        Debug.Log("Init 0");

        // Serialize
        byte[] bytes = objectToBytes(clients);
        Debug.Log($"[{string.Join(",", clients)}]");
        Debug.Log($"[{string.Join(",", bytes)}]");

        // Instantiate 
        puzzleInstance = Instantiate(puzzle); 

        // Spawn
        puzzleInstance.SpawnWithOwnership(OwnerClientId);

        // Initialize
        InitializeClientRpc(puzzleInstance, bytes);
    }

    [ClientRpc]
    private void InitializeClientRpc(NetworkObjectReference puzzleRef, byte[] clientBytes)
    {
        Debug.Log("Init 1");
        if (puzzleRef.TryGet(out NetworkObject puzzle))
        {
            // Deserialize
            List<ulong> clients = bytesToObject(clientBytes);

            Debug.Log("Init 2");
            puzzle.GetComponentInChildren<PuzzleData>().cam = cam;
            puzzle.GetComponentInChildren<PuzzleData>().completePuzzle = this;

            // TODO: Check if there are any ref issues with passing in entire list at once, faster than 
            // one at a time
            Debug.Log($"[{string.Join(",", clientBytes)}]");
            Debug.Log($"[{string.Join(",", clients)}]");
            foreach (ulong client in clients)
            {
                puzzle.GetComponentInChildren<PuzzleData>().connectedClients.Add(client);
            }
            puzzle.GetComponentInChildren<MazePuzzle>().InitializePuzzle();
            //puzzleRef.
            Debug.Log("Init 3");
        }
        
    }

    [ServerRpc(RequireOwnership = false)]
    public void CompletePuzzleServerRpc(ulong clientId)
    {
        puzzleInstance.Despawn();
        CompletePuzzleClientRpc();
    }

    [ClientRpc]
    public void CompletePuzzleClientRpc()
    {
        puzzleInstance.Despawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private byte[] objectToBytes(List<ulong> clients) 
    {
        return clients
            .SelectMany(BitConverter.GetBytes)
            .ToArray();
    }

    private List<ulong> bytesToObject(byte[] bytes) 
    {
        // TODO: Add ulong size check that changes Uint64/ UInt32
        var size = sizeof(ulong);
        return Enumerable.Range(0, bytes.Length / size)
                     .Select(i => BitConverter.ToUInt64(bytes, i * size))
                     .ToList();
    }
}
