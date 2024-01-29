using System.Collections;
using System.Collections.Generic;
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
        puzzleInstance = Instantiate(puzzle); 

        // Spawn
        puzzleInstance.SpawnWithOwnership(OwnerClientId);

        // Initialize
        InitializeClientRpc(puzzleInstance);
    }

    [ClientRpc]
    private void InitializeClientRpc(NetworkObjectReference puzzleRef)
    {
        if (puzzleRef.TryGet(out NetworkObject puzzle))
        {
            puzzle.GetComponentInChildren<PuzzleData>().cam = cam;
            puzzle.GetComponentInChildren<PuzzleData>().completePuzzle = this;
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
}
