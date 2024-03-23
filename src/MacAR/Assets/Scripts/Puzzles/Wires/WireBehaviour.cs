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
        var rootOrder = RandomList(this.seed);
        var wireOrder = RandomList(this.seed-1);

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

    public List<uint> RandomList(int seed)  
    {  
        // Absolute Value
        if(seed < 0)
        {
            seed *= -1;
        }

        //Debug.Log("Seed: " + seed);
        var baseList = new List<uint>{0, 1, 2, 3}; 
        
        // Switch Random Indices
        for(int i = 0; i < (seed % 10); i++)
        {
            (baseList[seed%2], baseList[seed%3]) = (baseList[seed%3], baseList[seed%2]);
            (baseList[0], baseList[seed%4]) = (baseList[seed%4], baseList[0]);
            (baseList[i%4], baseList[seed%2]) = (baseList[seed%2], baseList[i%4]);
            //Debug.Log("Switch: " + (seed%3) + (seed%2));
        }

        // Rotate
        for(int i = 0; i < (seed%3); i++)
        {
            (baseList[0], baseList[1], baseList[2], baseList[3]) =
                (baseList[3], baseList[0], baseList[1], baseList[2]);
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
