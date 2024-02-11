using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public int wireID;
    public PuzzleData puzzleData;
    public WireCollider collisionObject;
    public Transform baseAnchor;
    public AnchorList anchorList; 
    public Transform mainPuzzle;
    public Transform wireRoot;
    public ActiveWires active;

    private Camera mainCamera;
    private float cameraZDistance;
    private Vector3 cameraDistance;
    private Vector3 initialScale;
    private float initialZPos;
    private Vector3 previousPos;
    private Vector3 currentPos;
    
    // Start is called before the first frame update
    void Start()
    {
        initialScale = mainPuzzle.localScale; 
        mainCamera = puzzleData.cam;
        initialZPos = baseAnchor.localPosition.z;
    }

    private void OnMouseDrag()
    {
        // if(anchorList.Contains(collisionObject.collidedObject) || collisionObject.collidedObject == null)
        // {
        //     MoveCylinder();
        // }
        // else
        // {
        //     Debug.Log("Colliding: " + previousPos);
        //     currentPos = previousPos;
        //     SetPosition(previousPos, false);
        // }

        // Clear Connections
        active.UpdateSequence(wireID, -1);

        // Move Cylinder
        MoveCylinder();

        // Transform transform = mainCamera.transform;
        
        // Debug.Log(LayerMask.NameToLayer("Ignore Raycast"));
        // if (Physics.Raycast (transform.position, transform.forward, Mathf.Infinity, ~LayerMask.NameToLayer("Ignore Raycast")))
        // {
        //     Debug.Log("Cast");
        // }
    }

    private void MoveCylinder()
    {
        Quaternion puzzleRot = Quaternion.Euler(mainPuzzle.eulerAngles);

        cameraDistance = mainCamera.transform.position - mainPuzzle.position;
        Quaternion cameraRot = Quaternion.Euler(-mainCamera.transform.eulerAngles);

        Vector3 mouseScreenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraDistance.magnitude);

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);

        Vector3 realPos = mouseWorldPosition;
        realPos.z *= -1;
        realPos = puzzleRot * realPos;

        Vector3 relPuzzle = mainPuzzle.localPosition;
        relPuzzle.z *= -1;
        relPuzzle = puzzleRot * relPuzzle;

        Vector3 relRoot = wireRoot.localPosition;

        Vector3 endPos = realPos - relRoot - relPuzzle;

        // Re-Enable bound checking when working
        // Debug.Log("EndPos: " + endPos);
        // if(WithinBounds(endPos - relRoot))
        // {
        SetPosition(endPos);
        // }
    }

    private bool WithinBounds(Vector3 endPos)
    {
        var bounds = mainPuzzle.gameObject.GetComponent<Collider>().bounds.size;
        if(endPos.x < -bounds.x || endPos.y > bounds.y ||
           endPos.x > 0 || endPos.y < 0)
        {
            return false;
        }

        return true;
    }

    void OnMouseUp()
    {
        if(anchorList.Contains(collisionObject.collidedObject))
        {
            SetPosition(collisionObject.collidedObject.gameObject.transform.localPosition -
                        wireRoot.localPosition);

            active.UpdateSequence(wireID, anchorList.IndexOf(collisionObject.collidedObject));
        }
    }

    private void SetPosition(Vector3 endPos, bool update = true)
    {
        // Debug.Log("Endpos: " + endPos);
        // Save Positions
        previousPos = currentPos;
        currentPos = endPos;

        // Overwrite Z Position
        endPos.z = initialZPos;

        // Move Physical Wire
        float distance = Vector3.Distance(baseAnchor.localPosition, endPos);
        transform.localScale = new Vector3(initialScale.x, distance/2f, initialScale.z);

        Vector3 middlePoint = (baseAnchor.localPosition + endPos)/2f;
        transform.localPosition = middlePoint;

        Vector3 rotationDirection = (endPos - baseAnchor.localPosition);
        transform.up = Quaternion.Euler(mainPuzzle.eulerAngles) * rotationDirection;

        // Move Collider
        collisionObject.GetComponent<Collider>().transform.localPosition = endPos;
    }
}
