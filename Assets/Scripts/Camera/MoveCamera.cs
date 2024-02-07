using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveCamera : MonoBehaviour
{
    [SerializeField]
    private Camera camera;

    private Image button;

    [SerializeField]
    private float zoomStep, minCamSize, maxCamSize;

    private Vector3 dragOrigin;

    public bool IsInCameraMode;

    private void Start()
    {
        button = GameObject.Find("MoveCameraButton").GetComponent<Image>();
        IsInCameraMode = false;
    }
    void Update()
    {
        if (IsInCameraMode)
        {
            PanCamera();
            PanCameraByArrowKeys();
            ChangeZoom();
            button.color = Color.red;
        }
        else
        {
            button.color = Color.white;
        }
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

    private void PanCameraByArrowKeys()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            camera.transform.position += new Vector3(-100, 0, 0) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            camera.transform.position += new Vector3(100, 0, 0) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            camera.transform.position += new Vector3(0, 100, 0) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            camera.transform.position += new Vector3(0, -100, 0) * Time.deltaTime;
        }
    }
    private void ChangeZoom()
    {
        float newSize = camera.orthographicSize;
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            newSize -= zoomStep;
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            newSize += zoomStep;
        }
        if (Input.mouseScrollDelta.y > 0)
        {
            newSize -= zoomStep * Time.deltaTime * 5f;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            newSize += zoomStep * Time.deltaTime * 5f;
        }

        camera.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);

    }

    public void ToggleCameraMode()
    {
        if (IsInCameraMode)
        {
            IsInCameraMode = false;
        }
        else if (!IsInCameraMode)
        {
            IsInCameraMode= true;
        }
    }
}
