using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public abstract class PuzzleBase : NetworkBehaviour
{
    public abstract void InitializePuzzle();
}
