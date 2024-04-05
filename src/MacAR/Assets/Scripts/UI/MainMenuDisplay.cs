//Created by Matthew Collard
//Last Updated: 2024/04/04
using System;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
//Main Menu Display is in charge of controlling the main menu screen
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
    [SerializeField] private TMP_InputField userNameInputField;
    private bool isHosting = false;
    public int startCount = 0;
    public DataCollection data;
    private const string dataFileName = "PlayerData";
    //Signs into the authentication service when started, loads saved data from memory
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

        userNameInputField.text=PlayerPrefs.GetString("PlayerName","SpencerSmith");



    }
    public async void StartReconnect()
    {
        await Reconnect();
    }
    //Input field activates this function, changes the account name of the user
    public void SetAccountName()
    {
        PlayerPrefs.SetString("PlayerName", userNameInputField.text);
    }
    //Unimplemented, attempts to connect the user
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
    //Clears the lobby string when called
    public void RemoveLobby()
    {
        PlayerPrefs.SetString("lobbyID", "");
    }

    //Loads the lobby id from memory
    public void Load()
    {
        data.lobbyId = PlayerPrefs.GetString("lobbyID","");
    }
    //Saves user data
    public void Save()
    {
        PlayerPrefs.SetString("lobbyID", data.lobbyId);
    }

    //Starts the host when the button is pressed, error handles for incorrect lobby name size and password size
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

        HostManager.Instance.SetConnections((int)Math.Min(Math.Max(2.0, Mathf.RoundToInt(sliderInput.value * 10)), 10));
        HostManager.Instance.SetLobbyName(lobbyNameInputField.text);
        if(passwordInputField != null ) 
        {
            while((passwordInputField.text.Length!=0)&&(passwordInputField.text.Length<8))
            {
                passwordInputField.text = passwordInputField.text + " ";
            }

            HostManager.Instance.SetPassword(passwordInputField.text);
        }

        await HostManager.Instance.StartHost();
        isHosting = false;
    }
    //Button click that starts the client to join a lobby
    public async void StartClient()
    {

        Debug.Log(joinCodeInputField.text);
        await ClientManager.Instance.StartClient(joinCodeInputField.text);
    }
}
