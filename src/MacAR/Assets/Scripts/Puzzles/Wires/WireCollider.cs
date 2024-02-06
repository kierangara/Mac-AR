using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireCollider : MonoBehaviour
{
    public Collider collidedObject = null;

    void OnTriggerStay(Collider other)
    {
        collidedObject = other;
    }

    void OnTriggerExit(Collider other)
    {
        collidedObject = null;
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
