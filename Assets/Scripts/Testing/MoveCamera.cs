using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField]
    private Camera camera;

    [SerializeField]
    private float zoomStep, minCamSize, maxCamSize;

    private Vector3 dragOrigin;
    

    void Update()
    {
        PanCamera();
        ChangeZoom();
    }

    private void PanCamera()
    {
        if(Input.GetMouseButtonDown(0))
        {
            dragOrigin=camera.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - camera.ScreenToWorldPoint(Input.mousePosition);
            camera.transform.position += difference;
        }
    }
    private void ChangeZoom()
    {
        float newSize = camera.orthographicSize;
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            newSize -= zoomStep;
        }
        if(Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            newSize += zoomStep;
        }
        if (Input.mouseScrollDelta.y > 0)
        {
            newSize -= zoomStep * Time.deltaTime * 5f;
        }
        if (Input.mouseScrollDelta.y<0)
        {
            newSize += zoomStep * Time.deltaTime * 5f;
        }

        camera.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);

    }

    private void ZoomOut()
    {

    }
}
