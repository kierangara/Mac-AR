//Created by Matthew Collard
//Last Updated: 2024/04/04
using UnityEngine;
//This class deals with triggered collisions from the ball hitting the goal in the maze puzzle
public class MazeBall : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if(col.name=="MazeGoal")
        {
            GameObject.Find("Maze").GetComponent<MazePuzzle>().BallHitsGoal();
        }
        else if(col.name=="MazeCheckpoint")
        {
            GameObject.Find("Maze").GetComponent<MazePuzzle>().BallHitsCheckpoint(col.gameObject);
        }
    }
}
