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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity.Netcode;
using UnityEditor;
using NUnit.Framework.Constraints;
using System.IO;

public class IsometricTest : InputTestFixture
{
    GameObject isoPuzzle = Resources.Load<GameObject>("IsoMaster");
    GameObject isoInstance;

    public override void Setup()
    {
        SceneManager.LoadScene("Scenes/Test");
        base.Setup();

        var mouse = InputSystem.AddDevice<Mouse>();
        isoInstance = GameObject.Instantiate(isoPuzzle, Vector3.zero, Quaternion.identity);
    }


    [Test]
    public void IsometricSpawnSuccess()
    {

        NUnit.Framework.Assert.IsTrue(isoPuzzle.gameObject.activeSelf);
    }
    [Test]
    public void IsometricInitializeSuccessPasses()
    {
        NUnit.Framework.Assert.That(isoInstance, !Is.Null);
    }

    [Test]
    public void IsometricNoMultiplayerCatchExceptionSuccess()
    {
        IsometricPuzzleManager m_IsoPuzzle = new IsometricPuzzleManager();
        NUnit.Framework.Assert.Throws<NullReferenceException>(m_IsoPuzzle.InitializePuzzle);
    }

    [Test]
    public void IsometricSetCubesNoException()
    {


        IsometricPuzzleManager m_IsoPuzzle = new IsometricPuzzleManager();
        PrivateObject privateObjectIsoPuzzle = new PrivateObject(m_IsoPuzzle);
        object[] arguments = new object[1];
        privateObjectIsoPuzzle.Invoke("TestingSetup");
        privateObjectIsoPuzzle.Invoke("setCubes","1T");
        NUnit.Framework.Assert.True(true);
    }
}
