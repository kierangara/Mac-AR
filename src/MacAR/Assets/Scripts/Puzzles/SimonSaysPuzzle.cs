using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class SimonSaysPuzzle : MonoBehaviour
{
    [SerializeField] private GameObject cube;
    [SerializeField] private GameObject redButton;
    [SerializeField] private GameObject blueButton;
    [SerializeField] private GameObject greenButton;
    [SerializeField] private GameObject yellowButton;
    [SerializeField] private GameObject component1;
    [SerializeField] private GameObject component2;
    [SerializeField] private TMP_Text debugText;
    public static SimonSaysPuzzle instance;

    public SimonButton[] simonButtons;
    public int level = 1;


    public PuzzleData puzzleData;
    public List<int> generatedSequence = new List<int>();
    public List<int> playerSequence = new List<int>();

    public static bool demoInProgress;


    public void InitalizePuzzle()
    {
        if (instance != this && instance != null)
        {
            Destroy(this.gameObject);
        } else
        {
            instance = this;
            //Instantiate(instance, new Vector3(-6.51f, 0, 12.41f), Quaternion.identity);
            //Instantiate(component1, new Vector3(-6.51f, 0, 12.41f), Quaternion.identity);
            //Instantiate(cube, new Vector3(-3.0f, 0, 12.41f), Quaternion.identity);
            //Instantiate(component2, new Vector3(-6.51f, 0, 12.41f), Quaternion.identity);
            if (NetworkManager.Singleton.LocalClientId == puzzleData.connectedClients[0])
            {
                Debug.Log("client 0");
                //component1.transform.position = new Vector3(10, 10, 10); 
                //GameObject.Find("component 1").transform.localScale = new Vector3(0, 0, 0);
            }
            else
            {
                Debug.Log("other client");
                //component2.transform.position = new Vector3(10, 10, 10);
                //GameObject.Find("component 2").transform.localScale = new Vector3(0, 0, 0);
            }
            StartCoroutine(BeginSimonSays());
        }
        // component2.transform.Translate(0, 0, 10000);
    }

    //[ServerRpc(RequireOwnership = false)]



    private int IncrementLevel()
    {
        level += 1;
        return level;
    }



    public IEnumerator BeginSimonSays()
    {
        Debug.Log("In Simon");
        if (level == 4)
        {
            Debug.Log("Puzzle complete");
            debugText.text = "Congrats you completed the puzzle!";
            yield return new WaitForSeconds(3.0f);
            puzzleData.completePuzzle.CompletePuzzleServerRpc(0);
            yield break;
        }
        debugText.text = "Level = " + level;
        demoInProgress = true;
        generatedSequence.Clear();
        playerSequence.Clear();
        yield return new WaitForSeconds(2.0f);
        for (int i = 0; i < level * 2; i++)
        {
            Debug.Log("In for loop");
            int randColour = Random.Range(0, 3);
            generatedSequence.Add(randColour);
            if (randColour == 0)
            {
                component1.GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.red);
                cube.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            } else if (randColour == 1)
            {
                component1.GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.blue);
                cube.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
            }
            else if (randColour == 2)
            {
                component1.GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.green);
                cube.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            }
            else if (randColour == 3)
            {
                component1.GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.yellow);
                cube.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
            }

            //Make square change colours here
            yield return new WaitForSeconds(1.0f);
            cube.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
            yield return new WaitForSeconds(0.3f);
        }

        demoInProgress = false;
    }

    public void TrackUserInput(SimonButton button)
    {
        //Get button pressed by player (0 = red, 1 = blue, 2 = green, 3 = yellow)
        playerSequence.Add(button.id);
        for  (int i = 0; i < playerSequence.Count; i++)
        {
            if (playerSequence[i] != generatedSequence[i])
            {
                Debug.Log("Incorrect, level resetting");
                level = 1;
                StartCoroutine(BeginSimonSays());
            }
        }
        if (playerSequence.Count == generatedSequence.Count && generatedSequence.Count != 0)
        {
 
            IncrementLevel();
            //debugText.text = "Level = " + level;
            Debug.Log("Correct, Next Level");
            StartCoroutine(BeginSimonSays());

        }
    }


    // Update is called once per frame
    void Update()
    {
        //if (level == 4)
        //{
            //Debug.Log("Puzzle complete");
            //puzzleData.completePuzzle.CompletePuzzleServerRpc(0);

        //}
    }
}
