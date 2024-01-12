using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    [SerializeField] private Transform playerItemParent;
    [SerializeField] private PlayerItem playerItemPrefab;
    [SerializeField] private TMP_Text joinCodeText;
    public static readonly VivoxUnity.Client mainClient = new VivoxUnity.Client();
    private NetworkList<PlayerData> players;

    private bool readyState = false;
    private Color notReadyColor = Color.red;
    private Color readyColor = Color.green;
    private bool isRefreshing;
    private Lobby lobby;

    private void Awake()
    {
        players = new NetworkList<PlayerData>();
        VoiceToggle.onValueChanged.AddListener(delegate 
            { VivoxToggle(VoiceToggle,mainClient); });
    }

    void VivoxToggle(Toggle voiceToggle, VivoxUnity.Client client)
    {
        Debug.Log("Voice " + voiceToggle.isOn);
        if (voiceToggle.isOn)
        {
            client.AudioInputDevices.Muted = false; 
        } else
        {
            client.AudioInputDevices.Muted = true;
        }
    }

     public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            players.OnListChanged += HandlePlayersStateChanged;
        }

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;

            foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
            {
                HandleClientConnected(client.ClientId);
            }
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

    public void readyPress()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].ClientId != NetworkManager.Singleton.LocalClientId) { continue; }
        }

        readyState = readyState ^ true;
        ReadyServerRpc(readyState);
        changeReadyColor();
    }

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
        /*foreach (var player in players)
        {
            if (player.ClientId != NetworkManager.Singleton.LocalClientId) { continue; }

            /*if (player.ReadyState)
            {
                lockInButton.interactable = false;
                break;
            }

            if (IsCharacterTaken(player.CharacterId, false))
            {
                lockInButton.interactable = false;
                break;
            }

            lockInButton.interactable = true;
            
            break;
        }*/
    }





    public void setLobby(Lobby lobby)
    {
        this.lobby = lobby;
    }

    private void changeReadyColor()
    {
        Color changeColor = notReadyColor;
        if (readyState)
        {
            changeColor = readyColor;
        }
        ColorBlock cb = readyButton.colors;
        cb.normalColor = changeColor;
        cb.highlightedColor = changeColor;
        cb.pressedColor = changeColor;
        cb.selectedColor = changeColor;
        cb.selectedColor = changeColor;
        readyButton.colors = cb;
    }



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
