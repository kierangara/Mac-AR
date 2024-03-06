using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PuzzleManagerTests
{
    // Puzzle Manager Client Data Serialization
    [Test]
    public void PuzzleManagerSerialize()
    {
        MultiplayerPuzzleManager manager = new MultiplayerPuzzleManager();

        List<ulong> numbers = new List<ulong>{1, 2, 3, 4};
        List<byte> expectedBytes = new List<byte>{1, 0, 0, 0, 0, 0, 0, 0, 
                                                  2, 0, 0, 0, 0, 0, 0, 0, 
                                                  3, 0, 0, 0, 0, 0, 0, 0, 
                                                  4, 0, 0, 0, 0, 0, 0, 0};

        byte[] actualBytes = manager.objectToBytes(numbers);

        Assert.AreEqual(actualBytes.Length, expectedBytes.Count);

        for(int i = 0; i < actualBytes.Length; i++)
        {
            Assert.AreEqual(actualBytes[i], expectedBytes[i]);
        }
    }
}
