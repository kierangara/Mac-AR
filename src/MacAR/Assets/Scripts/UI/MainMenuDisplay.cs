using System;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class MainMenuDisplay : MonoBehaviour
{
    [Header("References")]
    //[SerializeField] private GameObject connectingPanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private TMP_InputField joinCodeInputField;
    [SerializeField] private TMP_InputField lobbyNameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private Slider sliderInput;
    [SerializeField] private GameObject reconnectPopUp;
    private bool isHosting = false;
    public int startCount = 0;
    public DataCollection data;
    private const string dataFileName = "PlayerData";

    private async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
            
            if(!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
            
            //Debug.Log($"Player Id: {AuthenticationService.Instance.PlayerId}");
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return;
        }

        //connectingPanel.SetActive(false);
        menuPanel.SetActive(true);
        if(startCount == 0) 
        {
            Load();
            startCount++;
        }
        else
        {
            data = SaveSystem.SaveExists(dataFileName) ? SaveSystem.LoadData<DataCollection>(dataFileName) : new DataCollection();
        }
        if(data.lobbyId != "")
        {
            //reconnectPopUp.SetActive(true);
        }

        

    }
    public async void StartReconnect()
    {
        await Reconnect();
    }


    public async Task Reconnect()
    {
        try
        {
            await LobbyService.Instance.ReconnectToLobbyAsync(PlayerPrefs.GetString("lobbyID", ""));

        }
        catch (LobbyServiceException e)
        {
            LogHandlerSettings.Instance.SpawnErrorPopup($"Error Joining Lobby : \n Lobby Does not Exist");
            Debug.Log(e);
            RemoveLobby();
        }

    }

    public void RemoveLobby()
    {
        PlayerPrefs.SetString("lobbyID", "");
    }


    public void Load()
    {
        data.lobbyId = PlayerPrefs.GetString("lobbyID","");
    }

    public void Save()
    {
        PlayerPrefs.SetString("lobbyID", data.lobbyId);
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
            while((passwordInputField.text.Length!=0)&&(passwordInputField.text.Length<8))
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
