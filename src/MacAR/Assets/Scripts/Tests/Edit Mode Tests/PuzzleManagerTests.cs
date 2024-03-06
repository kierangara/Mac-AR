using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PuzzleManagerTests
{
    // Puzzle Manager Client Data Simple Serialization
    [Test]
    public void PuzzleManagerSimpleSerialize()
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

    // Puzzle Manager Client Data Complex Serialization
    [Test]
    public void PuzzleManagerComplexSerialize()
    {
        MultiplayerPuzzleManager manager = new MultiplayerPuzzleManager();

        List<ulong> numbers = new List<ulong>{9999999999, 100, 55756, 0};
        List<byte> expectedBytes = new List<byte>{0xFF, 0xE3, 0x0B, 0x54, 0x2, 0, 0, 0, 
                                                  0x64, 0, 0, 0, 0, 0, 0, 0, 
                                                  0xCC, 0xD9, 0, 0, 0, 0, 0, 0, 
                                                  0, 0, 0, 0, 0, 0, 0, 0};

        byte[] actualBytes = manager.objectToBytes(numbers);

        Assert.AreEqual(actualBytes.Length, expectedBytes.Count);

        for(int i = 0; i < actualBytes.Length; i++)
        {
            Assert.AreEqual(actualBytes[i], expectedBytes[i]);
        }
    }

    // Puzzle Manager Client Data Simple Deserialization
    [Test]
    public void PuzzleManagerSimpleDeserialize()
    {
        MultiplayerPuzzleManager manager = new MultiplayerPuzzleManager();

        List<ulong> expectedNumbers = new List<ulong>{1, 2, 3, 4};
        List<byte> givenBytes = new List<byte>{1, 0, 0, 0, 0, 0, 0, 0, 
                                               2, 0, 0, 0, 0, 0, 0, 0, 
                                               3, 0, 0, 0, 0, 0, 0, 0, 
                                               4, 0, 0, 0, 0, 0, 0, 0};

        List<ulong> actualNumbers = manager.bytesToObject(givenBytes.ToArray());

        Assert.AreEqual(actualNumbers.Count, expectedNumbers.Count);

        for(int i = 0; i < actualNumbers.Count; i++)
        {
            Assert.AreEqual(actualNumbers[i], expectedNumbers[i]);
        }
    }

    // Puzzle Manager Client Data Complex Deserialization
    [Test]
    public void PuzzleManagerComplexDeserialize()
    {
        MultiplayerPuzzleManager manager = new MultiplayerPuzzleManager();

        List<ulong> expectedNumbers = new List<ulong>{9999999999, 100, 55756, 0};
        List<byte> givenBytes = new List<byte>{0xFF, 0xE3, 0x0B, 0x54, 0x2, 0, 0, 0, 
                                                  0x64, 0, 0, 0, 0, 0, 0, 0, 
                                                  0xCC, 0xD9, 0, 0, 0, 0, 0, 0, 
                                                  0, 0, 0, 0, 0, 0, 0, 0};

        List<ulong> actualNumbers = manager.bytesToObject(givenBytes.ToArray());

        Assert.AreEqual(actualNumbers.Count, expectedNumbers.Count);

        for(int i = 0; i < actualNumbers.Count; i++)
        {
            Assert.AreEqual(actualNumbers[i], expectedNumbers[i]);
        }
    }

    // Puzzle Manager Full Serialization - Deserilaization Cycle
    [Test]
    public void PuzzleManagerRoundTrip()
    {
        MultiplayerPuzzleManager manager = new MultiplayerPuzzleManager();

        List<ulong> expectedNumbers = new List<ulong>{9999999999, 100, 55756, 0};

        List<ulong> actualNumbers = manager.bytesToObject(manager.objectToBytes(expectedNumbers));

        Assert.AreEqual(actualNumbers.Count, expectedNumbers.Count);

        for(int i = 0; i < actualNumbers.Count; i++)
        {
            Assert.AreEqual(actualNumbers[i], expectedNumbers[i]);
        }
    }
}
