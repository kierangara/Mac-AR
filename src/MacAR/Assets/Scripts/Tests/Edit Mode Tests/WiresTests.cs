using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class WiresTests
{
    // Active Wires Initialization
    [Test]
    public void WiresTestsActiveInit()
    {
        var testSequence = new List<List<uint>>{new List<uint>{0, 0}, 
                                                new List<uint>{1, 1}, 
                                                new List<uint>{2, 2}, 
                                                new List<uint>{3, 3}};

        ActiveWires activeWires = new ActiveWires();
        activeWires.Init(testSequence);
        Assert.AreEqual(activeWires.m_sequence, testSequence);
    }

    // Active Wires Alternate Initialization
    [Test]
    public void WiresTestsAlternateActiveInit()
    {
        var testSequence = new List<List<uint>>{new List<uint>{0, 3}, 
                                                new List<uint>{1, 2}, 
                                                new List<uint>{2, 1}, 
                                                new List<uint>{3, 0}};

        ActiveWires activeWires = new ActiveWires();
        activeWires.Init(testSequence);
        Assert.AreEqual(activeWires.m_sequence, testSequence);
    }

    // Active Wires Initial Sequence
    [Test]
    public void WiresTestsInitialSequence()
    {
        var testSequence = new List<List<uint>>{new List<uint>{0, 3}, 
                                                new List<uint>{1, 2}, 
                                                new List<uint>{2, 1}, 
                                                new List<uint>{3, 0}};

        ActiveWires activeWires = new ActiveWires();
        activeWires.Init(testSequence);
        Assert.AreEqual(activeWires.currentSequence[0], -1);
    }

    // Active Wires Update Sequence
    [Test]
    public void WiresTestsUpdateSequence()
    {
        var testSequence = new List<List<uint>>{new List<uint>{0, 3}, 
                                                new List<uint>{1, 2}, 
                                                new List<uint>{2, 1}, 
                                                new List<uint>{3, 0}};

        ActiveWires activeWires = new ActiveWires();
        activeWires.Init(testSequence);
        activeWires.UpdateSequence(0, 0);
        Assert.AreEqual(activeWires.currentSequence[0], 0);
    }

    // Active Wires Wrong Sequence
    [Test]
    public void WiresTestsWrongSequence()
    {
        var testSequence = new List<List<uint>>{new List<uint>{0, 3}, 
                                                new List<uint>{1, 2}, 
                                                new List<uint>{2, 1}, 
                                                new List<uint>{3, 0}};

        ActiveWires activeWires = new ActiveWires();
        activeWires.Init(testSequence);
        Assert.IsTrue(!activeWires.UpdateSequence(0, 0));
    }
}
