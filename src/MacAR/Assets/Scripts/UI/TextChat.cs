//Created by Kieran Gara
//Last Updated: 2024/04/04

using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using VivoxUnity;
using System.Collections.Generic;
using System.Collections;
public class TextChat : MonoBehaviour
{
    private VivoxVoiceManager _vivoxVoiceManager;
    private ChannelId _lobbyChannelId;
    private List<GameObject> _messageObjPool = new List<GameObject>();
    private ScrollRect _textChatScrollRect;

    public GameObject ChatContentObj;
    public GameObject MessageObject;
    public Button EnterButton;
    public InputField MessageInputField;
    CanvasGroup notif;
    CanvasGroup chatWindow;
    //called when the text chat is first spawned, clears out any remaining text
    private void Awake()
    {
        _textChatScrollRect = GetComponent<ScrollRect>();
        _vivoxVoiceManager = VivoxVoiceManager.Instance;
        if (_messageObjPool.Count > 0)
        {
            ClearMessageObjectPool();
        }

        ClearOutTextField();

#if !(UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_STADIA)

        MessageInputField.gameObject.SetActive(false);
        EnterButton.gameObject.SetActive(false);
        SendTTSMessageButton.gameObject.SetActive(false);
#else
        EnterButton.onClick.AddListener(SubmitTextToVivox);
        MessageInputField.onEndEdit.AddListener((string text) => { EnterKeyOnTextField(); });

#endif
        if (_vivoxVoiceManager.ActiveChannels.Count > 0)
        {
            _lobbyChannelId = _vivoxVoiceManager.ActiveChannels.FirstOrDefault(ac => ac.Channel.Name == GameObject.Find("NetworkManager").GetComponent<VivoxPlayer>().lobbyer).Key;
        }
        _vivoxVoiceManager.OnTextMessageLogReceivedEvent += OnTextMessageLogReceivedEvent;
    }
    // Start is called before the first frame update


    //Called when text chat is destroyed, removes user from voice and text channels
    private void OnDestroy()
    {
        _vivoxVoiceManager.OnParticipantAddedEvent -= OnParticipantAdded;
        _vivoxVoiceManager.OnTextMessageLogReceivedEvent -= OnTextMessageLogReceivedEvent;

#if UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_STADIA
        EnterButton.onClick.RemoveAllListeners();
        MessageInputField.onEndEdit.RemoveAllListeners();
#endif
    }



    //Clears messages from text log
    private void ClearMessageObjectPool()
    {
        for (int i = 0; i < _messageObjPool.Count; i++)
        {
            Destroy(_messageObjPool[i]);
        }
        _messageObjPool.Clear();
    }
    //clears input text field
    private void ClearOutTextField()
    {
        MessageInputField.text = string.Empty;
        MessageInputField.Select();
        MessageInputField.ActivateInputField();
    }

    //sends message to text channel
    private void EnterKeyOnTextField()
    {
        if (!Input.GetKeyDown(KeyCode.Return))
        {
            return;
        }
        SubmitTextToVivox();
    }
    //if the input field is not empty, sends the text message
    private void SubmitTextToVivox()
    {
        if (string.IsNullOrEmpty(MessageInputField.text))
        {
            return;
        }

        _vivoxVoiceManager.SendTextMessage(MessageInputField.text, _lobbyChannelId);
        ClearOutTextField();
    }

    public static string TruncateAtWord(string value, int length)
    {
        if (value == null || value.Length < length || value.IndexOf(" ", length) == -1)
            return value;

        return value.Substring(0, value.IndexOf(" ", length));
    }



    //public string[] TruncateWithPreservation(string s, int len)
    //{
    //    string[] words = s.Split(' ');
    //    string[] sections;

    //    StringBuilder sb = new StringBuilder();

    //    string currentString;

    //    foreach (string word in words)
    //    {
    //        if (sb.Length + word.Length > len)

    //            currentString = Strin;
    //            break;
    //        currentString += " ";
    //        currentString += word;
    //    }

    //    return sb.ToString();
    //}
    //not implemented
    private void SubmitTTSMessageToVivox()
    {
        if (string.IsNullOrEmpty(MessageInputField.text))
        {
            return;
        }
        var ttsMessage = new TTSMessage(MessageInputField.text, TTSDestination.QueuedRemoteTransmissionWithLocalPlayback);
        _vivoxVoiceManager.LoginSession.TTS.Speak(ttsMessage);
        ClearOutTextField();
    }
    //scrolls text to the bottom when a new message arrives
    private IEnumerator SendScrollRectToBottom()
    {
        yield return new WaitForEndOfFrame();

        // We need to wait for the end of the frame for this to be updated, otherwise it happens too quickly.
        _textChatScrollRect.normalizedPosition = new Vector2(0, 0);

        yield return null;
    }
    //When the host sends a message, a different text object is used to display
    public void DisplayHostingMessage(IChannelTextMessage channelTextMessage)
    {
        var newMessageObj = Instantiate(MessageObject, ChatContentObj.transform);
        _messageObjPool.Add(newMessageObj);
        Text newMessageText = newMessageObj.GetComponent<Text>();
    }
    //called when a new user joins the text chat
    void OnParticipantAdded(string username, ChannelId channel, IParticipant participant)
    {
        if (_vivoxVoiceManager.ActiveChannels.Count > 0)
        {
            _lobbyChannelId = _vivoxVoiceManager.ActiveChannels.FirstOrDefault().Channel;
        }
    }
    //When a text message log is recieved, colours the text and prints to text chat area
    private void OnTextMessageLogReceivedEvent(string sender, IChannelTextMessage channelTextMessage)
    {
        if (!String.IsNullOrEmpty(channelTextMessage.ApplicationStanzaNamespace))
        {
            // If we find a message with an ApplicationStanzaNamespace we don't push that to the chat box.
            // Such messages denote opening/closing or requesting the open status of multiplayer matches.
            return;
        }
        notif = GameObject.Find("Notification").GetComponent<CanvasGroup>();
        chatWindow = GameObject.Find("ChatWindow").GetComponent<CanvasGroup>();
        var newMessageObj = Instantiate(MessageObject, ChatContentObj.transform);
        _messageObjPool.Add(newMessageObj);
        Text newMessageText = newMessageObj.GetComponent<Text>();
        Debug.Log("messages should be appearing on screen");
        if (channelTextMessage.FromSelf)
        {
            newMessageText.alignment = TextAnchor.MiddleLeft;
            //newMessageText.text = string.Format($"<color=white>{channelTextMessage.Message} </color> :<color=white>{sender} </color>\n<color=white><size=15>{channelTextMessage.ReceivedTime}</size></color>");
            newMessageText.text = string.Format($"<color=white><size=40>{sender}</size></color>\n<color=white>{channelTextMessage.Message} </color>\n");
            StartCoroutine(SendScrollRectToBottom());
        }
        else
        {
            if(chatWindow.alpha == 0){
                notif.alpha = 1;
            }
            newMessageText.alignment = TextAnchor.MiddleLeft;
            //newMessageText.text = string.Format($"<color=green>{sender} </color>: {channelTextMessage.Message}\n<color=white><size=10>{channelTextMessage.ReceivedTime}</size></color>");
            newMessageText.text = string.Format($"<color=green><size=40>{sender}</size></color>\n<color=white>{channelTextMessage.Message} </color>\n");
        }
    }
}
