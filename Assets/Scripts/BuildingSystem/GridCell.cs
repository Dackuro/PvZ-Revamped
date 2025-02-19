using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GridCell
{
    Dictionary<Vector3Int, PlacementData> placedPlants = new();

    public void AddPlantAt(Vector3Int gridPosition, Vector2Int plantSize, int ID, int placedPlantIndex)
    {
        List<Vector3Int> positionToOcuppy = CalculatePosition(gridPosition, plantSize);
        PlacementData data = new PlacementData(positionToOcuppy, ID, placedPlantIndex);
        foreach (var pos in positionToOcuppy)
        {
            if (placedPlants.ContainsKey(pos))
            {
                throw new Exception($"Dictionary already contais this cell position {pos}.");
            }
            else
            {
                placedPlants[pos] = data;
            }
        }
    }

    private List<Vector3Int> CalculatePosition(Vector3Int gridPosition, Vector2Int plantSize)
    {
        List<Vector3Int> returnVal = new();
        for (int x = 0; x < plantSize.x; x++)
        {
            for (int y = 0; y < plantSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y));
            }
        }
        return returnVal;
    }

    public bool CanPlacePlantAt(Vector3Int gridPosition, Vector2Int plantSize)
    {
        List<Vector3Int> positionToOccupy = CalculatePosition(gridPosition, plantSize);
        foreach (var pos in positionToOccupy)
        {
            if (placedPlants.ContainsKey(pos))
            {
                return false;
            }           
        }
        return true;
    }

    internal int GetRepresentationIndex(Vector3Int gridPos)
    {
        if (placedPlants.ContainsKey(gridPos) == false)
        {
            return -1;
        }
        return placedPlants[gridPos].PlacedPlantIndex;
    }

    internal void RemovePlantAt(Vector3Int gridPos)
    {
        foreach (var pos in placedPlants[gridPos].occupiedPositions)
        {
            placedPlants.Remove(pos);
        }
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedPositions;
    public int ID { get; private set; }
    public int PlacedPlantIndex { get; private set; }
    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedPlantIndex)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedPlantIndex = placedPlantIndex;
    }   
}
