using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMain : MonoBehaviour
{

    
    [SerializeField] private string mainMenuName = "Gameplay";

    // Start is called before the first frame update
    public async void returnToMain()
    {
        NetworkManager.Singleton.SceneManager.LoadScene(mainMenuName, LoadSceneMode.Single);
        NetworkManager.Singleton.DisconnectClient(NetworkManager.Singleton.LocalClientId);
        try
        {
            //Ensure you sign-in before calling Authentication Instance
            //See IAuthenticationService interface
            string playerId = AuthenticationService.Instance.PlayerId;
            await LobbyService.Instance.RemovePlayerAsync("lobbyId", playerId);
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }



}
