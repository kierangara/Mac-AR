//Created by Ethan Kannampuzha
//Last Updated: 2024/04/04
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using Unity.Netcode;
using UnityEditor;
using NUnit.Framework.Constraints;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Assert = NUnit.Framework.Assert;
//SimonSaysTests contains verification/testing code for SimonSaysPuzzle
public class SimonSaysTests : InputTestFixture
{
    public GameObject simonSaysPuzzle = Resources.Load<GameObject>("SimonSaysPuzzle");
    public GameObject characterInstance;
    SimonButton red = new SimonButton();
    SimonButton blue = new SimonButton();
    SimonButton green = new SimonButton();
    SimonButton yellow = new SimonButton();

    public override void Setup()
    {
        SceneManager.LoadScene("Scenes/Test");
        base.Setup();

        characterInstance = GameObject.Instantiate(simonSaysPuzzle, Vector3.zero, Quaternion.identity);
    }

    //Testing that Simon Says Puzzle spawns in correctly
    [Test]
    public void SimonSaysSpawnTest()
    {
        NUnit.Framework.Assert.IsTrue(simonSaysPuzzle.activeSelf);
    }

    //Testing that Simon Says Puzzle is not null when spawning in
    [Test]
    public void SimonSaysObjectNonNullTest()
    {
        NUnit.Framework.Assert.That(characterInstance, !Is.Null);
    }

    //Testing that Simon Says level increment function works correctly
    [Test]
    public void IncrementLevelTest()
    {
        SimonSaysPuzzle simonPuzzle = new SimonSaysPuzzle();
        simonPuzzle.level = 1;
        simonPuzzle.IncrementLevel();
        NUnit.Framework.Assert.AreEqual(2, simonPuzzle.level);

    }

    //Testing that user input on coloured buttons are tracked and stored correctly
    [Test]
    public void TrackUserInputTest()
    {
        red.id = 0;
        blue.id = 1;
        green.id = 2;
        yellow.id = 3;

        SimonSaysPuzzle simonPuzzle = new SimonSaysPuzzle();
        simonPuzzle.generatedSequence.Add(blue.id);
        simonPuzzle.generatedSequence.Add(green.id);
        simonPuzzle.TrackUserInput(blue);

        NUnit.Framework.Assert.AreEqual(simonPuzzle.generatedSequence[0], simonPuzzle.playerSequence[0]);

    }

    //Testing that BeginSimonSays function clears the current player input of coloured buttons
    [UnityTest]
    public IEnumerator BeginSimonSaysClearsPlayerSequenceTest()
    {
        red.id = 0;
        blue.id = 1;
        green.id = 2;
        yellow.id = 3;
        SimonSaysPuzzle simonPuzzle = new SimonSaysPuzzle();
        simonPuzzle.playerSequence.Add(blue.id);
        simonPuzzle.playerSequence.Add(red.id);
        simonPuzzle.counter = 1;
        yield return simonPuzzle.BeginSimonSays();
        //Debug.Log(simonPuzzle.playerSequence[0]);
        NUnit.Framework.Assert.IsEmpty(simonPuzzle.playerSequence);
    }

    //Testing that BeginSimonSays function clears the current generated colour sequence
    [UnityTest]
    public IEnumerator BeginSimonSaysClearsGeneratedSequenceTest()
    {

        SimonSaysPuzzle simonPuzzle = new SimonSaysPuzzle();
        simonPuzzle.generatedSequence.Add(blue.id);
        simonPuzzle.generatedSequence.Add(red.id);
        simonPuzzle.counter = 1;
        yield return simonPuzzle.BeginSimonSays();
        //Debug.Log(simonPuzzle.playerSequence[0]);
        NUnit.Framework.Assert.IsEmpty(simonPuzzle.generatedSequence);
    }

}
