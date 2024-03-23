using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class MultiplayerPuzzleManager : NetworkBehaviour
{
    // private List<GameObject> puzzles = new List<GameObject>();
    // [SerializeField] private NetworkObject puzzle;
    [SerializeField] private List<NetworkObject> puzzles;
    private int puzzleIndex = 0;
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

        // Serialize
        byte[] bytes = objectToBytes(clients);

        // Instantiate 
        Debug.Log("Spawn: " + puzzleIndex);
        puzzleInstance = Instantiate(puzzles[puzzleIndex]); 

        // Spawn
        puzzleInstance.SpawnWithOwnership(OwnerClientId);

        // Initialize
        InitializeClientRpc(puzzleInstance, bytes);

        // Increment
        puzzleIndex += 1;
    }

    [ClientRpc]
    private void InitializeClientRpc(NetworkObjectReference puzzleRef, byte[] clientBytes)
    {
        if (puzzleRef.TryGet(out NetworkObject puzzle))
        {
            // Deserialize
            List<ulong> clients = bytesToObject(clientBytes);

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

    // TODO: Will need to take in puzzle ID too to allow anyone to call (not just host) while also
    // making sure to ignore duplicate requests to complete the same puzzle
    [ServerRpc]
    public void CompletePuzzleServerRpc(ulong clientId)
    {
        Debug.Log("Despawn");
        try 
        {
            puzzleInstance.Despawn();
        }
        catch 
        {
            Debug.Log("No object to despawn");
        }
        
        if(puzzleIndex < puzzles.Count)
        {
            CompletePuzzleClientRpc();
        }
        else 
        {
            SceneManager.LoadScene(0);
        }
        
    }

    [ClientRpc]
    public void CompletePuzzleClientRpc()
    {
        Debug.Log("Despawn");
        try
        {
            puzzleInstance.Despawn();
        }
        catch 
        {
            Debug.Log("No object to despawn");
        }
    
        puzzleInstance = null;

        Debug.Log("Spawn New");
        SpawnPuzzleServerRpc();
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
