using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviour
{
    [SerializeField] private TMP_Text readyText;
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private Image readyImage;

    private PlayerList playerList;
    private Player player;
    public void Initialise(PlayerList playerList, Player player)
    {
        this.playerList = playerList;
        this.player = player;

        playerNameText.text = player.Id;
        readyText.text = "In Lobby";
        
    }

}
