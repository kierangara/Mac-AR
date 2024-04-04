//Created by Matthew Collard
//Last Updated: 2024/04/04
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HostManager : MonoBehaviour
{
    [Header("Settings")]
    private int maxConnections = 4;
    private string lobbyPassword = null;
    private string lobbyName;
    [SerializeField] private string characterSelectSceneName = "CharacterSelect";
    [SerializeField] private string gameplaySceneName = "Gameplay";
    //[SerializeField] private string mainMenuName = "MainMenu";

    public static HostManager Instance { get; private set; }

    private bool gameHasStarted;
    private string lobbyId;

    public Dictionary<ulong, ClientData> ClientData { get; private set; }
    public string JoinCode { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetConnections(int maxSize)
    {
        this.maxConnections = maxSize;
    }

    public void SetPassword(string password)
    {
        this.lobbyPassword = password;
    }

    public void SetLobbyName(string name)
    {
        this.lobbyName = name;
    }
    //Hosts a lobby, making the user the host
    public async Task StartHost()
    {

        Allocation allocation;

        try
        {//sets the lobby size, creates a lobby
            allocation = await RelayService.Instance.CreateAllocationAsync(maxConnections);
        }
        catch (Exception e)
        {
            Debug.LogError($"Relay create allocation request failed {e.Message}");
            throw;
        }

        Debug.Log($"server: {allocation.ConnectionData[0]} {allocation.ConnectionData[1]}");
        Debug.Log($"server: {allocation.AllocationId}");

        try
        {//gets the join code for the lobby
            JoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
        }
        catch
        {
            Debug.LogError("Relay get join code request failed");
            throw;
        }

        var relayServerData = new RelayServerData(allocation, "dtls");

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

        try
        {
            var createLobbyOptions = new CreateLobbyOptions();
            createLobbyOptions.IsPrivate = false;
            if (lobbyPassword.Length!=0)
            {
                createLobbyOptions.Password = lobbyPassword;
            }//Sets the lobby options
            createLobbyOptions.Data = new Dictionary<string, DataObject>()
            {
                {
                    "JoinCode", new DataObject(
                        visibility: DataObject.VisibilityOptions.Member,
                        value: JoinCode
                    )
                }
            };
            //Creates the lobby, starts a heartbeat to keep the lobby stable
            Lobby lobby = await Lobbies.Instance.CreateLobbyAsync(lobbyName, maxConnections, createLobbyOptions);
            lobbyId = lobby.Id;
            StartCoroutine(HeartbeatLobbyCoroutine(15));
            GameObject.Find("NetworkManager").GetComponent<VivoxPlayer>().setLobby(lobby);
            PlayerPrefs.SetString("lobbyID", lobby.Id);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            throw;
        }
        NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
        NetworkManager.Singleton.OnServerStarted += OnNetworkReady;
        ClientData = new Dictionary<ulong, ClientData>();
        NetworkManager.Singleton.StartHost();
        return;
    }
    //Changes the lobby settings
    public async Task ChangeLobbySettings()
    {
     
        var updateLobbyOptions = new UpdateLobbyOptions();
        updateLobbyOptions.IsPrivate = false;
        if (lobbyPassword.Length != 0)
        {
            updateLobbyOptions.Password = lobbyPassword;
        }
        updateLobbyOptions.Data = new Dictionary<string, DataObject>()
        {
            {
                "JoinCode", new DataObject(
                visibility: DataObject.VisibilityOptions.Member,
                value: JoinCode
                )
            }
        };
        var updatedLobby = await Lobbies.Instance.UpdateLobbyAsync(lobbyId, updateLobbyOptions);
    }
    //Pings the server every waitTimeSeconds to keep the lobby running
    private IEnumerator HeartbeatLobbyCoroutine(float waitTimeSeconds)
    {
        var delay = new WaitForSeconds(waitTimeSeconds);
        while (true)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(lobbyId);
            yield return delay;
        }
    }
    //Starts the game for all the users
    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
    {
        if (ClientData.Count >= 4 || gameHasStarted)
        {
            response.Approved = false;
            return;
        }

        response.Approved = true;
        response.CreatePlayerObject = false;
        response.Pending = false;

        ClientData[request.ClientNetworkId] = new ClientData(request.ClientNetworkId);

        Debug.Log($"Added client {request.ClientNetworkId}");
    }

    private void OnNetworkReady()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnect;

        NetworkManager.Singleton.SceneManager.LoadScene(characterSelectSceneName, LoadSceneMode.Single);
    }

    private void OnClientDisconnect(ulong clientId)
    {
        if (ClientData.ContainsKey(clientId))
        {
            if (ClientData.Remove(clientId))
            {
                Debug.Log($"Removed client {clientId}");
            }
        }
    }

    public void StartGame()
    {
        gameHasStarted = true;

        NetworkManager.Singleton.SceneManager.LoadScene(gameplaySceneName, LoadSceneMode.Single);
    }


}
