using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RemovingSystem : IBuildingState
{
    public AudioSource audioSource = GameObject.Find("PvZ Sounds").GetComponent<AudioSource>();

    [Header("Scripts")]
    Grid grid;
    PreviewSystem preview;
    GridCell gridCell;
    ObjectPlacer objectPlacer;

    [Header("Variables")]
    int selectedPlantIndex;

    public RemovingSystem(Grid grid, PreviewSystem preview, GridCell gridCell, ObjectPlacer objectPlacer)
    {
        this.grid = grid;
        this.preview = preview;
        this.gridCell = gridCell;
        this.objectPlacer = objectPlacer;


        preview.StartShowingRemovePreview();
    }

    public void EndState()
    {
        preview.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPos, AudioClip errorClip)
    {
        GridCell selectedData = null;
        if (gridCell.CanPlacePlantAt(gridPos, Vector2Int.one) == false)
        {
            selectedData = gridCell;
        }

        if (selectedData != null)
        {
            selectedPlantIndex = selectedData.GetRepresentationIndex(gridPos);
            if (selectedPlantIndex == -1)
            {
                return;
            }

            selectedData.RemovePlantAt(gridPos);
            objectPlacer.RemoveObject(selectedPlantIndex);
        }
        else
        {
            audioSource.PlayOneShot(errorClip);
        }
        Vector3 cellPos = grid.CellToWorld(gridPos);
        preview.UpdatePosition(cellPos, CheckIfSelectionIsValid(gridPos));
    }

    public bool CheckIfSelectionIsValid(Vector3Int gridPos)
    {
        return !(gridCell.CanPlacePlantAt(gridPos, Vector2Int.one));
    }

    public void UpdateState(Vector3Int gridPos)
    {
        bool validity = CheckIfSelectionIsValid(gridPos);
        preview.UpdatePosition(grid.CellToWorld(gridPos), validity);
    }
}

