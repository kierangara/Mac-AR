using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireBehaviour : PuzzleBase
{
    public PassiveWires passive;
    public ActiveWires active;

    private List<List<uint>> testSequence;

    // Start is called before the first frame update
    void Start()
    {
        testSequence = new List<List<uint>>{new List<uint>{1, 3}, 
                                            new List<uint>{0, 0}, 
                                            new List<uint>{3, 2}, 
                                            new List<uint>{2, 1}};
        
        InitializePuzzle();
    }

    public override void InitializePuzzle()
    {
        passive.Init(testSequence);
        active.Init(testSequence);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
