using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
public class SimonCube : MonoBehaviour
{
    public PuzzleData puzzleData;
    public float startDelay = 1f;
    public float colourPeriod = 0.001f;
    private float timePassed = 0f;
    //private float interval = 2.0f;
    int counter = 0;
    Random rnd = new Random();
    Color[] colorArray = { Color.red, Color.blue, Color.green, Color.yellow };
    int level = 0;

    // Start is called before the first frame update
    void Start()
    {

        //InvokeRepeating("ColourChange", startDelay, colourPeriod);
        
    }

    void ColourChange()
    {
        var cubeRender = gameObject.GetComponent<Renderer>();
        Color randomColour = colorArray[rnd.Next(0, colorArray.Length)];
        Debug.Log("Random Colour: " + randomColour);
        //Debug.Log("Delta Time " + Time.deltaTime);
        cubeRender.material.SetColor("_Color", randomColour);


    }

    private int IncrementLevel()
    {
        level += 1;
        return level;
    }

    public List<Color> GenerateColourSequence()
    {
        List<Color> colourList = new List<Color>();
        for (int i = 0; i < level * 3; i++)
        {
            Color randomColour = colorArray[rnd.Next(0, colorArray.Length)];
            colourList.Add(randomColour);
        }
        return colourList;
        
    }

    public List<int> ConvertColorName(List<Color> colourList)
    {
        List<int> colourNameList = new List<int>();
        for (int i = 0; i < colourList.Count; i++)
        {
            if (colourList[i] == Color.red)
            {
                colourNameList.Add(0);

            } else if (colourList[i] == Color.blue)
            {
                colourNameList.Add(1);

            } else if (colourList[i] == Color.green)
            {
                colourNameList.Add(2);

            } else if (colourList[i] == Color.yellow)
            {
                colourNameList.Add(3);
            }
        }
        return colourNameList;
    }

    void BlinkBlack()
    {
        Debug.Log("Time " + timePassed);
        var cubeRender = gameObject.GetComponent<Renderer>();
        cubeRender.material.SetColor("_Color", Color.black);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
    }

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed >= 0.3f && counter == 0)
        {
            ColourChange();
            counter = 1;
        } else if (timePassed >= 1.5f){
            BlinkBlack();
            counter = 0;
            timePassed = 0f;
            
        }

        //timePassed += Time.deltaTime;
        //if (timePassed >= 1.5f)
        //{
        
            //Debug.Log("Time Passed" + timePassed);
            //timePassed = 0.0f;
            //BlinkBlack();
        //}

    }
}
