using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

// Puzzle Interaction Acceptance Tests
public class PuzzleInteraction
{
    // Backend updates on puzzle interaction
    [Test]
    public void PuzzleInteraction_PI3()
    {
        // Wires Example
        var testSequence = new List<List<uint>>{new List<uint>{0, 3}, 
                                                new List<uint>{1, 2}, 
                                                new List<uint>{2, 1}, 
                                                new List<uint>{3, 0}};

        // Simulate User Connecting Red to Red
        ActiveWires activeWires = new ActiveWires();
        activeWires.Init(testSequence);
        activeWires.UpdateSequence(0, 0);

        // Check That Backend Updates
        Assert.AreEqual(activeWires.currentSequence[0], 0);
    }
}
