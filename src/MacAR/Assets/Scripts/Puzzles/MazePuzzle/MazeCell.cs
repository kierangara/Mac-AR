using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private GameObject _unvisitedBlock;
    public bool IsVisited { get; set; }

    public void Visit()
    {
        IsVisited = true;
        _unvisitedBlock.SetActive(false);
    }

    public void ClearLeftWall()
    {
        _leftWall.SetActive(false);
    }
    public void ClearRightWall()
    {
        _rightWall.SetActive(false);
    }
    public void ClearFrontWall()
    {
        _frontWall.SetActive(false);
    }
    public void ClearRearWall()
    {
        _rearWall.SetActive(false);
    }

    public bool GetLeftWall()
    {
        return( !_leftWall.activeSelf);
    }
    public bool GetRightWall()
    {
        return (!_rightWall.activeSelf);
    }
    public bool GetFrontWall()
    {
        return (!_frontWall.activeSelf);
    }
    public bool GetRearWall()
    {
        return (!_rearWall.activeSelf);
    }

}
