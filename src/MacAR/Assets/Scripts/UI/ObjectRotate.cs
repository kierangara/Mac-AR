using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ObjectRotate : MonoBehaviour
{
    public float PCRotationSpeed = 10f;
    public float MobileRotationSpeed = 0.1f;
    public PuzzleData puzzleData; 

    void OnMouseDrag()
    {
        float rotX = Input.GetAxis("Mouse X") * PCRotationSpeed;
        float rotY = Input.GetAxis("Mouse Y") * PCRotationSpeed;

        Vector3 right = Vector3.Cross(puzzleData.cam.transform.up, transform.position - puzzleData.cam.transform.position);
        Vector3 up = Vector3.Cross(transform.position - puzzleData.cam.transform.position, right);
        transform.rotation = Quaternion.AngleAxis(-rotX, up) * transform.rotation;
        transform.rotation = Quaternion.AngleAxis(rotY, right) * transform.rotation;
    }

    void Update ()
    {
        // get the user touch input
        foreach (Touch touch in Input.touches) {
            Ray camRay = puzzleData.cam.ScreenPointToRay (touch.position);
            RaycastHit raycastHit;
            if(Physics.Raycast (camRay, out raycastHit, 10))
            {
                if (touch.phase == TouchPhase.Moved) {
                    Debug.Log("Touch phase Moved");
                    transform.Rotate (touch.deltaPosition.y * MobileRotationSpeed, 
                        -touch.deltaPosition.x * MobileRotationSpeed, 0, Space.World);
                }    
            }
        }
    }
}