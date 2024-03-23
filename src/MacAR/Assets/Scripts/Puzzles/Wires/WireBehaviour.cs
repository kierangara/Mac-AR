using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class WireBehaviour : PuzzleBase
{
    public PassiveWires passiveWire;
    public ActiveWires activeWire;
    public GameObject passiveObj;
    public GameObject activeObj;
    public PuzzleData puzzleData;

    private List<List<uint>> initSequence = new List<List<uint>>();

    // Start is called before the first frame update
    void Start()
    {   
        //InitializePuzzle();
    }

    public override void InitializePuzzle()
    {
        var rootOrder = RandomList();
        var wireOrder = RandomList();

        for(int i = 0; i < rootOrder.Count; i++)
        {
            List<uint> inner = new List<uint>{rootOrder[i], wireOrder[i]};
            initSequence.Add(inner);
        }

        passiveWire.Init(initSequence);
        activeWire.Init(initSequence);

        if (NetworkManager.Singleton.LocalClientId == puzzleData.connectedClients[0])
        {
            passiveObj.transform.localScale = new Vector3(0, 0, 0);
        }
        else 
        {
            activeObj.transform.localScale = new Vector3(0, 0, 0);
        }
    }

    public List<uint> RandomList()  
    {  
        var baseList = new List<uint>{0, 1, 2, 3};
        var random = new System.Random();  
        int n = baseList.Count;  

        for(int i = baseList.Count - 1; i > 1; i--)
        {
            int rnd = random.Next(i + 1);  

            uint value = baseList[rnd];  
            baseList[rnd] = baseList[i];  
            baseList[i] = value;
        }

        return baseList;
    }

    public override void SetActive(bool status)
    {
        active = status;

        if(status == true)
        {
            activeWire.SetActive();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
