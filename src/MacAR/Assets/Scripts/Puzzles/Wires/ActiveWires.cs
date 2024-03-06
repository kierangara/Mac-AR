using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWires : MonoBehaviour
{
    public List<List<uint>> m_sequence;
    public List<int> currentSequence = new List<int>();
    public WireBehaviour wireMain;
    public PuzzleData puzzleData;

    public void Init(List<List<uint>> sequence)
    {
        m_sequence = sequence;

        for(int i = 0; i < 4; i++)
        {
            currentSequence.Add(-1);
        }
    }

    public bool UpdateSequence(int wire, int anchor)
    {
        currentSequence[wire] = anchor;

        bool correctSequence = true;

        for(int i = 0; i < 4; i++)
        {
            int expectedAnchor = (int)m_sequence[i][0];
            int expectedWire = (int)m_sequence[i][1];

            if(currentSequence[expectedWire] != expectedAnchor)
            {
                correctSequence = false;
            }
        }

        if(correctSequence)
        {
            Debug.Log("Correct Wires");
            puzzleData.completePuzzle.CompletePuzzleServerRpc(0);
        }

        return correctSequence;
    }

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
