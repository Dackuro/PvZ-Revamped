using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementSystem : MonoBehaviour // Sistema principal de colocación y eliminación de plantas. Creado siguiendo una guía de Sunny Valley Studio.
{
    [Header("Scripts")]
    [SerializeField] private InputManager inputManager;
    [SerializeField] private CostManager costManager;
    [SerializeField] private PlantsDatabase database;
    [SerializeField] private PreviewSystem preview;
    [SerializeField] private ObjectPlacer objectPlacer;
    public GridCell plantData;
    IBuildingState buildingState;

    [Header("Objetos")]
    [SerializeField] private Grid grid;
    [SerializeField] private GameObject gridVisualization;
    [SerializeField] private Color canPlaceColor, cantPlaceColor;

    [SerializeField] private AudioClip errorClip;

    public bool onLawn;
    private Vector3Int lastDetectedPosition = Vector3Int.zero;  

    private void Start()
    {
        StopPlacement();
        plantData = new GridCell();
    }

    public void StartPlacement(int ID, int cost) // Comienza el posicionamiento de la planta, gastando el dinero necesario y empezando todo el proceso.
    {
        StopPlacement();

        if (costManager.sunAmount - cost < 0)
        {
            Debug.Log("No hay suficiente dinero para colocar este objeto.");
            return;
        }

        costManager.SubstractSun(cost);
        gridVisualization.SetActive(true);
        buildingState = new PlacementState(ID, grid, preview, database, plantData, objectPlacer);
        inputManager.OnClicked += PlacePlant;
        inputManager.OnExit += StopPlacement;
    }

    public void StartRemoving() // Comienza la destrucción de la planta y empezando todo el proceso.
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new RemovingSystem(grid, preview, plantData, objectPlacer);
        inputManager.OnClicked += PlacePlant;
        inputManager.OnExit += StopPlacement;
    }
    private void PlacePlant()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }

        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPos = grid.WorldToCell(mousePosition);

        if (buildingState != null)
        {
            buildingState.OnAction(gridPos, errorClip);
        }
        StopPlacement();
    }

    private void StopPlacement() // Para todo proceso activo.
    {
        if (buildingState == null)
        {
            return;
        }
        gridVisualization.SetActive(false);
        buildingState.EndState();
        inputManager.OnClicked -= PlacePlant;
        inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }

    private void Update()
    {
        onLawn = inputManager.onLawn;
        if (buildingState == null)
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPos = grid.WorldToCell(mousePosition);

        if (lastDetectedPosition != gridPos)
        {
            buildingState.UpdateState(gridPos);
            lastDetectedPosition = gridPos;
        }        

        if (inputManager.hit.transform != null)
        {
            preview.cellIndicator.SetActive(true);
            preview.cellIndicator.transform.position = inputManager.hit.transform.position;
        }
        else
        {
            preview.cellIndicator.SetActive(false);
        }
    }
}
