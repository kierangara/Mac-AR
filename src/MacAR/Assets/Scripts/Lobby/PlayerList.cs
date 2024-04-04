//Created by Matthew Collard
//Last Updated: 2024/04/04
using Unity.Netcode;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerList : NetworkBehaviour
{
    [SerializeField] Button readyButton;
    [SerializeField] private Toggle VoiceToggle;
    [SerializeField] public Toggle inGameVoiceToggle;
    [SerializeField] private Transform playerItemParent;
    [SerializeField] private PlayerItem playerItemPrefab;
    [SerializeField] public TMP_Text joinCodeText;
    [SerializeField] public TMP_InputField playerNameField;



    public static readonly VivoxUnity.Client mainClient = new VivoxUnity.Client();
    private NetworkList<PlayerData> players;

    private bool readyState = false;
    private readonly Color NOT_READY_COLOR = Color.red;
    private readonly Color READY_COLOR = Color.green;
    private bool isRefreshing;
    private Lobby lobby;
    public static bool muted;
    //Called when initialized
    private void Awake()
    {
        print("playerlist creation");
        players = new NetworkList<PlayerData>();
        VoiceToggle.onValueChanged.AddListener(delegate 
            { VivoxToggle(VoiceToggle,mainClient); });
        mainClient.AudioInputDevices.Muted = true;
        muted = true;
    }
    //Toggles voice chat
    void VivoxToggle(Toggle voiceToggle, VivoxUnity.Client client)
    {
        Debug.Log("Voice " + voiceToggle.isOn);
        if (voiceToggle.isOn)
        {
            client.AudioInputDevices.Muted = true;
            muted = true;
        } else
        {
            client.AudioInputDevices.Muted = false;
            muted = false;
        }
    }
    //Added as a function that is called when the network first gets created, sets up the client callbacks
     public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            players.OnListChanged += HandlePlayersStateChanged;
            //joinCodeText.text = HostManager.Instance.JoinCode;
        }

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;

            foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
            {
                HandleClientConnected(client.ClientId);
            }
            //joinCodeText.text = HostManager.Instance.JoinCode;
        }

        if(IsHost)
        {
            joinCodeText.text = HostManager.Instance.JoinCode;

        }


        //Added Code

        var vTog = GameObject.Find("Toggle").GetComponent<Toggle>();
        Debug.Log("Getting before if statement " + vTog.isOn);
        if (vTog.isOn)
        {
            Debug.Log("Getting after if statement " + vTog.isOn);
            GameObject.Find("NetworkManager").GetComponent<VivoxPlayer>().SignIntoVivox();
        }
    }
    //Called when the network is destroyed
    public override void OnNetworkDespawn()
    {
        if (IsClient)
        {
            players.OnListChanged -= HandlePlayersStateChanged;
        }

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnected;
        }
    }

    private void HandleClientConnected(ulong clientId)
    {
        players.Add(new PlayerData(clientId));
    }

    private void HandleClientDisconnected(ulong clientId)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].ClientId != clientId) { continue; }

            players.RemoveAt(i);
            break;
        }
    }


    private void OnEnable()
    {
        RefreshPlayerList();
    }
    //Called when the ready button is pressed
    public void readyPress()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].ClientId != NetworkManager.Singleton.LocalClientId) { continue; }
        }

        readyState = readyState ^ true;
        ReadyServerRpc(readyState);
        ChangeReadyColor();
    }
    //Server RPC that changes the ready button color for all the users in the lobby
    [ServerRpc(RequireOwnership = false)]
    private void ReadyServerRpc(bool readyState, ServerRpcParams serverRpcParams = default)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].ClientId != serverRpcParams.Receive.SenderClientId) { continue; }

            players[i] = new PlayerData(
                players[i].ClientId,
                readyState
            );
        }


        foreach (var player in players)
        {
            if (!player.ReadyState) { return; }
        }
        HostManager.Instance.StartGame();
    }
private void HandlePlayersStateChanged(NetworkListEvent<PlayerData> changeEvent)
    {
        RefreshPlayerList();
    }
    public void setLobby(Lobby lobby)
    {
        this.lobby = lobby;
    }
    //Changes the colour of the ready button
    private void ChangeReadyColor()
    {
        Color changeColor = NOT_READY_COLOR;
        if (readyState)
        {
            changeColor = READY_COLOR;
        }
        ColorBlock cb = readyButton.colors;
        cb.normalColor = changeColor;
        cb.highlightedColor = changeColor;
        cb.pressedColor = changeColor;
        cb.selectedColor = changeColor;
        cb.selectedColor = changeColor;
        readyButton.colors = cb;
    }


    //Refreshes the player list that is visible to the user
    public void RefreshPlayerList()
    {
        if (isRefreshing) { return; }

        isRefreshing = true;
        try
        {
            foreach (Transform child in playerItemParent)
            {
                Destroy(child.gameObject);
            }
            foreach (PlayerData player in players)
            {
                var playerInstance = Instantiate(playerItemPrefab, playerItemParent);
                playerInstance.Initialise(player);
            }
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
            isRefreshing = false;
            throw;
        }
        isRefreshing = false;
    }
}
