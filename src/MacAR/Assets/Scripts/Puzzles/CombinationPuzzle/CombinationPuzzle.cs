using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Unity.Netcode;
using System.Linq;


public class CombinationPuzzle : PuzzleBase
{
    [SerializeField] TextMeshProUGUI codeInputField;
    [SerializeField]  GameObject combinationPuzzle;
    public PuzzleData puzzleData;

    private string[] codeCombo;
    private string currentCode;
    private int currentDigit;
    // Start is called before the first frame update
    void Start()
    {
        //Read initial placeholder code
        Debug.Log("Combo Puzzle Started");
        foreach(char j in codeInputField.text){
            currentCode+=j;
        }
        currentDigit = 0;
        //Create code and instructions
        string code = "1359";
        string instr1 = "The second row and column each contain one number";
        string instr2 = "The second number is the only number in the first column";
        string instr3 = "The first number is two greater than the second number";
        string instr4 = "The fourth number is the only number in the third row";
        codeCombo = new string[]{code,instr1, instr2, instr3, instr4};
    }

    public override void InitializePuzzle(){
        if (NetworkManager.Singleton.LocalClientId == puzzleData.connectedClients[0])
        {

        }
        Debug.Log("COMBO PUZZLE INITIALIZED");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KeyPadPress(string number){
        string temp;
        Debug.Log($"Clicked {number}");
        //Get digit from code to test input against
        char correctDigit = codeCombo[0][currentDigit];
        //Check if input is correct digit, output to screen if correct and move on to next digit
        Debug.Log("Test1");
        if(char.Parse(number)==correctDigit){
            Debug.Log("Test3");
            if(currentDigit==0){
                temp = number + currentCode.Substring(1,currentCode.Length-1);
            }
            else if(currentDigit<3){
                temp = currentCode.Substring(0,currentDigit*2) + number 
                + currentCode.Substring(currentDigit*2+1,currentCode.Length-(currentDigit*2+1));
            }
            //Puzzle complete
            else{
                temp = "Correct";
            }
            Debug.Log(temp);
            currentCode = temp;
            codeInputField.text = temp;
            currentDigit+=1;
        }
        //Incorrect digit entered, reset.
        else{
            currentDigit = 0;
            currentCode = "_ _ _ _";
            codeInputField.text = "_ _ _ _";
            Debug.Log(currentCode);
            Debug.Log(codeInputField.text);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestPuzzleDataServerRpc()
    {
        RequestPuzzleDataClientRpc();
    }

    [ClientRpc]
    public void RequestPuzzleDataClientRpc()
    {
        if(puzzleData.connectedClients!=null)
        {
            SendPuzzleDataServerRpc(puzzleData.connectedClients.ToArray());
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SendPuzzleDataServerRpc(ulong[] p)
    {
        SendPuzzleDataClientRpc(p);
    }

    [ClientRpc]
    public void SendPuzzleDataClientRpc(ulong[] p)
    {
        puzzleData.connectedClients = p.ToList();
        //Debug.Log("PuzzleDataUpdated");
    }

    [ServerRpc(RequireOwnership = false)]
    public void KeyPressedServerRpc(int[] mazeLayouts)
    {
        KeyPressedClientRpc(mazeLayouts);
    }

    [ClientRpc]
    public void KeyPressedClientRpc(int[] mazeLayouts)
    {
        
    }
}
