using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonClick : MonoBehaviour
{
    public ClickableObjectBase networkObject;

    void OnMouseDown()
    {
        var buttonRender = gameObject.GetComponent<Renderer>();
        networkObject.OnClick(buttonRender.material.GetColor("_Color"));
    }
}
