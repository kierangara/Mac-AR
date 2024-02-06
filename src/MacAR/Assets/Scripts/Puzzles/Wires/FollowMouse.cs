using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
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
        transform.localScale = new Vector3(initialScale.x, distance/2f, initialScale.z);

        Vector3 middlePoint = (baseAnchor.position + endPos)/2f;
        transform.position = middlePoint;
        if(debug) Debug.Log("Initial: " + transform.position);

        Vector3 rotationDirection = (endPos - baseAnchor.position);
        transform.up = rotationDirection;
        if(debug) Debug.Log("After Rot: " + transform.position);

        // Move Collider
        collisionObject.GetComponent<Collider>().transform.position = endPos;
        // if(debug) Debug.Log("After Collider: " + transform.position);
        // wireRoot.GetComponent<Collider>().transform.localScale = new Vector3(1, 1, 1);

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
