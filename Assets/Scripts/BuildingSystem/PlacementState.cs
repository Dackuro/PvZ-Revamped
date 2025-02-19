using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementState : IBuildingState
{
    [Header("Audio")]
    AudioSource audioSource = GameObject.Find("PvZ Sounds").GetComponent<AudioSource>();

    [Header("Scripts")]
    private InputManager inputManager = GameObject.FindAnyObjectByType<InputManager>();
    private int selectedPlantIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem preview;
    PlantsDatabase database;
    GridCell gridCell;
    ObjectPlacer objectPlacer;

    public PlacementState(int ID, Grid grid, PreviewSystem preview, PlantsDatabase database, GridCell gridCell, ObjectPlacer objectPlacer)
    {
        this.ID = ID;
        this.grid = grid;
        this.preview = preview;
        this.database = database;
        this.gridCell = gridCell;
        this.objectPlacer = objectPlacer;

        selectedPlantIndex = database.plantsData.FindIndex(data => data.ID == ID);
        if (selectedPlantIndex > -1)
        {
            preview.StartShowingPlacementPreview(database.plantsData[selectedPlantIndex].Prefab, database.plantsData[selectedPlantIndex].Size);
        }
        else
            throw new System.Exception($"No plant with ID {ID}");
    }

    public void EndState()
    {
        preview.StopShowingPreview();
    }
    public void OnAction(Vector3Int gridPos, AudioClip errorClip)
    {
        bool placementValidity = CheckPlacementValidity(gridPos, selectedPlantIndex);
        if (placementValidity == false)
        {
            audioSource.PlayOneShot(errorClip);
            return;
        }
        if (inputManager.hit.collider != null && inputManager.hit.transform.CompareTag("Lawn"))
        {
            int index = objectPlacer.PlaceObject(database.plantsData[selectedPlantIndex].Prefab, grid.CellToWorld(gridPos), gridPos);

            GridCell selectedData = gridCell;

            selectedData.AddPlantAt(gridPos, database.plantsData[selectedPlantIndex].Size, database.plantsData[selectedPlantIndex].ID, index);

            preview.UpdatePosition(grid.CellToWorld(gridPos), false);
        }
    }

    private bool CheckPlacementValidity(Vector3Int gridPos, int selectedPlantIndex)
    {
        GridCell selectedData = gridCell;
        return selectedData.CanPlacePlantAt(gridPos, database.plantsData[selectedPlantIndex].Size);
    }

    public void UpdateState(Vector3Int gridPos)
    {
        bool placementValidity = CheckPlacementValidity(gridPos, selectedPlantIndex);
        preview.UpdatePosition(grid.CellToWorld(gridPos), placementValidity);
    }
}
