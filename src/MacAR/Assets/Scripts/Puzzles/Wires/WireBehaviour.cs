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

    private List<List<uint>> testSequence;

    // Start is called before the first frame update
    void Start()
    {   
        // InitializePuzzle();
    }

    public override void InitializePuzzle()
    {
        testSequence = new List<List<uint>>{new List<uint>{1, 3}, 
                                            new List<uint>{0, 0}, 
                                            new List<uint>{3, 2}, 
                                            new List<uint>{2, 1}};

        passiveWire.Init(testSequence);
        activeWire.Init(testSequence);

        if (NetworkManager.Singleton.LocalClientId == puzzleData.connectedClients[0])
        {
            passiveObj.transform.localScale = new Vector3(0, 0, 0);
        }
        else 
        {
            activeObj.transform.localScale = new Vector3(0, 0, 0);
        }
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
