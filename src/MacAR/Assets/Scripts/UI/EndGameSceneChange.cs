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
