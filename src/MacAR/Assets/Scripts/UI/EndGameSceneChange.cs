using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameSceneChange : MonoBehaviour
{
    // Start is called before the first frame update
    public void ChangeScene(){
        SceneManager.LoadScene(1);
        Debug.Log("Returning to main menu");
    }
}
