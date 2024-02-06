using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public GameObject wireRoot;
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
        // Move Physical Wire
        Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraZDistance);
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);

        float distance = Vector3.Distance(baseAnchor.position, mouseWorldPosition);
        transform.localScale = new Vector3(initialScale.x, distance/2f, initialScale.z);

        Vector3 middlePoint = (baseAnchor.position + mouseWorldPosition)/2f;
        transform.position = middlePoint;

        Vector3 rotationDirection = (mouseWorldPosition - baseAnchor.position);
        transform.up = rotationDirection;

        // Move Collider
        wireRoot.GetComponent<Collider>().transform.position = mouseWorldPosition - new Vector3(0, 1, 0);
    }

    void OnMouseUp()
    {
        Debug.Log("Mouse Released");
    }
}
