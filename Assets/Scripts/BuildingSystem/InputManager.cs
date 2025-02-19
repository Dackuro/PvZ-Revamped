using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private Vector3 lastPos;
    [SerializeField] private LayerMask placementLayerMask;
    public RaycastHit hit;

    public event Action OnClicked, OnExit;

    public bool onLawn;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClicked?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnExit?.Invoke();
        }
    }

    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cam.nearClipPlane;
        Ray ray = cam.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit, 100, placementLayerMask))
        {
            lastPos = hit.point;
            onLawn = true;
        }
        else
        {
            onLawn = false;
        }
        return lastPos;
    }
}
