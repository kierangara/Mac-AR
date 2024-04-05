//Created by Kieran Gara
//Last Updated: 2024/04/04

using UnityEngine;
using UnityEngine.SceneManagement;
//Changes the scene when called
public class EndGameSceneChange : MonoBehaviour
{
    // Start is called before the first frame update
    public void ChangeScene(){
        SceneManager.LoadScene(1);
        Debug.Log("Returning to main menu");
    }
}
