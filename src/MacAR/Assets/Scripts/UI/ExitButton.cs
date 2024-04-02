using UnityEngine;
using UnityEngine.UI;
public class ExitButton : MonoBehaviour
{
    public Button exitButton;
    public MultiplayerPuzzleManager puzzleManager;

	void Start () 
    {
		Button btn = exitButton.GetComponent<Button>();
		btn.onClick.AddListener(ExitButtonClick);
	}

	void ExitButtonClick()
    {
		puzzleManager.ButtonExitGame();
	}
}
