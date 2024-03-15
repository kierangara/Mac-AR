using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HintButtons : MonoBehaviour
{
    public Text hintText;
    List<string> hintList = new List<string>();
    int counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("got hint text");
        Debug.Log(hintText.text);
        hintList.Add("This is the first hint");
        hintList.Add("This is the second hint");
        hintList.Add("This is the third hint");
    }

    public void changeText()
    {
        hintText.text = hintList[counter];
        Debug.Log("worked");
        Debug.Log(hintText.text);
        if (counter < 2)
        {
            counter += 1;
        }
    }

}
