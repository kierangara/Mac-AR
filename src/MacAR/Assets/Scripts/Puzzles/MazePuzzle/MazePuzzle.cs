using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;
using System;
using System.ComponentModel;
using Unity.Netcode;




public class MazePuzzle : PuzzleBase
{
    //Vector3 BallPosition=new Vector3(0,0,0);
    //Vector3 MazeRotation = new Vector3(0, 0, 0);

    [SerializeField] private GameObject maze;
    [SerializeField] private GameObject ball;
    [SerializeField] private TMP_Text debugText;

    [SerializeField] private MazeCell _mazeCellPrefab;

    [SerializeField] private int _mazeWidth;

    [SerializeField] private int _mazeLength;

    private MazeCell[,] _mazeGrid;

    public PuzzleData puzzleData;
    private int[,] mazeLayout;

    private float trackingYRot;
    private float trackingXRot;
    private float trackingZRot;
    private GameObject checkPoint;
    private static bool puzzleComplete = false;
    // Start is called before the first frame update
    private void Start()
    {
        Debug.Log("MazeSpawned");
    }

    public override void InitializePuzzle()
    {
        Debug.Log("Maze Initializing");
        //puzzleData =GameObject.Find("PuzzleInit").GetComponent<PuzzleData>();
        if (NetworkManager.Singleton.LocalClientId == puzzleData.connectedClients[0])
        {
            _mazeGrid = new MazeCell[_mazeWidth, _mazeLength];
            for (int x = 0; x < _mazeWidth; x++)
            {
                for (int z = 0; z < _mazeLength; z++)
                {
                    _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3((float)(x * 0.1 + maze.transform.position.x - 0.45), (float)-1.44, (float)(z * 0.1 + maze.transform.position.z - 0.45)), Quaternion.identity);
                    _mazeGrid[x, z].transform.parent = maze.transform;
                }
            }
            GenerateMaze(null, _mazeGrid[0, 0]);
            

            UpdateMazeAndBallTransformServerRpc(new Vector3(maze.transform.position.x - 0.45f, maze.transform.position.y + 0.1f, maze.transform.position.z - 0.45f), Quaternion.identity.eulerAngles);

            mazeLayout = new int[_mazeWidth, _mazeLength];
            for (int x = 0; x < _mazeWidth; x++)
            {
                for (int z = 0; z < _mazeLength; z++)
                {
                    mazeLayout[x, z] = (_mazeGrid[x, z].GetRightWall() ? 1 : 0) + (_mazeGrid[x, z].GetFrontWall() ? 2 : 0);
                }
            }
            GenerateMazeServerRpc(To1DArray(mazeLayout));
        }


        //GenerateMaze(null, _mazeGrid[0,0]);
        //ball.transform.position= new Vector3(maze.transform.position.x - 0.45f, maze.transform.position.y + 0.1f, maze.transform.position.z - 0.45f);
        //maze.transform.rotation = Quaternion.identity;
        Input.gyro.enabled = true;

