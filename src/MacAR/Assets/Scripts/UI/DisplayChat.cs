using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayChat : MonoBehaviour
{
    CanvasGroup canvasGroup;
    public Sprite textChatLogo;
    public Sprite xLogo;
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GameObject.Find("ChatWindow").GetComponent<CanvasGroup>();
    }

    public void ToggleChat(){
        if(canvasGroup.alpha == 0){
            ShowChat();
        }
        else{
            HideChat();
        }
    }
    void ShowChat(){
        if(canvasGroup.alpha == 0){
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            GameObject.Find("DisplayChat").GetComponent<Image>().overrideSprite = xLogo;
        }
    }

    void HideChat(){
        if(canvasGroup.alpha == 1){
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            GameObject.Find("DisplayChat").GetComponent<Image>().overrideSprite = textChatLogo;
        }
    }
}
