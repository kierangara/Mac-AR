using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using System;
public class LobbiesList : MonoBehaviour
{
    [SerializeField] private Transform lobbyItemParent;
    [SerializeField] private LobbyItem lobbyItemPrefab;

    private bool isRefreshing;
    private bool isJoining;

    private void OnEnable()
    {
        RefreshList();
    }

    public async void RefreshList()
    {
        if (isRefreshing) { return; }

        isRefreshing = true;

        try
        {
            var options = new QueryLobbiesOptions();
            options.Count = 25;

            options.Filters = new List<QueryFilter>()
            {
                new QueryFilter(
                    field: QueryFilter.FieldOptions.AvailableSlots,
                    op: QueryFilter.OpOptions.GT,
                    value: "0"),
                new QueryFilter(
                    field: QueryFilter.FieldOptions.IsLocked,
                    op: QueryFilter.OpOptions.EQ,
                    value: "0")
            };

            var lobbies = await Lobbies.Instance.QueryLobbiesAsync(options);

            foreach (Transform child in lobbyItemParent)
            {
                Destroy(child.gameObject);
            }

            foreach (Lobby lobby in lobbies.Results)
            {
                var lobbyInstance = Instantiate(lobbyItemPrefab, lobbyItemParent);
                lobbyInstance.Initialise(this, lobby);
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            isRefreshing = false;
            throw;
        }

        isRefreshing = false;
    }

    public async void JoinAsync(Lobby lobby, string password = null)
    {
        //Debug.Log(password);
        if (isJoining) { return; }

        isJoining = true;
        //password = "12345678";
        //try
        //{
        
        try
        {
            JoinLobbyByIdOptions options = new JoinLobbyByIdOptions();
            if (password.Length != 0)
            {
                options = new JoinLobbyByIdOptions
                { Password = password };
                var joiningLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobby.Id, options);
                string joinCode = joiningLobby.Data["JoinCode"].Value;
                //GameObject.Find("Lobby").GetComponent<PlayerList>().joinCodeText.text = joinCode; 
                await ClientManager.Instance.StartClient(joinCode);
            }
            else
            {
                var joiningLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobby.Id);
                string joinCode = joiningLobby.Data["JoinCode"].Value;
                //GameObject.Find("Lobby").GetComponent<PlayerList>().joinCodeText.text = joinCode;
                await ClientManager.Instance.StartClient(joinCode);
            }
            
        }
        catch
        {
            LogHandlerSettings.Instance.SpawnErrorPopup($"Error joining lobby : Password Mismatch issue");
        }
            
        //}
        //catch (LobbyServiceException e)
       /* {
            Debug.Log(e);
            isJoining = false;
            throw;
        }*/

        isJoining = false;
    }
}
