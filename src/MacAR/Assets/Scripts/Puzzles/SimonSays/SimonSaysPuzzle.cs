//Created by Ethan Kannampuzha
//Last Updated: 2024/04/04
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;
//The Simon Says Puzzle contains all code responsible for simon says puzzle
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

    [SerializeField] private Material redMaterial;
    [SerializeField] private Material blueMaterial;
    [SerializeField] private Material yellowMaterial;
    [SerializeField] private Material greenMaterial;
    [SerializeField] private Material blackMaterial;


    public static SimonSaysPuzzle instance;

    public SimonButton[] simonButtons;
    public int level = 1;
    public int counter = 0;
    public NetworkVariable<int> randColour = new NetworkVariable<int>();
    int red = 0;
    int blue = 1;
    int green = 2;
    int yellow = 3;


    public PuzzleData puzzleData;
    public List<int> generatedSequence = new List<int>();
    public List<int> playerSequence = new List<int>();

    public static bool demoInProgress;

    //Initializes the puzzle, coloured buttons spawn for host, cube spawns for non-hosts
    public override void InitializePuzzle()
    {
        puzzleId = PuzzleConstants.SIMON_ID;

        if (instance != this && instance != null)
        {
            Destroy(this.gameObject);
        } else
        {
            instance = this;
            if (puzzleData.connectedClients.Count > 1 && NetworkManager.Singleton.LocalClientId != puzzleData.connectedClients[0])
            {
                component2.SetActive(false);
                //component2.transform.position = new Vector3(10, 10, 10);
            } else if (puzzleData.connectedClients.Count > 1 && NetworkManager.Singleton.LocalClientId == puzzleData.connectedClients[0])
            {
                component1.SetActive(false);
                //component1.transform.position = new Vector3(10, 10, 10);
            }
 
            //StartCoroutine(BeginSimonSays());
        }
    }
    //Sets puzzle to active
    public override void SetActive(bool status)
    {
        active = status;

        if (status == true)
        {
            StartCoroutine(BeginSimonSays());
        }
    }
    //Requests for cube colour to be updated for non-hosts
    [ServerRpc(RequireOwnership = false)]
    public void UpdateCubeServerRpc(Color colour)
    {
        UpdateCubeClientRpc(colour);
    }

    //Updates cube colour for each client
    [ClientRpc]
    public void UpdateCubeClientRpc(Color colour)
    {
        if(colour==Color.black)
        {
            cube.GetComponent<Renderer>().material=blackMaterial;
        }
        else if(colour==Color.red) 
        {
            cube.GetComponent<Renderer>().material = redMaterial;
        }
        else if (colour == Color.blue)
        {
            cube.GetComponent<Renderer>().material = blueMaterial;
        }
        else if (colour == Color.green)
        {
            cube.GetComponent<Renderer>().material = greenMaterial;
        }
        else if (colour == Color.yellow)
        {
            cube.GetComponent<Renderer>().material = yellowMaterial;
        }

    }

    //Requests level text to be updated for non-hosts
    [ServerRpc(RequireOwnership = false)]
    public void UpdateTextServerRpc(string text)
    {
        UpdateTextClientRpc(text);
    }

    //Updates level text for each client
    [ClientRpc]
    public void UpdateTextClientRpc(string text)
    {
        debugText.text = text;

    }

    //Increments current level of Simon Says
    public int IncrementLevel()
    {
        level += 1;
        return level;
    }


    //Begins colour sequence of Simon Says
    public IEnumerator BeginSimonSays()
    {
       
        Debug.Log("In Simon");
        if (level == 4)
        {
            Debug.Log("Puzzle complete");
            string texter = "Congrats you completed the puzzle!";
            UpdateTextServerRpc(texter);
            yield return new WaitForSeconds(3.0f);
            puzzleData.completePuzzle.CompletePuzzleServerRpc(0, PuzzleConstants.SIMON_ID);
            yield break;
        }
        string text = "Level = " + level;
        UpdateTextServerRpc(text);
        demoInProgress = true;
        generatedSequence.Clear();

        playerSequence.Clear();
        Debug.Log("player sequence cleared");

        UpdateCubeServerRpc(Color.black);
        yield return new WaitForSeconds(2.0f);
        //int counter = 0;
        if (counter == 0 && NetworkManager.Singleton.LocalClientId == puzzleData.connectedClients[0])
        {

        
            for (int i = 0; i < level * 2; i++)
            {

                Debug.Log("In for loop" + i);

                randColour.Value = Random.Range(0, 4);
                //int randColour = Random.Range(0, 3);
                generatedSequence.Add(randColour.Value);
                Debug.Log("RAND COLOUR" + randColour.Value);

                if (randColour.Value == red)
                {

                    UpdateCubeServerRpc(Color.red);
                } else if (randColour.Value == blue)
                {

                    UpdateCubeServerRpc(Color.blue);
                }
                else if (randColour.Value == green)
                {

                    UpdateCubeServerRpc(Color.green);
                }
                else if (randColour.Value == yellow)
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
    //Function that keeps track of if player input sequence matches colour sequence
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
