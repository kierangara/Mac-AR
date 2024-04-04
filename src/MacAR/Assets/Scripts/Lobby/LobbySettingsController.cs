//Created by Matthew Collard
//Last Updated: 2024/04/04
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbySettingsController : MonoBehaviour
{
    [SerializeField] private TMP_InputField lobbyNameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private Slider sliderInput;
    //Updates the lobby when the user hits the update button on the screen
    public async void UpdateLobby()
    {
        HostManager.Instance.SetConnections((int)Math.Min(Math.Max(2.0, Mathf.RoundToInt(sliderInput.value * 10)), 10));
        HostManager.Instance.SetLobbyName(lobbyNameInputField.text);
        if (passwordInputField != null)
        {
            HostManager.Instance.SetPassword(passwordInputField.text);
        }

        await HostManager.Instance.ChangeLobbySettings();
    }
}
