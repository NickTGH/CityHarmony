using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;

    private Vector2 lastPosition;

    [SerializeField]
    private LayerMask placementLayermask;

    public Vector2 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);   
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, placementLayermask);

        if (hit.collider != null)
        {
            lastPosition = hit.point;
        }

        return lastPosition;
    }
}
