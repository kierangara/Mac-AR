using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeBall : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if(col.name=="MazeGoal")
        {
            MazePuzzle.BallHitsGoal();
        }
    }
}
