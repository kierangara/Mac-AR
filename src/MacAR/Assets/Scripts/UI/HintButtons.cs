using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HintButtons : MonoBehaviour
{
    public Button forwardButton;
    public Button backButton;
    public Button dispButton;
    public TextMeshProUGUI hintTexter;
    public TextMeshProUGUI hintCounter;
    List<string> hintList = new List<string>();
    int counter = 0;
    int hintCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("got hint text");
        hintList.Add("This is the first hint");
        hintList.Add("This is the second hint");
        hintList.Add("This is the third hint");
        hintTexter.text = "hello";
        Button forBtn = forwardButton.GetComponent<Button>();
        Button backBtn = backButton.GetComponent<Button>();
        Button dispBtn = dispButton.GetComponent<Button>();
        forBtn.onClick.AddListener(forwardClick);
        backBtn.onClick.AddListener(backwardClick);
        dispBtn.onClick.AddListener(initialClick);
    }

    void initialClick()
    {
        counter = 0;
        hintTexter.text = hintList[counter];
        hintCount = counter + 1;
        hintCounter.text = " Hint: " + hintCount + "/" + hintList.Count;
        backButton.gameObject.SetActive(false);
        forwardButton.gameObject.SetActive(true);
        Debug.Log("initial" + counter);
    }

    void forwardClick()
    {
        counter += 1;
        hintCount = counter + 1;

        if (counter > 0)
        {
            backButton.gameObject.SetActive(true);
     
        } else if (counter == 0)
        {
            backButton.gameObject.SetActive(false);
        }

        if (counter == 2)
        {
            forwardButton.gameObject.SetActive(false);
        }

        Debug.Log("counter in front" + counter);
        hintTexter.text = hintList[counter];
        hintCounter.text = " Hint: " + hintCount + "/" + hintList.Count;




    }

    void backwardClick()
    {
        if (counter > 0)
        {
            counter -= 1;
            hintCount = counter + 1;
        }

        if (counter == 0)
        {
            backButton.gameObject.SetActive(false);
        }

        if (counter < 2)
        {
            forwardButton.gameObject.SetActive(true);
        }
        Debug.Log("counter in back" + counter);
        hintTexter.text = hintList[counter];
        hintCounter.text = " Hint: " + hintCount + "/" + hintList.Count;

    }



}
