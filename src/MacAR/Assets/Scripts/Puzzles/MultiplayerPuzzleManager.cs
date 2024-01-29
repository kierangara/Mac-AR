using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerPuzzleManager : MonoBehaviour
{
    // private List<GameObject> puzzles = new List<GameObject>();
    public GameObject puzzle;
    public Camera cam; 

    //Start is called before the first frame update
    void Start()
    {
        var puzzleInstance = Instantiate(puzzle, new Vector3(0, 0, 0), Quaternion.identity);
        // Make Abstract
        puzzleInstance.GetComponentInChildren<ObjectRotate>().cam = cam;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
