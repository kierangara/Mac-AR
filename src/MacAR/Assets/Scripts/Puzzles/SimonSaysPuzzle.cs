using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class SimonSaysPuzzle : PuzzleBase
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
    public int counter = 0;
    public NetworkVariable<int> randColour = new NetworkVariable<int>();

    //public int counter = 0;

    public PuzzleData puzzleData;
    public List<int> generatedSequence = new List<int>();
    public List<int> playerSequence = new List<int>();

    public static bool demoInProgress;


    //[ServerRpc(RequireOwnership = false)]
    public override void InitializePuzzle()
    {
        if (instance != this && instance != null)
        {
            Destroy(this.gameObject);
        } else
        {
            instance = this;
            if (puzzleData.connectedClients.Count > 1 && NetworkManager.Singleton.LocalClientId != puzzleData.connectedClients[0])
            {
                component2.transform.position = new Vector3(10, 10, 10);
            } else if (puzzleData.connectedClients.Count > 1 && NetworkManager.Singleton.LocalClientId == puzzleData.connectedClients[0])
            {
                component1.transform.position = new Vector3(10, 10, 10);
            }
 
            StartCoroutine(BeginSimonSays());
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateCubeServerRpc(Color colour)
    {
        UpdateCubeClientRpc(colour);
    }

    [ClientRpc]
    public void UpdateCubeClientRpc(Color colour)
    {
        cube.GetComponent<Renderer>().material.SetColor("_Color", colour);

    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateTextServerRpc(string text)
    {
        UpdateTextClientRpc(text);
    }

    [ClientRpc]
    public void UpdateTextClientRpc(string text)
    {
        debugText.text = text;

    }


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
            string texter = "Congrats you completed the puzzle!";
            UpdateTextServerRpc(texter);
            yield return new WaitForSeconds(3.0f);
            puzzleData.completePuzzle.CompletePuzzleServerRpc(0);
            yield break;
        }
        string text = "Level = " + level;
        UpdateTextServerRpc(text);
        demoInProgress = true;
        generatedSequence.Clear();

        playerSequence.Clear();

        UpdateCubeServerRpc(Color.black);
        yield return new WaitForSeconds(2.0f);
        //int counter = 0;
        if (NetworkManager.Singleton.LocalClientId == puzzleData.connectedClients[0])
        {

        
            for (int i = 0; i < level * 2; i++)
            {

                Debug.Log("In for loop" + i);

                randColour.Value = Random.Range(0, 4);
                //int randColour = Random.Range(0, 3);
                generatedSequence.Add(randColour.Value);
                Debug.Log("RAND COLOUR" + randColour.Value);

                if (randColour.Value == 0)
                {

                    UpdateCubeServerRpc(Color.red);
                } else if (randColour.Value == 1)
                {

                    UpdateCubeServerRpc(Color.blue);
                }
                else if (randColour.Value == 2)
                {

                    UpdateCubeServerRpc(Color.green);
                }
                else if (randColour.Value == 3)
                {

                    UpdateCubeServerRpc(Color.yellow);
                }

                //Make square change colours here
                yield return new WaitForSeconds(1.0f);
                UpdateCubeServerRpc(Color.black);
                yield return new WaitForSeconds(0.3f);
            
            }
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
