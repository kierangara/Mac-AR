using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public WireCollider collisionObject;
    public Transform baseAnchor;
    public AnchorList anchorList; 
    public Transform mainPuzzle;
    public Transform wireRoot;

    private Camera mainCamera;
    private float cameraZDistance;
    private Vector3 initialScale;
    private float initialZPos;
    
    // Start is called before the first frame update
    void Start()
    {
        initialScale = mainPuzzle.localScale; 
        mainCamera = Camera.main;
        cameraZDistance = mainCamera.WorldToScreenPoint(transform.position).z;
        initialZPos = baseAnchor.localPosition.z;
    }

    private void OnMouseDrag()
    {
        Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraZDistance);
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);

        Vector3 relPos = Quaternion.Euler(mainPuzzle.eulerAngles) * mouseWorldPosition;
        Vector3 puzzleRelPos = Quaternion.Euler(mainPuzzle.eulerAngles) * mainPuzzle.localPosition;
        SetPosition(relPos - wireRoot.localPosition - puzzleRelPos);

        Debug.Log("Rot:" + mainPuzzle.eulerAngles);
        Debug.Log("Screen: " + mouseScreenPosition);
        Debug.Log("World: " + mouseWorldPosition);
        // Debug.Log("Rel Pos: " + relPos);
    }

    void OnMouseUp()
    {
        if(anchorList.Contains(collisionObject.collidedObject))
        {
            // Debug.Log(collisionObject.collidedObject.gameObject.transform.localPosition);
            SetPosition(collisionObject.collidedObject.gameObject.transform.localPosition -
                        wireRoot.localPosition);
        }
    }

    private void SetPosition(Vector3 endPos, bool debug = false)
    {
        // Overwrite Z Position
        endPos.z = initialZPos;
        // Debug.Log(initialZPos);
        // Debug.Log(endPos);
        // Debug.Log(baseAnchor.position);

        // Move Physical Wire
        float distance = Vector3.Distance(baseAnchor.localPosition, endPos);
        transform.localScale = new Vector3(initialScale.x, distance/2f, initialScale.z);

        Vector3 middlePoint = (baseAnchor.localPosition + endPos)/2f;
        transform.localPosition = middlePoint;
        // if(debug) Debug.Log("Initial: " + transform.position);

        Vector3 rotationDirection = (endPos - baseAnchor.localPosition);
        transform.up = Quaternion.Euler(mainPuzzle.eulerAngles) * rotationDirection;
        // if(debug) Debug.Log("After Rot: " + transform.position);

        // Move Collider
        collisionObject.GetComponent<Collider>().transform.localPosition = endPos;
        // if(debug) Debug.Log("After Collider: " + transform.position);
        // wireRoot.GetComponent<Collider>().transform.localScale = new Vector3(1, 1, 1);

        // Debug.Log(baseAnchor.localPosition);
        // Debug.Log(endPos);
        // Debug.Log(middlePoint);

        if(debug)
        {
            // Debug.Log("Top: " + endPos);
            // Debug.Log("Mid: " + middlePoint);
            // Debug.Log("Bottom: " + baseAnchor.position);
            // Debug.Log("Up: " + middlePoint + rotationDirection);
            // Debug.Log("Actual: " + transform.position);
            // Vector3 scale = new Vector3(initialScale.x, distance/2f, initialScale.z);
            // Debug.Log("Scale: " + scale);
        }
    }
}
