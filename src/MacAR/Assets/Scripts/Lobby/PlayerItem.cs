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
    private PlayerData player;
    public void Initialise(PlayerData player)
    {
        this.player = player;

        playerNameText.text = player.ClientId.ToString();
        if(player.ReadyState){
            readyText.text="Ready";
            readyImage.color=Color.green;
        }
        else{
            readyText.text="Not Ready";
            readyImage.color=Color.red;
        }
    }


}
