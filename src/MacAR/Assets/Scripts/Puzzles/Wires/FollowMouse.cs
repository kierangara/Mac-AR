using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public GameObject wireRoot;
    public WireCollider collisionObject;
    public Transform baseAnchor;
    public List<Transform> finalPositions;

    private Camera mainCamera;
    private float cameraZDistance;
    private Vector3 initialScale;
    
    // Start is called before the first frame update
    void Start()
    {
        // TODO: May need to be scale of parent prefab
        // Noticed odd disconnect from anchor when scale was changed
        initialScale = transform.localScale; 
        mainCamera = Camera.main;
        cameraZDistance = mainCamera.WorldToScreenPoint(transform.position).z;
    }

    private void OnMouseDrag()
    {
        Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraZDistance);
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);

        SetPosition(mouseWorldPosition);
    }

    void OnMouseUp()
    {
        if(collisionObject.collidedObject != null)
        {
            SetPosition(collisionObject.collidedObject.gameObject.transform.position, true);
        }
    }

    private void SetPosition(Vector3 endPos, bool debug = false)
    {
        // Move Physical Wire
        float distance = Vector3.Distance(baseAnchor.position, endPos);
        wireRoot.transform.localScale = new Vector3(initialScale.x, distance/2f, initialScale.z);

        Vector3 middlePoint = (baseAnchor.position + endPos)/2f;
        wireRoot.transform.position = middlePoint;
        if(debug) Debug.Log("Initial: " + transform.position);

        Vector3 rotationDirection = (endPos - baseAnchor.position);
        wireRoot.transform.up = rotationDirection;
        if(debug) Debug.Log("After Rot: " + transform.position);

        // Move Collider
        // wireRoot.GetComponent<Collider>().transform.position = endPos; //- new Vector3(0, 1, 0);
        // if(debug) Debug.Log("After Collider: " + transform.position);

        if(debug)
        {
            // Debug.Log("Top: " + endPos);
            // Debug.Log("Mid: " + middlePoint);
            // Debug.Log("Bottom: " + baseAnchor.position);
            // Debug.Log("Up: " + middlePoint + rotationDirection);
            Debug.Log("Actual: " + transform.position);
            // Vector3 scale = new Vector3(initialScale.x, distance/2f, initialScale.z);
            // Debug.Log("Scale: " + scale);
        }
    }
}
