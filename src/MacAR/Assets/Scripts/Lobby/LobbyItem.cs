//Created by Matthew Collard
//Last Updated: 2024/04/04
using TMPro;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using System;

public class LobbyItem : MonoBehaviour
{
    [SerializeField] private TMP_Text lobbyNameText;
    [SerializeField] private TMP_Text lobbyPlayersText;
    [SerializeField] private GameObject passwordPopUp;
    [SerializeField] private TMP_InputField passwordEnter;

    private LobbiesList lobbiesList;
    private Lobby lobby;
    private bool isJoining = false;
    //Creates the lobby item on the screen
    public void Initialise(LobbiesList lobbiesList, Lobby lobby)
    {
        this.lobbiesList = lobbiesList;
        this.lobby = lobby;

        lobbyNameText.text = lobby.Name;
        lobbyPlayersText.text = $"{lobby.Players.Count}/{lobby.MaxPlayers}";
        passwordPopUp.SetActive(false);
    }
    //Join button is pressed by the user, attempts to join a lobby, if a password is required, prompts the user to enter a password. 
    public void Join()
    {
        if(isJoining)
        {
            return;
        }
        isJoining = true;
        try
        {
            if (!lobby.HasPassword || (passwordEnter.text.Length!=0))
            {
                lobbiesList.JoinAsync(lobby,passwordEnter.text);
            }
            else
            {
                passwordPopUp.SetActive(true);
            }
            
        }
        catch (LobbyServiceException exception)
        {
            LogHandlerSettings.Instance.SpawnErrorPopup($"Error joining lobby : ({exception.ErrorCode}) {exception.Message}");
            passwordPopUp.SetActive(true);
        }
        catch(Exception)
        {
            LogHandlerSettings.Instance.SpawnErrorPopup($"Error joining lobby : Password Mismatch issue");
        }

        isJoining=false;
        

        
    }
}
