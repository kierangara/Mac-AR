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
    private Vector3 cameraDistance;
    private Vector3 initialScale;
    private float initialZPos;
    
    // Start is called before the first frame update
    void Start()
    {
        initialScale = mainPuzzle.localScale; 
        mainCamera = Camera.main;
        // cameraZDistance = mainCamera.WorldToScreenPoint(transform.position).z;
        initialZPos = baseAnchor.localPosition.z;
    }

    private void OnMouseDrag()
    {
        Quaternion puzzleRot = Quaternion.Euler(mainPuzzle.eulerAngles);
        // Vector3 testVector = new Vector3(1, 2, 3);
        // Debug.Log(puzzleRot * testVector);

        cameraDistance = mainCamera.transform.position - mainPuzzle.position;
        // Debug.Log("Distance 0: " + cameraDistance);
        // Debug.Log("Rot: " + mainPuzzle.eulerAngles);
        // cameraDistance = puzzleRot * cameraDistance;
        Quaternion cameraRot = Quaternion.Euler(-mainCamera.transform.eulerAngles);

        Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraDistance.magnitude);
        // Vector3 a = mouseScreenPosition;
        // mouseScreenPosition += cameraDistance;

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        // Vector3 realPos = mouseWorldPosition + cameraDistance;
        Vector3 realPos = mouseWorldPosition;
        realPos.z *= -1;
        realPos = puzzleRot * realPos;
        // mouseWorldPosition += cameraDistance;

        // Vector3 relPos = puzzleRot * mouseWorldPosition;
        // Vector3 puzzleRelPos = puzzleRot * mainPuzzle.localPosition;
        // SetPosition(relPos - wireRoot.localPosition - puzzleRelPos);
        Vector3 relPuzzle = mainPuzzle.localPosition;
        relPuzzle.z *= -1;
        relPuzzle = puzzleRot * relPuzzle;
        // relPuzzle.z *= -1;

        Vector3 relRoot = wireRoot.localPosition;
        // relRoot = puzzleRot * relRoot;
        // relRoot.z *= -1;
        // Vector3 endPos = realPos - wireRoot.localPosition - mainPuzzle.localPosition;
        Vector3 endPos = realPos - relRoot - relPuzzle;
        // Quaternion puzzleRot = Quaternion.Euler(mainPuzzle.eulerAngles);
        SetPosition(endPos);

        // Debug.Log("Screen:" + mouseScreenPosition);
        Debug.Log("World: " + mouseWorldPosition);
        Debug.Log("Real: " + realPos);
        Debug.Log("End:" + endPos);
        Debug.Log("Puzzle Rot: " + puzzleRot);
        // Debug.Log("Cam Rot:" + cameraRot);
        // Debug.Log("Tot Rot:" + puzzleRot * cameraRot);
        // Debug.Log("Rel: " + relPos);
        // Debug.Log("World: " + mouseWorldPosition);
        // // Debug.Log("Distance 1: " + cameraDistance);
        // Debug.Log("Original: " + a);
        // Debug.Log("After: " + mouseScreenPosition);
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
        // transform.up = rotationDirection;
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
