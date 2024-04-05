//Created by Matthew Collard
//Last Updated: 2024/04/04
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionToMainMenu : MonoBehaviour
{
    //Changes the scene to scene 5
    void Start()
    {
        SceneManager.LoadScene(5);
    }
}
