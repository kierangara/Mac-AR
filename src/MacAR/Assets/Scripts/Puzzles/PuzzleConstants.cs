using System.Collections.Generic;
using UnityEngine;
public static class PuzzleConstants
{
    // Puzzle IDs (Should match index of MultiplayerPuzzleManger list for now)
    public const int ISO_ID = 0;
    public const int COMBINATION_ID = 1;
    public const int WIRE_ID = 2;
    public const int SIMON_ID = 3;
    public const int MAZE_ID = 4;

    // Puzzle Locations
    public static readonly Vector3 ISO_SPAWN_POS = new Vector3(0, 0, 0);
    public static readonly Vector3 COMBINATION_SPAWN_POS = new Vector3(0, 0, 0);
    public static readonly Vector3 WIRE_SPAWN_POS = new Vector3(0, 0, -5);
    public static readonly Vector3 SIMON_SPAWN_POS = new Vector3(-14, 0, 5f);
    public static readonly Vector3 MAZE_SPAWN_POS = new Vector3(0, 0, 0);

    // Puzzle  Rotations
    public static readonly Quaternion ISO_ROTATION = Quaternion.identity;
    public static readonly Quaternion COMBINATION_ROTATION = Quaternion.identity;
    public static readonly Quaternion WIRE_ROTATION = Quaternion.Euler(0, 180, 0);
    public static readonly Quaternion SIMON_ROTATION = Quaternion.Euler(0, 90, 0);
    public static readonly Quaternion MAZE_ROTATION = Quaternion.identity;

    // Puzzle Batches (Not all puzzles work on screen together, split into batches)
    public static readonly List<(int, Vector3, Quaternion)> batch1 = new List<(int, Vector3, Quaternion)>{
        (COMBINATION_ID, COMBINATION_SPAWN_POS, COMBINATION_ROTATION),
        (WIRE_ID, WIRE_SPAWN_POS, WIRE_ROTATION),
        (SIMON_ID, SIMON_SPAWN_POS, SIMON_ROTATION)
    };
    public static readonly List<(int, Vector3, Quaternion)> batch2 = new List<(int, Vector3, Quaternion)>{
        (ISO_ID, ISO_SPAWN_POS, ISO_ROTATION)
    };
    public static readonly List<(int, Vector3, Quaternion)> batch3 = new List<(int, Vector3, Quaternion)>{
        (MAZE_ID, MAZE_SPAWN_POS, MAZE_ROTATION)
    };

    public static readonly List<List<(int, Vector3, Quaternion)>> puzzleBatches = new List<List<(int, Vector3, Quaternion)>>{
        batch1, batch2, batch3
    };
}
