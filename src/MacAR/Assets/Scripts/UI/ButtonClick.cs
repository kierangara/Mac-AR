using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonClick : MonoBehaviour
{
    // public GameObject cube;

    // public Button button;
    // public GameObject puzzle;
    public TestPuzzleBehaviour networkObject;
    void OnMouseDown()
    {
        networkObject.OnClick();
        // ChangeColor();
    }
    public void ChangeColor()
    {
        // var cubeRenderer = cube.GetComponent<Renderer>();

        // // Call SetColor using the shader property name "_Color" and setting the color to red
        // if(cubeRenderer.material.GetColor("_Color") != Color.red)
        // {
        //     cubeRenderer.material.SetColor("_Color", Color.red);
        // }
        // else
        // {
        //     cubeRenderer.material.SetColor("_Color", Color.white);
        // }

        Debug.Log("Click2");

    }
}
