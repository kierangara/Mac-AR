using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
//using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameSceneChange : MonoBehaviour
{
    [SerializeField] private string mainMenuScene = "MainMenu";
    // Start is called before the first frame update
    public void ChangeScene(){
        SceneManager.LoadScene(1);
        Debug.Log("Returning to main menu");
    }
}
