using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionToMainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene(5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
