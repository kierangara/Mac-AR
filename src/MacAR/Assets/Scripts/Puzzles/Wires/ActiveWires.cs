using System.Collections.Generic;
using UnityEngine;
public class ActiveWires : MonoBehaviour
{
    public List<List<uint>> m_sequence;
    public List<int> currentSequence = new List<int>();
    public WireBehaviour wireMain;
    public PuzzleData puzzleData;
    public List<GameObject> lightFixtures;
    public List<Light> lights;

    public void Init(List<List<uint>> sequence)
    {
        m_sequence = sequence;

        for(int i = 0; i < 4; i++)
        {
            currentSequence.Add(-1);
        }
    }

    public void SetActive()
    {
        for(int i = 0; i < lightFixtures.Count; i++)
        {
            lightFixtures[i].GetComponent<Renderer>().material.color = Color.red;
        }

        for(int i = 0; i < lights.Count; i++)
        {
            lights[i].color = Color.red;
            lights[i].intensity = 1;
        }
    }

    public bool UpdateSequence(int wire, int anchor)
    {
        // Set New Sequence
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

        // Complete Puzzle if Correct
        if(correctSequence)
        {
            for(int i = 0; i < lightFixtures.Count; i++)
            {
                lightFixtures[i].GetComponent<Renderer>().material.color = Color.green;
            }

            for(int i = 0; i < lights.Count; i++)
            {
                lights[i].color = Color.green;
            }

            puzzleData.completePuzzle.CompletePuzzleServerRpc(0, PuzzleConstants.WIRE_ID);
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
