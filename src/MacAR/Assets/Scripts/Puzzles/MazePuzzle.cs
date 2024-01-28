using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class MazePuzzle : MonoBehaviour
{
    //Vector3 BallPosition=new Vector3(0,0,0);
    //Vector3 MazeRotation = new Vector3(0, 0, 0);

    [SerializeField] private GameObject maze;
    [SerializeField] private GameObject ball;
    [SerializeField] private TMP_Text debugText;
    private float trackingYRot;
    private float trackingXRot;
    private float trackingZRot;
    // Start is called before the first frame update
    private void Start()
    {
        maze.transform.rotation = Quaternion.identity;
        Input.gyro.enabled = true;
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

    private void BallHitsHole()
    {

    }

    private void BallHitsGoal()
    {

    }
    // Update is called once per frame
    void Update()
    {
        //Quaternion phoneRotation = Input.gyro.attitude;
        if(SystemInfo.supportsGyroscope) 
        {
            Input.gyro.enabled = true;
            trackingXRot+= -Input.gyro.rotationRateUnbiased.x;
            trackingYRot += -Input.gyro.rotationRateUnbiased.y;
            trackingZRot += -Input.gyro.rotationRateUnbiased.z;
            Vector3 phoneRotation = new Vector3(-trackingXRot*3, trackingZRot, -trackingYRot*3);
            RotateMaze(phoneRotation);
            //Debug.Log(phoneRotation.ToString());
            debugText.text = phoneRotation.ToString();
        }
        else
        {
            debugText.text = "This phone does not support Gyroscope";
        }
        //RotateMaze (GyroToUnity(phoneRotation));
        //Debug.Log(phoneRotation.ToString());
        
        
    }

    private static Quaternion GyroToUnity(Quaternion q)
    {
        return Quaternion.Euler(90, 0, 0) * new Quaternion(q.x, q.y, -q.z, -q.w);
    }
}
