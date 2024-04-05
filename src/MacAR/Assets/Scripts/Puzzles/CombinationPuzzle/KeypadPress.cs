//Created by Kieran Gara
//Last Updated: 2024/04/04
using UnityEngine;
using UnityEngine.Events;

public class KeypadPress : MonoBehaviour
{
    //Calls KeypadPressed in Combination puzzle when the user clicks on a keypad
    public UnityEvent KeypadPressed;
    public void OnMouseDown(){
        Debug.Log("Clicked Me");
        KeypadPressed.Invoke();
    }
}
