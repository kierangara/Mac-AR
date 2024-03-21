using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleData : MonoBehaviour
{
    // Populated by Puzzle
    public int minPlayers = 0;
    public int maxPlayers = 0;
    
    // Populated by Manager
    public Camera cam;
    [HideInInspector] public MultiplayerPuzzleManager completePuzzle;
    [HideInInspector] public List<ulong> connectedClients;
}
