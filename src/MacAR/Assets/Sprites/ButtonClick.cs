using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ButtonClick : MonoBehaviour
{
    public GameObject cube;
	void OnMouseDown()
	{
		ChangeColor();
	}
   public void ChangeColor()
   {
        var cubeRenderer = cube.GetComponent<Renderer>();

       // Call SetColor using the shader property name "_Color" and setting the color to red
       cubeRenderer.material.SetColor("_Color", Color.red);
   }
}
