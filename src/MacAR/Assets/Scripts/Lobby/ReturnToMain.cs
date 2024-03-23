using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMain : MonoBehaviour
{

    
    [SerializeField] private string mainMenuName = "Gameplay";

    // Start is called before the first frame update
    public void returnToMain()
    {
        
        try
        {
            //Ensure you sign-in before calling Authentication Instance
            //See IAuthenticationService interface
            GameObject.Find("NetworkManager").GetComponent<VivoxPlayer>()._vvm.Logout();
            //NetworkManager.Shutdown();
            //GameObject networkManager = GameObject.Find("NetworkManager");
            
            //string playerId = AuthenticationService.Instance.PlayerId;
            
            AuthenticationService.Instance.SignOut();
            //await LobbyService.Instance.RemovePlayerAsync(lobbyID, playerId);
            //NetworkManager.Singleton.Shutdown();
            //Destroy(networkManager);
            //NetworkManager.Singleton.DisconnectClient(NetworkManager.Singleton.LocalClientId);
            //NetworkManager.Singleton.SceneManager.LoadScene(mainMenuName, LoadSceneMode.Single);
            if(NetworkManager.Singleton.IsHost)
            {
                NetworkManager.Singleton.ConnectionApprovalCallback = null;
            }
            SceneManager.LoadScene(mainMenuName);
            NetworkManager.Singleton.Shutdown();

        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }



}
