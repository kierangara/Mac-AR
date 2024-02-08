using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MazeBall : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if(col.name=="MazeGoal")
        {
            //MazePuzzle.BallHitsGoal();
            GameObject.Find("Maze").GetComponent<MazePuzzle>().BallHitsGoal();
            //GameObject.Find("NetworkManager").GetComponent<VivoxPlayer>().setJoinCode(joinCode);
        }
        else if(col.name=="MazeCheckpoint")
        {
            
            GameObject.Find("Maze").GetComponent<MazePuzzle>().BallHitsCheckpoint(col.gameObject);
        }
    }
}
