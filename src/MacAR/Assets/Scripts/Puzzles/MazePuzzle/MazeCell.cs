//Created by Matthew Collard
//Last Updated: 2024/04/04
using UnityEngine;
//The maze cell is a section of the maze with 4 walls and 4 columns. 
public class MazeCell : MonoBehaviour
{
    [SerializeField]
    private GameObject _leftWall;
    [SerializeField]
    private GameObject _rightWall;
    [SerializeField]
    private GameObject _frontWall;
    [SerializeField]
    private GameObject _rearWall;
    [SerializeField]
    private GameObject _columnFR;
    [SerializeField]
    private GameObject _columnFL;
    [SerializeField]
    private GameObject _columnBR;
    [SerializeField]
    private GameObject _columnBL;
    [SerializeField]
    private GameObject _unvisitedBlock;
    public bool IsVisited { get; set; }
    //During the maze generation algoithm, visit gets called to tell the alogithm this square does not need to be visited again
    public void Visit()
    {
        IsVisited = true;
        _unvisitedBlock.SetActive(false);
    }
    //removes the left wall from view
    public void ClearLeftWall()
    {
        _leftWall.SetActive(false);
    }
    //removes the right wall from view
    public void ClearRightWall()
    {
        _rightWall.SetActive(false);
    }
    //removes the front wall from view
    public void ClearFrontWall()
    {
        _frontWall.SetActive(false);
    }
    //removes the rear wall from view
    public void ClearRearWall()
    {
        _rearWall.SetActive(false);
    }
    //Gets whether the Left wall is visible
    public bool GetLeftWall()
    {
        return( !_leftWall.activeSelf);
    }
    //Gets whether the Right wall is visible
    public bool GetRightWall()
    {
        return (!_rightWall.activeSelf);
    }
    //Gets whether the Front wall is visible
    public bool GetFrontWall()
    {
        return (!_frontWall.activeSelf);
    }
    //Gets whether the Rear wall is visible
    public bool GetRearWall()
    {
        return (!_rearWall.activeSelf);
    }

    //Makes the Back Right Column Visible
    public void ShowColumnBR()
    {
        _columnBR.SetActive(true);
    }
    //Makes the Back Left Column Visible
    public void ShowColumnBL()
    {
        _columnBL.SetActive(true);
    }
    //Makes the Front Left Column Visible
    public void ShowColumnFL()
    {
        _columnFL.SetActive(true);
    }

}
