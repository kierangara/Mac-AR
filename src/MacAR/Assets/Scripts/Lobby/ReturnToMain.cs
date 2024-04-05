//Created by Matthew Collard
//Last Updated: 2024/04/04
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ReturnToMain : MonoBehaviour
{
    //Returns the user to the main menu by signing them out of vivox, the lobby, and the authenication services.
    public async void ReturnToMainMenu(int nextScene=1)
    {
        try
        {
            string playerId = AuthenticationService.Instance.PlayerId;
            await LobbyService.Instance.RemovePlayerAsync(PlayerPrefs.GetString("lobbyID", ""), playerId);
            GameObject.Find("NetworkManager").GetComponent<VivoxPlayer>()._vvm.Logout();
            print(PlayerPrefs.GetString("lobbyID", ""));
            if (NetworkManager.Singleton.IsHost)
            {
                NetworkManager.Singleton.ConnectionApprovalCallback = null;
            }
            SceneManager.LoadScene(nextScene);
            NetworkManager.Singleton.Shutdown();
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
}
