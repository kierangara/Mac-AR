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


    void Start()
    {
        if (instance != this && instance != null)
        {
            Destroy(this.gameObject);
        } else
        {
            instance = this;
            StartCoroutine(BeginSimonSays());
        }
        // component2.transform.Translate(0, 0, 10000);
    }

    [ServerRpc(RequireOwnership = false)]



    private int IncrementLevel()
    {
        level += 1;
        return level;
    }



    public IEnumerator BeginSimonSays()
    {
        if (level == 4)
        {
            Debug.Log("Puzzle complete");
            debugText.text = "Congrats you completed the puzzle!";
            yield break;
        }
        demoInProgress = true;
        generatedSequence.Clear();
        playerSequence.Clear();
        yield return new WaitForSeconds(2.0f);
        for (int i = 0; i < level * 2; i++)
        {
            
            int randColour = Random.Range(0, 3);
            generatedSequence.Add(randColour);
            if (randColour == 0)
            {
                cube.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
            } else if (randColour == 1)
            {
                cube.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
            }
            else if (randColour == 2)
            {
                cube.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            }
            else if (randColour == 3)
            {
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
