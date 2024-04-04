//Created by Matthew Collard
//Last Updated: 2024/04/04
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using Unity.Netcode;
//The Maze Puzzle contains all the code responsible for the maze puzzle
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
    // Initializes the puzzle, Generates the puzzle ONLY for connectedClinets[0] which is the host of the lobby. 
    public override void InitializePuzzle()
    {
        puzzleId = PuzzleConstants.MAZE_ID;

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
        setColumns();


    }
    //requests for the maze to be sent to the non-hosts
    [ServerRpc(RequireOwnership = false)]
    public void RequestMazeServerRpc()
    {
        SendMazeClientRpc();
    }

    //If you are the host, converts the maze into a 1D array and sends it to all the clients
   [ClientRpc]
    public void SendMazeClientRpc()
    {
        if(NetworkManager.Singleton.LocalClientId == puzzleData.connectedClients[0])
        {
            GenerateMazeServerRpc ( To1DArray( mazeLayout));
        }
    }
    //Requests the puzzle data from the host
    [ServerRpc(RequireOwnership = false)]
    public void RequestPuzzleDataServerRpc()
    {
        RequestPuzzleDataClientRpc();
    }
    //Host sends the puzzle data to the clients
    [ClientRpc]
    public void RequestPuzzleDataClientRpc()
    {
        if(puzzleData.connectedClients!=null)
        {
            SendPuzzleDataServerRpc(puzzleData.connectedClients.ToArray());
        }
    }
    //Sends puzzle data to all clients
    [ServerRpc(RequireOwnership = false)]
    public void SendPuzzleDataServerRpc(ulong[] p)
    {
        SendPuzzleDataClientRpc(p);
    }
    //changes the puzzle data structure to the client list
    [ClientRpc]
    public void SendPuzzleDataClientRpc(ulong[] p)
    {
        puzzleData.connectedClients = p.ToList();
        //Debug.Log("PuzzleDataUpdated");
    }


    //Sends the generated maze to all the clients
    [ServerRpc(RequireOwnership = false)]
    public void GenerateMazeServerRpc(int[] mazeLayouts)
    {
        GenerateMazeClientRpc(mazeLayouts);
    }
    //converts the layout to a visible maze on the screen
    [ClientRpc]
    public void GenerateMazeClientRpc(int[] mazeLayouts)
    {
        if(_mazeGrid==null)
        {
            ConvertLayoutToGrid(mazeLayouts);
            //Debug.Log("Maze should get updated");
        }
    }
    //Sends updated ball position and maze rotation to clients
    [ServerRpc(RequireOwnership =false)]
    public void UpdateMazeAndBallTransformServerRpc(Vector3 ballPosition, Vector3 mazeRotation)
    {
        UpdateMazeAndBallTransformClientRpc(ballPosition, mazeRotation);
        //GenerateMazeClientRpc(newColor);
    }
    //Updates the ball position and maze rotation
    [ClientRpc]
    public void UpdateMazeAndBallTransformClientRpc(Vector3 ballPosition, Vector3 mazeRotation)
    {
        ball.transform.position = ballPosition;
        maze.transform.eulerAngles = mazeRotation;
        //GenerateMazeClientRpc(newColor);
    }
    //Called from a button, resets the maze rotation, host only
    public void resetRotationPress()
    {
        trackingXRot = 0;
        trackingYRot = 0;
        trackingZRot = 0;
    }
    // sets the visual look of the maze for the user
    private void setColumns()
    {
        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeLength; z++)
            {
                if(x==0)
                {
                    _mazeGrid[x, z].ShowColumnFL();
                }
                if(z==0)
                {
                    _mazeGrid[x,z].ShowColumnBL();
                }
                if(z==_mazeWidth-1)
                {
                    _mazeGrid[x,z].ShowColumnBR();
                }
                if(x!=0)
                {
                    _mazeGrid[x,z].ClearLeftWall();
                }
                if(z!=0)
                {
                    _mazeGrid[x, z].ClearRearWall();
                }
            }
        }
    }


    public bool returnCompletionStatus()
    {
        return puzzleComplete;
    }

    //Converts 2D array to 1D array
    private int[] To1DArray(int[,] input)
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
    //Function for Automated Testing
    private void setMazeDimensions()
    {
        _mazeLength = 10;
        _mazeWidth = 10;
        maze = Resources.Load<GameObject>("MazePuzzle");
        maze=GameObject.Instantiate(maze, new Vector3(0, 0, 0), Quaternion.identity);
        _mazeCellPrefab = Resources.Load<MazeCell>("MazeCell");
    }

    //Converts a 1D array of ints to a maze puzzle on the screen. 
    private void ConvertLayoutToGrid(int[] mazeLayouts)
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
        mazeLayout = Make2DArray(mazeLayouts, _mazeWidth, _mazeLength);


        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeLength; z++)
            {
                _mazeGrid[x, z].Visit();
                if ((mazeLayout[x,z]%2==1)&&(x<_mazeWidth-1))
                {
                    _mazeGrid[x, z].ClearRightWall();
                    _mazeGrid[x+1,z].ClearLeftWall();

                }
                if ((mazeLayout[x,z]>=2) && (z < _mazeLength-1))
                {
                    _mazeGrid[x, z].ClearFrontWall();
                    _mazeGrid[x,z+1].ClearRearWall();
                }
            }
        }
        setColumns();

    }

    //Converts 1D array to a 2D array
    private int[,] Make2DArray(int[] input, int height, int width)
    {
        int[,] output = new int[height, width];
        for (int i = 0; i < height; i++)
        {
            for (int j = 0; j < width; j++)
            {
                output[i, j] = input[i * width + j];
            }
        }
        return output;
    }

    //Starts the maze generation algorithm
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
    //Finds the next unvisited cell for the maze gen algo
    private MazeCell GetNextUnvisitedCell(MazeCell currentcell)
    {
        var unvisitedCells=GetUnvisitedCells(currentcell);

        return unvisitedCells.OrderBy(_ => UnityEngine.Random.Range(1, 10)).FirstOrDefault();
    }
    //gets a list of all the unvisited maze cells
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
    //clears two walls between neighbouring cells
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
    //called when puzzle is instantiated, enables the gyroscope on the phone
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
    //changes the checkpoint when the ball passes over it
    public void BallHitsCheckpoint(GameObject checkpoint)
    {
        checkPoint= checkpoint;
    }
    //completes the game when the ball hits the goal
    public void BallHitsGoal()
    {
        puzzleData.completePuzzle.CompletePuzzleServerRpc(0, PuzzleConstants.MAZE_ID);
        puzzleComplete = true;

    }
    // Update is called once per frame
    // Update updates each user with the up to date ball position and maze rotation. 
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

            if (SystemInfo.supportsGyroscope)//tracks the x,y,z rotation of the phone and updates each user
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
}
