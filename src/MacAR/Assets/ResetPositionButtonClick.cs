using System.Collections;
using System.Collections.Generic;
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
