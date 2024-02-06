using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireCollider : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        Debug.Log("Collision");
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Collision Exit");
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
