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

    public void StartHost()
    {
        HostManager.Instance.setConnections((int)Math.Min(Math.Max(2.0, Mathf.RoundToInt(sliderInput.value * 10)), 10));
        HostManager.Instance.setLobbyName(lobbyNameInputField.text);
        if(passwordInputField != null ) 
        {
            HostManager.Instance.setPassword(passwordInputField.text);
        }

        HostManager.Instance.StartHost();
    }

    public async void StartClient()
    {

        Debug.Log(joinCodeInputField.text);
        await ClientManager.Instance.StartClient(joinCodeInputField.text);
    }
}
