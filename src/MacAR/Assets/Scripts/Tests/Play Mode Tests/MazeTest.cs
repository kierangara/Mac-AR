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

public class MazeTest : InputTestFixture
{
    GameObject mazePuzzle = Resources.Load<GameObject>("MazePuzzle");
    GameObject characterInstance;

    public override void Setup()
    {
        SceneManager.LoadScene("Scenes/Test");
        base.Setup();

        var mouse = InputSystem.AddDevice<Mouse>();
        characterInstance = GameObject.Instantiate(mazePuzzle, Vector3.zero, Quaternion.identity);
    }


    [Test]
    public void MazeSpawnSuccessPasses()
    {

        NUnit.Framework.Assert.IsTrue(mazePuzzle.activeSelf);
    }
    [Test]
    public void MazeInitializeSuccessPasses()
    {
        NUnit.Framework.Assert.That(characterInstance, !Is.Null);
    }
    [Test]
    public void Maze2DArrayTo1DSuccess()
    {
        int[,] matrix = new int[3,3];
        int[] solution = new int[9];
        for (int i=0; i<matrix.GetLength(0); i++) 
        { 
            for (int j=0; j<matrix.GetLength(1); j++)
            {
                System.Random random = new System.Random();
                matrix[i, j] = random.Next();
                solution[3 * i + j] = matrix[i, j];
            }
        }

        MazePuzzle m_MazePuzzle = new MazePuzzle();

        PrivateObject privateObjectMazePuzzle = new PrivateObject(m_MazePuzzle);
        object[] arguments = new object[1];
        int[] returnedMatrix = (int[])privateObjectMazePuzzle.Invoke("To1DArray", (object)matrix);
        NUnit.Framework.Assert.True(solution.SequenceEqual(returnedMatrix));
    }

    [Test]
    public void Maze2DArrayTo1DFail()
    {
        int[,] matrix = new int[5, 4];
        int[] solution = new int[20];
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                System.Random random = new System.Random();
                matrix[i, j] = random.Next();
                solution[i + 3*j] = matrix[i, j];
            }
        }

        MazePuzzle m_MazePuzzle = new MazePuzzle();

        PrivateObject privateObjectMazePuzzle = new PrivateObject(m_MazePuzzle);
        object[] arguments = new object[1];
        int[] returnedMatrix = (int[])privateObjectMazePuzzle.Invoke("To1DArray", (object)matrix);
        NUnit.Framework.Assert.False(solution.SequenceEqual(returnedMatrix));
    }

    [Test]
    public void Maze1DArrayTo2DSuccess()
    {
        int[,] matrix = new int[3, 3];
        int[] solution = new int[9];
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                System.Random random = new System.Random();
                matrix[i, j] = random.Next();
                solution[3 * i + j] = matrix[i, j];
            }
        }
        
        MazePuzzle m_MazePuzzle = new MazePuzzle();
        PrivateObject privateObjectMazePuzzle = new PrivateObject(m_MazePuzzle);
        object[] arguments = new object[1];
        int[,] returnedMatrix = (int[,])privateObjectMazePuzzle.Invoke("Make2DArray", new object[] { solution, matrix.GetLength(0), matrix.GetLength(1) });
        bool successFull=true;
        for(int i=0;i< returnedMatrix.GetLength(0);i++)
        {
            int[] array1=Enumerable.Range(0, returnedMatrix.GetLength(1))
                .Select(x => matrix[i, x])
                .ToArray();
            int[] array2= Enumerable.Range(0, matrix.GetLength(1))
                .Select(x => matrix[i, x])
                .ToArray();
            successFull = successFull & array1.SequenceEqual(array2);
        }
        NUnit.Framework.Assert.True(successFull);
    }
    [Test]
    public void Maze1DArrayTo2DFail()
    {
        int[,] matrix = new int[5, 4];
        int[] solution = new int[20];
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                System.Random random = new System.Random();
                matrix[i, j] = random.Next();
                solution[i + 3*j] = matrix[i, j];
            }
        }
        MazePuzzle m_MazePuzzle = new MazePuzzle();
        PrivateObject privateObjectMazePuzzle = new PrivateObject(m_MazePuzzle);
        object[] arguments = new object[1];
        int[,] returnedMatrix = (int[,])privateObjectMazePuzzle.Invoke("Make2DArray", new object[] { solution, matrix.GetLength(0), matrix.GetLength(1) });
        bool successFull = true;
        for (int i = 0; i < returnedMatrix.GetLength(0); i++)
        {
            int[] array1 = Enumerable.Range(0, returnedMatrix.GetLength(1))
                .Select(x => returnedMatrix[i, x])
                .ToArray();
            int[] array2 = Enumerable.Range(0, matrix.GetLength(1))
                .Select(x => matrix[i, x])
                .ToArray();
            successFull = successFull & array1.SequenceEqual(array2);

        }
        NUnit.Framework.Assert.False(successFull);
    }

    [Test]
    public void ClearWallsPositive()
    {
        MazeCell cell1,cell2 = new MazeCell();
        cell1 = MazeCell.Instantiate(Resources.Load<MazeCell>("MazeCell"), new Vector3(1,0,0), Quaternion.identity);
        cell2 = MazeCell.Instantiate(Resources.Load<MazeCell>("MazeCell"), Vector3.zero, Quaternion.identity);

        MazePuzzle m_MazePuzzle = new MazePuzzle();

        PrivateObject privateObjectMazePuzzle = new PrivateObject(m_MazePuzzle);
        privateObjectMazePuzzle.Invoke("ClearWalls", new object[] { cell1,cell2 });


        NUnit.Framework.Assert.True((cell1.GetLeftWall())&&(cell2.GetRightWall()));

    }
    [Test]
    public void convertLayoutToGridSuccess()
    {
        int[] mazeLayout=new int[100];
        for(int i =0;i<100;i++)
        {
            mazeLayout[i] = UnityEngine.Random.Range(0, 3);
        }
        MazePuzzle m_MazePuzzle = new MazePuzzle();
        PrivateObject privateObjectMazePuzzle = new PrivateObject(m_MazePuzzle); 
        privateObjectMazePuzzle.Invoke("setMazeDimensions");
        privateObjectMazePuzzle.Invoke("convertLayoutToGrid", new object[] {mazeLayout });
        NUnit.Framework.Assert.True(true);
    }



    [Test]
    public void GetUnvisitedCellsSuccess()
    {
        int[] mazeLayout = new int[100];
        for (int i = 0; i < 100; i++)
        {
            mazeLayout[i] = UnityEngine.Random.Range(0, 3);
        }
        MazeCell cell = new MazeCell();
        cell = MazeCell.Instantiate(Resources.Load<MazeCell>("MazeCell"), new Vector3((float)-0.45, (float)-1.44, (float)-0.45), Quaternion.identity);

        MazePuzzle m_MazePuzzle = new MazePuzzle();
        PrivateObject privateObjectMazePuzzle = new PrivateObject(m_MazePuzzle);
        privateObjectMazePuzzle.Invoke("setMazeDimensions");
        privateObjectMazePuzzle.Invoke("convertLayoutToGrid", new object[] { mazeLayout });
        NUnit.Framework.CollectionAssert.AreEqual(new List<MazeCell>(), (IEnumerable<MazeCell>)privateObjectMazePuzzle.Invoke("GetUnvisitedCells", new object[] { cell }));
    }
}
