using System.Collections.Generic;
using UnityEngine;
public class PassiveWires : MonoBehaviour
{
    public GameObject root1;
    public GameObject root2;
    public GameObject root3;
    public GameObject root4;

    public GameObject wire1;
    public GameObject wire2;
    public GameObject wire3;
    public GameObject wire4;

    public Material materialRed;
    public Material materialBlue;
    public Material materialGreen;
    public Material materialYellow;

    private List<List<uint>> m_sequence;
    private List<GameObject> roots;
    private List<GameObject> wires;

    private const int ROOT_INDEX = 0;
    private const int WIRE_INDEX = 1;

    public void Init(List<List<uint>> sequence)
    {
        roots = new List<GameObject>{root1, root2, root3, root4};
        wires = new List<GameObject>{wire1, wire2, wire3, wire4};

        m_sequence = sequence;

        for(int i = 0; i < 4; i++)
        {
            SetMaterial(roots[i], sequence[i][ROOT_INDEX]);
            SetMaterial(wires[i], sequence[i][WIRE_INDEX]);
        }
    }

    private void SetMaterial(GameObject gameObject, uint colour)
    {
        Material material = materialRed;

        if(colour == 1)
        {
            material = materialGreen;
        }
        else if(colour == 2)
        {
            material = materialBlue;
        }
        else if(colour == 3)
        {
            material = materialYellow;
        }

        gameObject.GetComponent<Renderer>().material = material;
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
