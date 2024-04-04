//Created by Matthew Collard
//Last Updated: 2024/04/04
//Resets the rotation of the maze when the button is clicked.
using UnityEngine;
public class ResetPositionButtonClick : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject mazePuzzle;
    void OnMouseDown()
    {
        mazePuzzle.GetComponent<MazePuzzle>().resetRotationPress();
    }
}
