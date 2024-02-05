using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public Transform WorldAnchor;

    private Camera mainCamera;
    private float cameraZDistance;
    private Vector3 initialScale;
    
    // Start is called before the first frame update
    void Start()
    {
        initialScale = transform.localScale;
        mainCamera = Camera.main;
        cameraZDistance = mainCamera.WorldToScreenPoint(transform.position).z;
    }

    private void OnMouseDrag()
    {
        Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraZDistance);
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);

        float distance = Vector3.Distance(WorldAnchor.position, mouseWorldPosition);
        transform.localScale = new Vector3(initialScale.x, distance/2f, initialScale.z);

        Vector3 middlePoint = (WorldAnchor.position + mouseWorldPosition)/2f;
        transform.position = middlePoint;

        Vector3 rotationDirection = (mouseWorldPosition - WorldAnchor.position);
        transform.up = rotationDirection;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
