using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorList : MonoBehaviour
{
    public List<Collider> anchors;

    public bool Contains(Collider collider)
    {
        if(collider == null)
        {
            return false;
        }

        return anchors.Contains(collider);
    }

    public int IndexOf(Collider collider)
    {
        return anchors.IndexOf(collider);
    }
}
