using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayChat : MonoBehaviour
{
    CanvasGroup canvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GameObject.Find("ChatWindow").GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}
