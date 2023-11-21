using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using TMPro;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
//using Unity.Tutorials.Core.Editor;
//using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class LobbyItem : MonoBehaviour
{
    [SerializeField] private TMP_Text lobbyNameText;
    [SerializeField] private TMP_Text lobbyPlayersText;
    [SerializeField] private GameObject passwordPopUp;
    [SerializeField] private TMP_InputField passwordEnter;

    private LobbiesList lobbiesList;
    private Lobby lobby;

    public void Initialise(LobbiesList lobbiesList, Lobby lobby)
    {
        this.lobbiesList = lobbiesList;
        this.lobby = lobby;

        lobbyNameText.text = lobby.Name;
        lobbyPlayersText.text = $"{lobby.Players.Count}/{lobby.MaxPlayers}";
        passwordPopUp.SetActive(false);
    }

    public void Join()
    {
        bool lobbyMade = false;
        //Debug.Log(passwordEnter.text);
        try
        {
            if (!lobby.HasPassword || (passwordEnter.text.Length!=0))
            {
                //Debug.Log(lobby.HasPassword);
                lobbiesList.JoinAsync(lobby,passwordEnter.text);
                lobbyMade = true;
                //passwordPopUp.SetActive(false);
            }
            else
            {
                passwordPopUp.SetActive(true);
            }
            
        }
        catch (LobbyServiceException exception)
        {
            //SetGameState(GameState.JoinMenu);
            LogHandlerSettings.Instance.SpawnErrorPopup($"Error joining lobby : ({exception.ErrorCode}) {exception.Message}");
            passwordPopUp.SetActive(true);
        }
        catch(Exception e)
        {
            LogHandlerSettings.Instance.SpawnErrorPopup($"Error joining lobby : Password Mismatch issue");
        }
        

        
    }
}
