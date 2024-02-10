using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeypadPress : MonoBehaviour
{
    // Start is called before the first frame update

    public UnityEvent KeypadPressed;
    public void OnMouseDown(){
        Debug.Log("Clicked Me");
        KeypadPressed.Invoke();
    }
}
