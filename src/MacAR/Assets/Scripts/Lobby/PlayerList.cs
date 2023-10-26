using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class PlayerList : NetworkBehaviour
{
    [SerializeField] Button readyButton;
    [SerializeField] private Transform playerItemParent;
    [SerializeField] private PlayerItem playerItemPrefab;

    private bool readyState = false;
    private Color notReadyColor = Color.red;
    private Color readyColor = Color.green;
    private bool isRefreshing;
    private Lobby lobby;

    private void OnEnable()
    {
        RefreshPlayerList();
    }

    public void readyPress()
    {
        readyState = readyState ^ true;
        changeReadyColor();
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
            foreach (Player player in lobby.Players)
            {
                var playerInstance = Instantiate(playerItemPrefab, playerItemParent);
                playerInstance.Initialise(this, player);
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
