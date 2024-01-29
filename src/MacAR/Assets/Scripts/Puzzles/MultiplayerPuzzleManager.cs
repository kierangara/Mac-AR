using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MultiplayerPuzzleManager : NetworkBehaviour
{
    // private List<GameObject> puzzles = new List<GameObject>();
    [SerializeField] private NetworkObject puzzle;
    public Camera cam; 

    //Start is called before the first frame update
    void Start()
    {
        SpawnPuzzleServerRpc();
    }

    [ServerRpc]
    private void SpawnPuzzleServerRpc()
    {
        if(!IsServer)
        {
            return;
        }

        // Instantiate 
        NetworkObject puzzleInstance = Instantiate(puzzle); 

        // Spawn
        NetworkObject spawnedNetworkObject = puzzleInstance.GetComponent<NetworkObject>();
        spawnedNetworkObject.Spawn();

        // Initialize
        // puzzleInstance.GetComponentInChildren<ObjectRotate>().cam = cam;
        InitializeClientRpc(puzzleInstance);
    }

    [ClientRpc]
    private void InitializeClientRpc(NetworkObjectReference puzzleRef)
    {
        if (puzzleRef.TryGet(out NetworkObject puzzle))
        {
            puzzle.GetComponentInChildren<ObjectRotate>().cam = cam;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
