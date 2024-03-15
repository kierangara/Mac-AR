using System;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuDisplay : MonoBehaviour
{
    [Header("References")]
    //[SerializeField] private GameObject connectingPanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private TMP_InputField joinCodeInputField;
    [SerializeField] private TMP_InputField lobbyNameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private Slider sliderInput;
    private bool isHosting = false;


    private async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            //Debug.Log($"Player Id: {AuthenticationService.Instance.PlayerId}");
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return;
        }

        //connectingPanel.SetActive(false);
        menuPanel.SetActive(true);
    }

    public async void StartHost()
    {
        if(isHosting)
        {
            return;
        }
        isHosting = true;
        if(lobbyNameInputField.text.Length<3)
        {
            LogHandlerSettings.Instance.SpawnErrorPopup($"Error creating lobby : Lobby name not correct length, needs to be between 3 and 64 characters");
            isHosting=false;
            return;
        }

        HostManager.Instance.setConnections((int)Math.Min(Math.Max(2.0, Mathf.RoundToInt(sliderInput.value * 10)), 10));
        HostManager.Instance.setLobbyName(lobbyNameInputField.text);
        if(passwordInputField != null ) 
        {
            while(passwordInputField.text.Length<8)
            {
                passwordInputField.text = passwordInputField.text + " ";
            }

            HostManager.Instance.setPassword(passwordInputField.text);
        }

        await HostManager.Instance.StartHost();
        isHosting = false;
    }

    public async void StartClient()
    {

        Debug.Log(joinCodeInputField.text);
        await ClientManager.Instance.StartClient(joinCodeInputField.text);
    }
}
