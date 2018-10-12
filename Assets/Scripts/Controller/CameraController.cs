using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float zoomSpeed = 4.0f;      // Speed of the camera going back and forth

    private Vector3 mouseOrigin;    // Position of cursor when mouse dragging starts
    private bool isZooming;     // Is the camera zooming?


    void Start ()
    {
       
	}
	
	
	void Update ()
    {

        float xAxisValue = Input.GetAxis("Horizontal");
        float zAxisValue = Input.GetAxis("Vertical");
        if (Camera.current != null)
        {
            Camera.current.transform.Translate(new Vector3(xAxisValue, 0.0f, zAxisValue));
        }

        // Get the middle mouse button
        if (Input.GetMouseButtonDown(2))
        {
            // Get mouse origin
            mouseOrigin = Input.mousePosition;
            isZooming = true;
        }

        if (!Input.GetMouseButton(2)) isZooming = false;

        // Move the camera linearly along Z axis
        if (isZooming)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

            Vector3 move = pos.y * zoomSpeed * transform.forward;
            transform.Translate(move, Space.World);
        }
    }
}
