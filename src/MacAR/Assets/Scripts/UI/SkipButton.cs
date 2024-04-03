using UnityEngine;
using UnityEngine.UI;
public class SkipButton : MonoBehaviour
{
    public Button skipButton;
    public MultiplayerPuzzleManager puzzleManager;

	void Start () 
    {
		Button btn = skipButton.GetComponent<Button>();
		btn.onClick.AddListener(SkipButtonClick);
	}

	void SkipButtonClick()
    {
		puzzleManager.SkipPuzzleServerRpc();
	}
}
