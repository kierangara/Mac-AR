//Created by Ethan Kannampuzha
//Last Updated: 2024/04/04
using System.Collections;
using UnityEngine;
//Simon Button contains all code responsible for coloured buttons of Simon Says Puzzle
public class SimonButton : MonoBehaviour
{
    public int id;
    public static SimonButton instance;
    [SerializeField] private Material blackMaterial;
    //OnMouseDown called when button is pressed. Calls coroutine Wait that makes button blink black
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
    //Wait makes button turn black for 0.3s and then go back to its original colour
    private IEnumerator Wait()
    {
        Color buttonColour = gameObject.GetComponent<Renderer>().material.GetColor("_Color");
        Material mat = gameObject.GetComponent<Renderer>().material;
        Debug.Log("Waiting");
        gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
        gameObject.GetComponent<Renderer>().material = blackMaterial;
        Debug.Log("Set colour black" + gameObject.GetComponent<Renderer>().material.GetColor("_Color"));
        yield return new WaitForSeconds(0.3f);
        gameObject.GetComponent<Renderer>().material.SetColor("_Color", buttonColour);
        gameObject.GetComponent<Renderer>().material = mat;
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
