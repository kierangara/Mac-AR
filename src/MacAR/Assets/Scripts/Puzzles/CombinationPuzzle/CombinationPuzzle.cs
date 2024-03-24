using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Unity.Netcode;
using System.Linq;
using System;


public class CombinationPuzzle : PuzzleBase
{
    [SerializeField] TextMeshProUGUI codeInputField;
    [SerializeField] GameObject combinationPuzzle;

    [SerializeField] TextMeshProUGUI instructionPage;

    [SerializeField] Material wrongKeypadMat;

    [SerializeField] Material keypadMat;

    [SerializeField] GameObject keypad;
    public PuzzleData puzzleData;
    private string[] codeCombo;
    private string currentCode;
    private int currentDigit;

    private int wrongInputTimer;
    private int numPosibilities = 5;
    // Start is called before the first frame update
    void Start()
    {
        //Read initial placeholder code
        Debug.Log("Combo Puzzle Started");
    }

    public override void InitializePuzzle()
    {
        puzzleId = PuzzleConstants.COMBINATION_ID;
        
        wrongInputTimer = -1;
        foreach(char j in codeInputField.text){
            currentCode+=j;
        }
        currentDigit = 0;
        //Create code and instructions
        //string code = "3159";
        //string instr1 = "The second row and column each contain one number\n";
        //string instr2 = "The second number is the only number in the first column\n";
        //string instr3 = "The first number is two greater than the second number\n";
        //string instr4 = "The fourth number is the only number in the third row\n";
        GenerateCodeServerRpc();
        for(int i = 0; i<puzzleData.connectedClients.Count; i++){
            if (NetworkManager.Singleton.LocalClientId == puzzleData.connectedClients[i])
            {
                Debug.Log(i);
                Debug.Log(instructionPage.text);
                instructionPage.text = "\n"+codeCombo[i+1];
                if(puzzleData.connectedClients.Count<(codeCombo.Length-(i+1))){
                    instructionPage.text += "\n"+codeCombo[puzzleData.connectedClients.Count+i+1];
                }
                //instructionPage.text = "Hello";
            }
        }
        Debug.Log("COMBO PUZZLE INITIALIZED");
    }

    // Update is called once per frame
    void Update()
    {
        if(wrongInputTimer!=-1){
            wrongInputTimer+=1;
            if(wrongInputTimer>=10){
                keypad.GetComponent<Renderer>().material = keypadMat;
            }
        }
    }

    //Button reaction
    public void KeyPadPress(string number){
        string temp;
        //Get digit from code to test input against
        char correctDigit = codeCombo[0][currentDigit];
        //Check if input is correct digit, output to screen if correct and move on to next digit
        if(char.Parse(number)==correctDigit){
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
                puzzleData.completePuzzle.CompletePuzzleServerRpc(0, PuzzleConstants.COMBINATION_ID);
            }
            Debug.Log(temp);
            currentCode = temp;
            codeInputField.text = temp;
            currentDigit+=1;
        }
        //Incorrect digit entered, reset.
        else{
            keypad.GetComponent<Renderer>().material = wrongKeypadMat;
            wrongInputTimer = 0;
            currentDigit = 0;
            currentCode = "_ _ _ _";
            codeInputField.text = "_ _ _ _";
        }
    }


    private void generateCode(int numPosibilities, int randNum){
        codeCombo = GenerateInstructionSet()[randNum];
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

    //Button action on side of commit
    [ServerRpc(RequireOwnership = false)]
    public void KeyPressedServerRpc(string number)
    {
        KeyPressedClientRpc(number);
    }

    //Button action applied to other users
    [ClientRpc]
    public void KeyPressedClientRpc(string number)
    {
        KeyPadPress(number);
    }

    [ServerRpc]
    public void GenerateCodeServerRpc(){
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
        int randNum = UnityEngine.Random.Range(0,numPosibilities);
        GenerateCodeClientRpc(randNum);
    }
    [ClientRpc]
    public void GenerateCodeClientRpc(int randomNum){
        generateCode(numPosibilities,randomNum);
    }

    private string[][] GenerateInstructionSet(){
        string[][] instructionSet = new string[5][];
        string code = "3159";
        string instr1 = "The second row and column of the keypad combined contain only one number of the combination\n";
        string instr2 = "The second number of the combination is the only number in the first column of the keypad\n";
        string instr3 = "The first number of the combination is two greater than the second number\n";
        string instr4 = "The fourth number of the combination is the only number in the third row of the keypad\n";
        instructionSet[0] = new string[]{code,instr1, instr2, instr3, instr4};
        code = "6194";
        instr1 = "The second number of the combination is just above the fourth on the keypad\n";
        instr2 = "The third number of the combination is 9, and it is 5 more than the fourth number\n";
        instr3 = "The fourth number of the combination is two less than the first number\n";
        instr4 = "The first number of the combination is in the third column of the keypad\n";
        instructionSet[1] = new string[]{code,instr1, instr2, instr3, instr4};
        code = "7531";
        instr1 = "The first number of the combination is 7\n";
        instr2 = "All numbers in the combination are odd\n";
        instr3 = "No numbers repeat themselves in the combination\n";
        instr4 = "The numbers in the combination are in decreasing order\n";
        instructionSet[2] = new string[]{code,instr1, instr2, instr3, instr4};
        code = "2349";
        instr1 = "The last number of the combination is 9\n";
        instr2 = "Three numbers of the combination are consecutive\n";
        instr3 = "The first number of the combination is 7 less than the last number\n";
        instr4 = "The second number of the combination is 3\n";
        instructionSet[3] = new string[]{code,instr1, instr2, instr3, instr4};
        code = "4848";
        instr1 = "The first number of the combination is 4\n";
        instr2 = "The first number of the combination is half the last number\n";
        instr3 = "The third and first number of the combination add up to the last number\n";
        instr4 = "The second number of the combination is the same as the last number\n";
        instructionSet[4] = new string[]{code,instr1, instr2, instr3, instr4};
        return instructionSet;
    }
}
