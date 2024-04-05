//Created by Matthew Collard
//Last Updated: 2024/04/04
using System.Collections.Generic;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
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
    //This refreashes the list of available lobbies
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
    //This fucntion allows the user to join a lobby that may or may not require a password
    public async void JoinAsync(Lobby lobby, string password = null)
    {
        if (isJoining) { return; }

        isJoining = true;
        
        try
        {
            JoinLobbyByIdOptions options = new JoinLobbyByIdOptions();
            if (password.Length != 0)//Tries to join a lobby that has a password
            {
                while(password.Length < 8)
                {
                    password = password + " ";
                }
                options = new JoinLobbyByIdOptions
                { Password = password };
                var joiningLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobby.Id, options);
                string joinCode = joiningLobby.Data["JoinCode"].Value;
                await ClientManager.Instance.StartClient(joinCode);
                GameObject.Find("NetworkManager").GetComponent<VivoxPlayer>().setJoinCode(joinCode);
                PlayerPrefs.SetString("lobbyID", lobby.Id);
            }
            else//Tries to join a lobby that does not have a password
            {
                var joiningLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobby.Id);
                string joinCode = joiningLobby.Data["JoinCode"].Value;
                await ClientManager.Instance.StartClient(joinCode);
                GameObject.Find("NetworkManager").GetComponent<VivoxPlayer>().setJoinCode(joinCode);
                PlayerPrefs.SetString("lobbyID", lobby.Id);
            }
        }
        catch
        {//Spawns a popup on the screen if joining the lobby fails.
            LogHandlerSettings.Instance.SpawnErrorPopup($"Error joining lobby : Password Mismatch issue");
        }

        isJoining = false;
    }
}
