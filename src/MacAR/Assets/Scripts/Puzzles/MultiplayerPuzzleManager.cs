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
        // // Register Prefabs
        // NetworkManager.AddNetworkPrefab(puzzle);

        // // Spawn Puzzle Instance
        // var puzzleInstance = Instantiate(puzzle, new Vector3(0, 0, 0), Quaternion.identity);
        // /// Make Abstract
        // puzzleInstance.GetComponentInChildren<ObjectRotate>().cam = cam;
        // puzzleInstance.GetComponentInChildren<NetworkObject>().Spawn();
        SpawnPuzzleServerRpc();
    }

    [ServerRpc]
    private void SpawnPuzzleServerRpc()
    {
       NetworkObject puzzleInstance = Instantiate(puzzle, new Vector3(0, 0, 0), Quaternion.identity); 
       puzzleInstance.GetComponentInChildren<ObjectRotate>().cam = cam;
       puzzleInstance.SpawnWithOwnership(OwnerClientId);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
