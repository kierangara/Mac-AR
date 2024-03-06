using System;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbySettingsController : MonoBehaviour
{
    [SerializeField] private TMP_InputField lobbyNameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private Slider sliderInput;
    public async void UpdateLobby()
    {
        HostManager.Instance.setConnections((int)Math.Min(Math.Max(2.0, Mathf.RoundToInt(sliderInput.value * 10)), 10));
        HostManager.Instance.setLobbyName(lobbyNameInputField.text);
        if (passwordInputField != null)
        {
            HostManager.Instance.setPassword(passwordInputField.text);
        }

        await HostManager.Instance.ChangeLobbySettings();
    }
}
