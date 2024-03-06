using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using System.Linq;
using static Unity.VisualScripting.Member;
using System;
using Unity.Collections.LowLevel.Unsafe;

public class IsometricPuzzleManager : PuzzleBase
{
    // Start is called before the first frame update
    [SerializeField] private TMP_InputField solutionField;
    [SerializeField] private IsometricCube _cubePrefab;
    [SerializeField] private int _cubeWidth;
    [SerializeField] private int _cubeLength;
    [SerializeField] private int _cubeHeight;
    public PuzzleData puzzleData;
    private IsometricCube[,,] _cubesCollection;
    private bool[,,] activeGrid;
    private string[] _cubeNames;
    private int cubeIndex;
    public override void InitializePuzzle()
    {
        

        _cubesCollection = new IsometricCube[_cubeWidth,_cubeHeight ,_cubeLength];
        activeGrid = new bool[_cubeWidth, _cubeHeight, _cubeLength];
        for (int x = 0; x < _cubeWidth; x++)
        {
            for (int y=0; y < _cubeLength; y++)
            {
                for (int z = 0; z < _cubeLength; z++)
                {
                    _cubesCollection[x,y,z] = Instantiate(_cubePrefab, new Vector3((float)(x * 0.2 + this.transform.position.x - 0.5), (float)(y * 0.2 +this.transform.position.y - 0.5), (float)(z * 0.2 + this.transform.position.z - 0.5)), Quaternion.identity);
                    _cubesCollection[x,y,z].transform.parent = this.transform;
                }
            }
        }
        cubeIndex = 0;
        SendPuzzleDataServerRpc(puzzleData.connectedClients.ToArray());
        SetIsometricPuzzlesServerRpc("1T 2W 3I 4L 5I 6G 7H 8T");

        setCubes(_cubeNames[cubeIndex]);
    }
    //--------------------------------------------------------------------//
    //This function will set the current cube layout
    private void setCubes(string key)
    {
        for (int x = 0; x < _cubeWidth; x++)
        {
            for (int y = 0; y < _cubeLength; y++)
            {
                for (int z = 0; z < _cubeLength; z++)
                {
                    activeGrid[x, y, z] = false;
                }
            }
        }
        switch (key)
        {
            case "1T":
                activeGrid[2, 0, 2] = true;
                activeGrid[2, 1, 2] = true;
                activeGrid[2, 2, 2] = true;
                activeGrid[2, 3, 2] = true;
                activeGrid[2, 4, 2] = true;
                activeGrid[1, 4, 1] = true;
                activeGrid[1, 4, 3] = true;
                activeGrid[2, 4, 4] = true;
                activeGrid[2, 4, 0] = true;
                break;
            case "2W":
                activeGrid[0, 0, 0] = true;
                activeGrid[0, 0, 3] = true;
                activeGrid[0, 1, 1] = true;
                activeGrid[0, 2, 2] = true;
                activeGrid[0, 3, 3] = true;
                activeGrid[0, 4, 2] = true;
                activeGrid[1, 1, 1] = true;
                activeGrid[2, 2, 2] = true;
                activeGrid[3, 1, 1] = true;
                activeGrid[4, 0, 1] = true;
                activeGrid[4, 0, 2] = true;
                activeGrid[4, 1, 1] = true;
                activeGrid[4, 2, 2] = true;
                activeGrid[4, 3, 0] = true;
                activeGrid[4, 4, 1] = true;
                break;
            case "3I":
                activeGrid[0, 0, 0] = true;
                activeGrid[0, 0, 2] = true;
                activeGrid[0, 4, 1] = true;
                activeGrid[0, 4, 3] = true;
                activeGrid[1, 0, 0] = true;
                activeGrid[1, 4, 0] = true;
                activeGrid[2, 0, 2] = true;
                activeGrid[2, 1, 1] = true;
                activeGrid[2, 2, 0] = true;
                activeGrid[2, 3, 2] = true;
                activeGrid[2, 4, 1] = true;
                activeGrid[3, 0, 0] = true;
                activeGrid[3, 4, 0] = true;
                activeGrid[4, 0, 1] = true;
                activeGrid[4, 0, 3] = true;
                activeGrid[4, 4, 0] = true;
                activeGrid[4, 4, 2] = true;
                break;
            case "4L":
                activeGrid[0, 0, 0] = true;
                activeGrid[0, 1, 0] = true;
                activeGrid[0, 2, 0] = true;
                activeGrid[0, 2, 1] = true;
                activeGrid[3, 2, 2] = true;
                activeGrid[0, 3, 0] = true;
                activeGrid[0, 3, 2] = true;
                activeGrid[2, 3, 2] = true;
                activeGrid[0, 4, 0] = true;
                activeGrid[1, 4, 2] = true;
                break;
            case "5I":
                activeGrid[0, 0, 2] = true;
                activeGrid[0, 4, 2] = true;
                activeGrid[1, 0, 1] = true;
                activeGrid[1, 4, 0] = true;
                activeGrid[2, 0, 0] = true;
                activeGrid[2, 1, 0] = true;
                activeGrid[2, 2, 0] = true;
                activeGrid[2, 2, 1] = true;
                activeGrid[2, 2, 2] = true;
                activeGrid[2, 2, 3] = true;
                activeGrid[2, 3, 3] = true;
                activeGrid[2, 4, 1] = true;
                activeGrid[3, 0, 3] = true;
                activeGrid[3, 4, 0] = true;
                activeGrid[4, 0, 1] = true;
                activeGrid[4, 4, 2] = true;
                break;
            case "6G":
                activeGrid[0, 0, 3] = true;
                activeGrid[0, 1, 4] = true;
                activeGrid[0, 2, 2] = true;
                activeGrid[0, 3, 4] = true;
                activeGrid[0, 4, 3] = true;
                activeGrid[1, 0, 2] = true;
                activeGrid[1, 2, 0] = true;
                activeGrid[1, 4, 0] = true;
                activeGrid[2, 0, 0] = true;
                activeGrid[2, 2, 4] = true;
                activeGrid[2, 4, 1] = true;
                activeGrid[3, 0, 1] = true;
                activeGrid[3, 0, 4] = true;
                activeGrid[3, 1, 0] = true;
                activeGrid[3, 2, 1] = true;
                activeGrid[3, 4, 2] = true;
                activeGrid[3, 4, 4] = true;
                break;
            case "7H":
                activeGrid[0, 0, 0] = true;
                activeGrid[0, 1, 0] = true;
                activeGrid[0, 1, 4] = true;
                activeGrid[0, 2, 0] = true;
                activeGrid[0, 3, 0] = true;
                activeGrid[0, 4, 0] = true;
                activeGrid[1, 2, 1] = true;
                activeGrid[1, 3, 4] = true;
                activeGrid[2, 2, 2] = true;
                activeGrid[2, 2, 4] = true;
                activeGrid[3, 0, 4] = true;
                activeGrid[3, 2, 3] = true;
                activeGrid[3, 4, 4] = true;
                break;
            case "8T":
                activeGrid[0, 0, 2] = true;
                activeGrid[0, 1, 2] = true;
                activeGrid[0, 3, 2] = true;
                activeGrid[0, 4, 0] = true;
                activeGrid[0, 4, 2] = true;
                activeGrid[1, 0, 2] = true;
                activeGrid[1, 2, 2] = true;
                activeGrid[1, 4, 4] = true;
                activeGrid[2, 0, 2] = true;
                activeGrid[2, 2, 2] = true;
                activeGrid[2, 4, 3] = true;
                activeGrid[3, 0, 2] = true;
                activeGrid[3, 1, 2] = true;
                activeGrid[3, 3, 2] = true;
                activeGrid[3, 4, 1] = true;
                break;
            default:
                break;

        }

        for (int x = 0; x < _cubeWidth; x++)
        {
            for (int y = 0; y < _cubeLength; y++)
            {
                for (int z = 0; z < _cubeLength; z++)
                {
                    _cubesCollection[x, y, z].cube.SetActive(activeGrid[x, y, z]);
                }
            }
        }
    }

