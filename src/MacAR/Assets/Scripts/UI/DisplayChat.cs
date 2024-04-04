//Created by Kieran Gara
//Last Updated: 2024/04/04

using UnityEngine;
using UnityEngine.UI;
public class DisplayChat : MonoBehaviour
{
    CanvasGroup canvasGroup;
    CanvasGroup notif;
    public Sprite textChatLogo;
    public Sprite xLogo;
    public Button dispButton;
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GameObject.Find("ChatWindow").GetComponent<CanvasGroup>();
        notif = GameObject.Find("Notification").GetComponent<CanvasGroup>();
    }
    //Toggles the visibility of the text chat
    public void ToggleChat(){
        if(canvasGroup.alpha == 0){
            ShowChat();
        }
        else{
            HideChat();
        }
    }
    //Changes the alphas of the chat to show the text chat panels
    void ShowChat(){
        if(canvasGroup.alpha == 0){
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            GameObject.Find("DisplayChat").GetComponent<Image>().overrideSprite = xLogo;
            dispButton.interactable = false;
            notif.alpha = 0;
        }
    }
    //Changes the alphas of the chat to hide the text chat panels
    void HideChat(){
        if(canvasGroup.alpha == 1){
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            GameObject.Find("DisplayChat").GetComponent<Image>().overrideSprite = textChatLogo;
            dispButton.interactable = true;
        }
    }
}
