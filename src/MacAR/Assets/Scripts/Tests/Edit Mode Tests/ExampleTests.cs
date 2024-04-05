using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EditTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void ExampleTestsSimplePasses()
    {
        Assert.IsTrue(true);
    }

    // A Test that utlizes an object
    [Test]
    public void ExampleTestsExistingObj()
    {
        var testSequence = new List<List<uint>>{new List<uint>{0, 0}, 
                                                new List<uint>{1, 1}, 
                                                new List<uint>{2, 2}, 
                                                new List<uint>{3, 3}};

        ActiveWires activeWires = new ActiveWires();
        activeWires.Init(testSequence);
        Assert.AreEqual(activeWires.m_sequence, testSequence);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator EditTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
