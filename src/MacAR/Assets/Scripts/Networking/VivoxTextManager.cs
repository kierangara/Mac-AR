using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using VivoxUnity;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System;
using Unity.XR.CoreUtils;



public class VivoxTextManager : MonoBehaviour
{
    /*public int maxMessages = 25;

    public Canvas textChat;
    private GameObject textObject;
    private GameObject chatPanel;
    //public GameObject chatPanel, textObject;
    //public InputField chatBox;

    //[SerializeField] VivoxPlayer voiceMan;
    //public VivoxVoiceManager voiceMan= Instantiate(VivoxVoiceManager.Instance);

    //[SerializeField] VivoxVoiceManager voiceMan;
    List<Message> messageList = new List<Message>();
    // Start is called before the first frame update
    private async void Start()
    {
        try
        {
            Debug.Log("Logging in");
            //await UnityServices.InitializeAsync();
            //await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log($"Player Id: {AuthenticationService.Instance.PlayerId}");
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return;
        }
        //voiceMan= Instantiate(VivoxVoiceManager.Instance);
        Debug.Log("Hi");
        
        //voiceMan.Login("Kieran");
        //voiceMan.JoinChannel("Text Channel", 0, VivoxVoiceManager.ChatCapability.TextOnly);    
    }

    

    // Update is called once per frame
/*     void Update()
    {
        if(chatBox.text != ""){
            if(Input.GetKeyDown(KeyCode.Return)){
                SendMessageToChat(chatBox.text);
                chatBox.text="";
            }
        }
        if(!chatBox.isFocused){
            if(Input.GetKeyDown(KeyCode.Space)){
            Debug.Log("hello");
            SendMessageToChat("You pressed space!");
            }
        }
        
    } */

    public void SendMessageToChat(string text){
        //voiceMan.Send_Group_Message(text);
        /* if(messageList.Count >= maxMessages){
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }

        Message newMessage = new Message();
        newMessage.text = text;

        GameObject[] uiElem = textChat.GetComponents<GameObject>();
        foreach (GameObject i in uiElem){
            Debug.Log($"i={i.name}");
            if(i.name == "Content"){
                textObject = i;
            }
            else if(i.name == "Text"){
                chatPanel = i;
            }
        }
        GameObject newText = Instantiate(textObject, chatPanel.transform);

        newMessage.textObject = newText.GetComponent<Text>();
        newMessage.textObject.text = newMessage.text;

        messageList.Add(newMessage); */
    }
    [System.Serializable]
    public class Message{
        public string text;
        public Text textObject;
    }
}