    private void TestingSetup()
    {
        _cubeHeight = 5;
        _cubeWidth= 5;
        _cubeLength = 5;
        GameObject tempTransform = Resources.Load<GameObject>("IsoMaster");
        tempTransform = GameObject.Instantiate(tempTransform, new Vector3(0, 0, 0), Quaternion.identity);
        _cubePrefab = Resources.Load<IsometricCube>("IsoCube");
        _cubesCollection = new IsometricCube[_cubeWidth, _cubeHeight, _cubeLength];
        activeGrid = new bool[_cubeWidth, _cubeHeight, _cubeLength];
        for (int x = 0; x < _cubeWidth; x++)
        {
            for (int y = 0; y < _cubeLength; y++)
            {
                for (int z = 0; z < _cubeLength; z++)
                {
                    _cubesCollection[x, y, z] = Instantiate(_cubePrefab, new Vector3((float)(x * 0.2 + tempTransform.transform.position.x - 0.5), (float)(y * 0.2 + tempTransform.transform.position.y - 0.5), (float)(z * 0.2 + tempTransform.transform.position.z - 0.5)), Quaternion.identity);
                    _cubesCollection[x, y, z].transform.parent = tempTransform.transform;
                }
            }
        }
        cubeIndex = 0;
    }

    [ServerRpc]
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

