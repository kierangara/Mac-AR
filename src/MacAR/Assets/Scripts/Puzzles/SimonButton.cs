using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonButton : MonoBehaviour
{
    public int id;
    public static SimonButton instance;

    private void OnMouseDown()
    {
        if (SimonSaysPuzzle.demoInProgress == false)
        {
            SimonSaysPuzzle.instance.TrackUserInput(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
