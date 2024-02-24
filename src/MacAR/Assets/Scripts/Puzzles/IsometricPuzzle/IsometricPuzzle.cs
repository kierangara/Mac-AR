using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

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
        setCubes("2W");
    }

    public void setCubes(string key)
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


    //--------------------------------------------------------------------//
    //These 3 functions will update the input field on each player's screen
    //solutionFieldChanged is called from the solutionField script
    public void solutionFieldChanged()
    {
        UpdateTextServerRpc(solutionField.text.Substring(0,8));
    }
    [ServerRpc(RequireOwnership = false)]//calls the server to find the client RPC's to all clients
    public void UpdateTextServerRpc(string text)
    {
        UpdateTextClientRpc(text);
    }
    [ClientRpc]//each client will run this and update their text
    public void UpdateTextClientRpc(string text)
    {
        solutionField.text = text;

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