    //--------------------------------------------------------------------//
    //This function will update choose the next puzzle to show

    public void nextIsometric()
    {
        cubeIndex++;
        if(cubeIndex>=_cubeNames.Length)
        {
            cubeIndex = 0;
        }
        setCubes(_cubeNames[cubeIndex]);
    }
    public void prevIsometric()
    {
        cubeIndex--;
        if (cubeIndex <= -1)
        {
            cubeIndex = _cubeNames.Length-1;
        }
        setCubes(_cubeNames[cubeIndex]);
    }



    //--------------------------------------------------------------------//
    //These 3 functions will update the input field on each player's screen
    //solutionFieldChanged is called from the solutionField input field
    public void solutionFieldChanged()
    {
        
        UpdateTextServerRpc(solutionField.text);
    }
    [ServerRpc(RequireOwnership = false)]//calls the server to find the client RPC's to all clients
    public void UpdateTextServerRpc(string text)
    {
        UpdateTextClientRpc(text);
    }
    [ClientRpc]//each client will run this and update their text
    public void UpdateTextClientRpc(string text)
    {
        if(solutionField.text!=text)
        {
            solutionField.text = text;
        }
        if (solutionField.text.ToUpper() == "TWILIGHT")
        {
            puzzleData.completePuzzle.CompletePuzzleServerRpc(0);
        }

    }


    // Update is called once per frame
    void Update()
    {
        
    }

    //The following two functions are used to initialize each player's state
    [ServerRpc]//calls the server to find the client RPC's to all clients
    public void SetIsometricPuzzlesServerRpc(string word )
    {
        int i = 0;
        string[] words = word.Split(" ");
        int chunkSize =(int)Math.Ceiling((float)(words.Length)/puzzleData.connectedClients.Count);
        System.Random r = new System.Random();
        var result = words.OrderBy(x => r.Next()).GroupBy(s => i++ / chunkSize).Select(g => g.ToArray()).ToArray();
        for (int j = 0; j < result.Length; j++)
        {
            Debug.Log(result.Length);
            SetIsometricPuzzlesClientRpc(string.Join(" ", result[j]),j);
        }
        
    }
    [ClientRpc]//each client will run this and update their text
    public void SetIsometricPuzzlesClientRpc(string isoPuzzles,int clientId)
    {
        if(NetworkManager.Singleton.LocalClientId== puzzleData.connectedClients[clientId])
        {
            _cubeNames=isoPuzzles.Split(' ');
            
        }
    }
}
