using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class CombinationPuzzle : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI codeInputField;

    string[] codeCombo;
    string currentCode;
    int currentDigit;
    // Start is called before the first frame update
    void Start()
    {
        foreach(char j in codeInputField.text){
            currentCode+=j;
        }
        currentDigit = 0;
        string code = "1359";
        string instr1 = "The second row and column each contain one number";
        string instr2 = "The second number is the only number in the first column";
        string instr3 = "The first number is two greater than the second number";
        string instr4 = "The fourth number is the only number in the third row";
        codeCombo = new string[]{code,instr1, instr2, instr3, instr4};
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KeyPadPress(string number){
        string temp;
        Debug.Log($"Clicked {number}");
        char correctDigit = codeCombo[0][currentDigit];
        if(char.Parse(number)==correctDigit){
            if(currentDigit==0){
                temp = number + currentCode.Substring(1,currentCode.Length-1);
            }
            else if(currentDigit<3){
                temp = currentCode.Substring(0,currentDigit*2) + number 
                + currentCode.Substring(currentDigit*2+1,currentCode.Length-(currentDigit*2+1));
            }
            else{
                temp = "Correct";
            }
            Debug.Log(temp);
            currentCode = temp;
            codeInputField.text = temp;
            currentDigit+=1;
        }
        else{
            currentDigit = 0;
            currentCode = "_ _ _ _";
            codeInputField.text = "_ _ _ _";
        }
    }
}
