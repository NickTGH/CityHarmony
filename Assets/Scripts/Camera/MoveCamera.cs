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
    private float zoomStep, minCamSize, maxCamSize, defaultCamSize;

    private Vector3 dragOrigin;

    public bool IsInCameraMode;

    public Vector2 minValues, maxValues;

    [SerializeField]
    private AudioManager audioManager;

    private void Start()
    {
        camera.orthographicSize = defaultCamSize;
        if (StaticValues.Size != 0)
        {
            minValues.x = -StaticValues.Size * 5 + (maxCamSize*1.75f);
            minValues.y = -StaticValues.Size * 5 + maxCamSize;
            maxValues.x = StaticValues.Size * 5 - (maxCamSize * 1.75f);
            maxValues.y = StaticValues.Size * 5 - maxCamSize;
        }
        else
        {
            minValues.x = -1000 + maxCamSize * 1.78f;
            minValues.y = -1000 + maxCamSize;
            maxValues.x = 1000 - maxCamSize * 1.78f;
            maxValues.y = 1000 - maxCamSize;
        }

        if (defaultCamSize== 0)
        {
            defaultCamSize = minCamSize;
        }
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        button = GameObject.Find("MoveCameraButton").GetComponent<Image>();
        IsInCameraMode = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ToggleCameraMode();
        }
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
        Vector3 targetPosition = camera.transform.position;
        if(Input.GetMouseButtonDown(0))
        {
            dragOrigin=camera.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - camera.ScreenToWorldPoint(Input.mousePosition);
            targetPosition += difference;
            Vector3 finalPosition = new Vector3(
                Mathf.Clamp(targetPosition.x, minValues.x, maxValues.x),
                Mathf.Clamp(targetPosition.y, minValues.y, maxValues.y),
                camera.transform.position.z
                );
            camera.transform.position = finalPosition;
        }
    }

    private void PanCameraByArrowKeys()
    {
        Vector3 targetPosition = camera.transform.position;
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            targetPosition += new Vector3(-100, 0, 0) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            targetPosition += new Vector3(100, 0, 0) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            targetPosition += new Vector3(0, 100, 0) * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            targetPosition += new Vector3(0, -100, 0) * Time.deltaTime;
        }
		Vector3 finalPosition = new Vector3(
	        Mathf.Clamp(targetPosition.x, minValues.x, maxValues.x),
	        Mathf.Clamp(targetPosition.y, minValues.y, maxValues.y),
	        camera.transform.position.z
	        );
		camera.transform.position = finalPosition;
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
        audioManager.PlayActivateCameraSfx();
    }
}
