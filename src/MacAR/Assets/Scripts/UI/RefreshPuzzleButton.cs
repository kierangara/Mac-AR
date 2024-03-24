using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class RefreshPuzzleButton : MonoBehaviour
{
    public Button refreshButton;
    public MultiplayerPuzzleManager puzzleManager;

	void Start() 
    {
		Button btn = refreshButton.GetComponent<Button>();
		btn.onClick.AddListener(RefreshButtonClick);
	}

	void RefreshButtonClick()
    {
		puzzleManager.RefreshPuzzlePositionsServerRpc(NetworkManager.Singleton.LocalClientId);
	}
}