        if (_mazeGrid == null)
        {
            RequestMazeServerRpc();
        }


    }
    [ServerRpc(RequireOwnership = false)]
    public void RequestMazeServerRpc()
    {
        SendMazeClientRpc();
    }


   [ClientRpc]
    public void SendMazeClientRpc()
    {


        if(NetworkManager.Singleton.LocalClientId == puzzleData.connectedClients[0])
        {
            GenerateMazeServerRpc ( To1DArray( mazeLayout));
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestPuzzleDataServerRpc()
    {
        RequestPuzzleDataClientRpc();
    }

    [ClientRpc]
    public void RequestPuzzleDataClientRpc()
    {
        if(puzzleData.connectedClients!=null)
        {
            SendPuzzleDataServerRpc(puzzleData.connectedClients.ToArray());
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SendPuzzleDataServerRpc(ulong[] p)
    {
        SendPuzzleDataClientRpc(p);
    }

    [ClientRpc]
    public void SendPuzzleDataClientRpc(ulong[] p)
    {
        puzzleData.connectedClients = p.ToList();
        //Debug.Log("PuzzleDataUpdated");
    }



    [ServerRpc(RequireOwnership = false)]
    public void GenerateMazeServerRpc(int[] mazeLayouts)
    {
        GenerateMazeClientRpc(mazeLayouts);
    }

    [ClientRpc]
    public void GenerateMazeClientRpc(int[] mazeLayouts)
    {
        if(_mazeGrid==null)
        {
            convertLayoutToGrid(mazeLayouts);
            //Debug.Log("Maze should get updated");
        }
    }

    [ServerRpc(RequireOwnership =false)]
    public void UpdateMazeAndBallTransformServerRpc(Vector3 ballPosition, Vector3 mazeRotation)
    {
        UpdateMazeAndBallTransformClientRpc(ballPosition, mazeRotation);
        //GenerateMazeClientRpc(newColor);
    }
    [ClientRpc]
    public void UpdateMazeAndBallTransformClientRpc(Vector3 ballPosition, Vector3 mazeRotation)
    {
        ball.transform.position = ballPosition;
        maze.transform.eulerAngles = mazeRotation;
        //GenerateMazeClientRpc(newColor);
    }

    public void resetRotationPress()
    {
        trackingXRot = 0;
        trackingYRot = 0;
        trackingZRot = 0;
    }


    static int[] To1DArray(int[,] input)
    {
        // Step 1: get total size of 2D array, and allocate 1D array.
        int size = input.Length;
        int[] result = new int[size];

        // Step 2: copy 2D array elements into a 1D array.
        int write = 0;
        for (int i = 0; i <= input.GetUpperBound(0); i++)
        {
            for (int z = 0; z <= input.GetUpperBound(1); z++)
            {
                result[write++] = input[i, z];
            }
        }
        // Step 3: return the new array.
        return result;
    }


    private void convertLayoutToGrid(int[] mazeLayouts)
    {
        _mazeGrid = new MazeCell[_mazeWidth, _mazeLength];
        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeLength; z++)
            {
                _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3((float)(x * 0.1 + maze.transform.position.x - 0.45), (float)-1.44, (float)(z * 0.1 + maze.transform.position.z - 0.45)), Quaternion.identity);
                _mazeGrid[x, z].transform.parent = maze.transform;
            }
        }
        mazeLayout = Make2DArray<int>(mazeLayouts, _mazeWidth, _mazeLength);


        for (int x = 0; x < _mazeWidth*_mazeLength; x++)
        {
            for (int z = 0; z < _mazeLength; z++)
            {
                _mazeGrid[x, z].Visit();
                if (mazeLayout[x,z]%2==1)
                {
                    _mazeGrid[x, z].ClearRightWall();
                    _mazeGrid[x+1,z].ClearLeftWall();

                }
                if (mazeLayout[x,z]>=2)
                {
                    _mazeGrid[x, z].ClearFrontWall();
                    _mazeGrid[x,z+1].ClearRearWall();
                }
            }
        }

    }


    private static T[,] Make2DArray<T>(T[] input, int height, int width)
    {
        T[,] output = new T[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                output[i, j] = input[i * width + j];
            }
        }
        return output;
    }


    private void GenerateMaze(MazeCell previousCell,MazeCell currentCell)
    {
        currentCell.IsVisited=true;
        ClearWalls(previousCell, currentCell);

        MazeCell nextCell;

        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }

        }while(nextCell != null);

        
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentcell)
    {
        var unvisitedCells=GetUnvisitedCells(currentcell);

        return unvisitedCells.OrderBy(_ => UnityEngine.Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)Mathf.Round((float)(currentCell.transform.position.x-(maze.transform.position.x - 0.45)) *10);
        int z = (int)Mathf.Round((float)(currentCell.transform.position.z-(maze.transform.position.z - 0.45)) *10);

        if(x+1< _mazeWidth)
        {
            var cellToRight = _mazeGrid[x + 1, z];
            if(cellToRight.IsVisited==false)
            {
                yield return cellToRight;
            }
        }

        if(x-1>=0)
        {
            var cellToLeft = _mazeGrid[x - 1, z];
            if (cellToLeft.IsVisited == false)
            {
                yield return cellToLeft;
            }
        }

        if (z + 1 < _mazeLength)
        {
            var cellToFront = _mazeGrid[x , z+1];
            if (cellToFront.IsVisited == false)
            {
                yield return cellToFront;
            }
        }

        if (z - 1 >= 0)
        {
            var cellToRear = _mazeGrid[x, z - 1];
            if (cellToRear.IsVisited == false)
            {
                yield return cellToRear;
            }
        }
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if(previousCell==null)
        {
            return;
        }
        if(previousCell.transform.position.x<currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearRearWall();
            return;
        }

        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearRearWall();
            currentCell.ClearFrontWall();
            return;
        }


    }

    private void Awake()
    {
        maze.transform.rotation = Quaternion.identity;
        Input.gyro.enabled = true;
        GenerateMaze(2);
        //Debug.Log("Reached Awake");
    }

    private void GenerateMaze(int NumberOfPlayers)
    {
        maze.transform.rotation = Quaternion.identity;
    }

    private void RotateMaze(Vector3 phoneRotation)
    {
        maze.transform.eulerAngles = phoneRotation;
        //maze.gameObject.transform.rotation = phoneRotation;//Quaternion.Lerp(phoneRotation, maze.transform.rotation,(float)0.5); //Quaternion.LookRotation((phoneRotation + maze.transform.rotation.eulerAngles)/2, Vector3.up);
        //maze.gameObject.transform.rotation = Quaternion.FromToRotation(Vector3.right, Vector3.down);
        //maze.transform.eulerAngles=new Vector3(45,45,0);
        //Debug.Log(maze.transform.rotation.ToString());
    }

    public void BallHitsCheckpoint(GameObject checkpoint)
    {
        checkPoint= checkpoint;
    }

    public void BallHitsGoal()
    {
        puzzleData.completePuzzle.CompletePuzzleServerRpc(0);
        puzzleComplete = true;

    }
    // Update is called once per frame
    void Update()
    {

        if (puzzleData.connectedClients == null || puzzleData.connectedClients.Count == 0)
        {
            RequestPuzzleDataServerRpc();
        }
        else if (_mazeGrid == null)
        {
            RequestMazeServerRpc();
        }
        else if (puzzleComplete) 
        {
            //debugText.text = "Congrats you completed the puzzle!";
        }
        else if(NetworkManager.Singleton.LocalClientId == puzzleData.connectedClients[0])
        {

            if (ball.transform.position.y < maze.transform.position.y - 0.1)
            {
                ball.transform.position = checkPoint.transform.position;
               // ball.transform.position = new Vector3(ball.transform.position.x, maze.transform.position.y + 0.1f /*(float)-0.95*/, ball.transform.position.z);
            }
            if (ball.transform.position.x > maze.transform.position.x + 0.8 || ball.transform.position.x < maze.transform.position.x - 0.8)
            {
                ball.transform.position = checkPoint.transform.position;
                //ball.transform.position = new Vector3(maze.transform.position.x - 0.45f, ball.transform.position.y, ball.transform.position.z);
            }
            if (ball.transform.position.z > maze.transform.position.z + 0.8 || ball.transform.position.z < maze.transform.position.z - 0.8)
            {
                ball.transform.position = checkPoint.transform.position;
                //ball.transform.position = new Vector3(ball.transform.position.x, ball.transform.position.y, maze.transform.position.z - 0.45f);
            }

            if (SystemInfo.supportsGyroscope)
            {
                Input.gyro.enabled = true;
                trackingXRot += -Input.gyro.rotationRateUnbiased.x;
                trackingYRot += -Input.gyro.rotationRateUnbiased.y;
                trackingZRot += -Input.gyro.rotationRateUnbiased.z;
                Vector3 phoneRotation = new Vector3(trackingXRot * 3, trackingZRot, trackingYRot * 3);
                UpdateMazeAndBallTransformServerRpc(ball.transform.position,phoneRotation);
                //RotateMaze(phoneRotation);
                //Debug.Log(phoneRotation.ToString());
                //debugText.text = phoneRotation.ToString();
            }
            else
            {
                //debugText.text = "This phone does not support Gyroscope";
            }
        }

        

        //RotateMaze (GyroToUnity(phoneRotation));
        //Debug.Log(phoneRotation.ToString());


    }

    private static Quaternion GyroToUnity(Quaternion q)
    {
        return Quaternion.Euler(90, 0, 0) * new Quaternion(q.x, q.y, -q.z, -q.w);
    }
}
