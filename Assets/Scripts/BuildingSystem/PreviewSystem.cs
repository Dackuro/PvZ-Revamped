using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PreviewSystem : MonoBehaviour // Sistema de preview. Permite previsualizar las plantas antes de colocarlas. Creado siguiendo una guía de Sunny Valley Studio.
{
    [Header("Objeto")]
    public GameObject cellIndicator;
    private GameObject previewObject;

    [Header("Variables")]
    [SerializeField] private float previewYOffset = 0.06f;  
    [SerializeField] private Material previewMaterialsPrefab;
    private Material previewMaterialInstance;
    private Renderer cellIndicatorRenderer;

    private void Start()
    {
        previewMaterialInstance = new Material(previewMaterialsPrefab);
        cellIndicator.SetActive(false);
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartShowingPlacementPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);
        Plants plants = previewObject.GetComponent<Plants>();
        previewObject.GetComponent<Collider>().enabled = false;

        if (plants != null)
        {
            plants.isActive = false;
        }  

        PreparePreview(previewObject);
        cellIndicator.SetActive(true);
    }
    private void PreparePreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material[] materials = renderer.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = previewMaterialInstance;
            }
            renderer.materials = materials;
        }
    }

    public void StopShowingPreview()
    {
        cellIndicator.SetActive(false);
        if (previewObject != null)
        {
            Destroy(previewObject);
        }
    }

    public void UpdatePosition(Vector3 position, bool validity)
    {
        if (previewObject != null)
        {
            MovePreview(position);
            ApplyFeedbackToPreview(validity);
        }
        
        MoveCursor(position);
        ApplyFeedbackToCursor(validity);
    }
    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = new Vector3(position.x+2.5f, position.y+previewYOffset, position.z+2.5f);
    }

    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position;
    }

    private void ApplyFeedbackToPreview(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        previewMaterialInstance.color = c;
        c.a = 0.5f;   
    }
    
    private void ApplyFeedbackToCursor(bool validity)
    {
        Color c = validity ? Color.white : Color.red;
        cellIndicatorRenderer.material.color = c;
        c.a = 0.5f;
    }

    internal void StartShowingRemovePreview()
    {
        cellIndicator.SetActive(true);
        ApplyFeedbackToCursor(false);
    }
}
