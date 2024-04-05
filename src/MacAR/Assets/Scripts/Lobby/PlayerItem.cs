//Created by Matthew Collard
//Last Updated: 2024/04/04
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//Single Player item that is displayed in a list when the player joins a lobby
public class PlayerItem : MonoBehaviour
{
    [SerializeField] private TMP_Text readyText;
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private Image readyImage;
    public void Initialise(PlayerData player)
    {
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
