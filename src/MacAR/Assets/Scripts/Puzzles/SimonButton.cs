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
            Color buttonColour = gameObject.GetComponent<Renderer>().material.GetColor("_Color");
            Debug.Log("buttonColour" + buttonColour);
            //gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
            //Debug.Log("buttonColour" + gameObject.GetComponent<Renderer>().material.GetColor("_Color"));
            StartCoroutine(Wait());

            //Debug.Log("buttonColourFinal" + gameObject.GetComponent<Renderer>().material.GetColor("_Color"));
            //gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
            //gameObject.GetComponent<Renderer>().material.SetColor("_Color", buttonColour);
            SimonSaysPuzzle.instance.TrackUserInput(this);
        }
    }

    private IEnumerator Wait()
    {
        Color buttonColour = gameObject.GetComponent<Renderer>().material.GetColor("_Color");
        Debug.Log("Waiting");
        gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
        yield return new WaitForSeconds(0.3f);
        gameObject.GetComponent<Renderer>().material.SetColor("_Color", buttonColour);
        Debug.Log("Done Waiting");
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
