using System.Collections;
using System.Collections.Generic;
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
    }
}
