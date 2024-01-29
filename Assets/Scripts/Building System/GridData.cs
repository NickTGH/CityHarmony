using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new();
   // public MapGenerator mapGenerator;

    public void AddObjectAt(Vector3Int gridPosition,Vector2Int objectSize, int ID, int placedObjectIndex)
    {
        List<Vector3Int> positionToOccupy = CalculatePosition(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex);
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
            {
                throw new Exception($"Dictionary already contains this cell position {pos}");
            }
            placedObjects[pos] = data;
        }
    }

    private List<Vector3Int> CalculatePosition(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnValues = new List<Vector3Int>();
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnValues.Add(gridPosition + new Vector3Int(x,y, 0));
            }
        }
        return returnValues;
    }
    private bool CalculateBuildingSurroundings(Vector3Int gridPosition, Vector2Int buildingSize)
    {
		List<Vector3Int> returnValues = new List<Vector3Int>();
		for (int x = -1; x < buildingSize.x+1; x++)
		{
			for (int y = -1; y < buildingSize.y+1; y++)
			{
				returnValues.Add(gridPosition + new Vector3Int(x, y, 0));
			}
		}
        foreach (var pos in returnValues)
        {
            if (placedObjects.ContainsKey(pos))
            {
				if (placedObjects[pos].ID == 1)
				{
					return true;
				}
			}
        }
        return false;
	}

    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int selectedIndex)
    {
        //float[,] noiseMap = Noise.GenerateNoiseMap(mapGenerator.mapWidth, mapGenerator.mapHeight, mapGenerator.seed,mapGenerator.noiseScale,mapGenerator.octaves, mapGenerator.persistance, mapGenerator.lacunarity, mapGenerator.offset);
        List<Vector3Int> positionToOccupy = CalculatePosition(gridPosition, objectSize);
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
            {
                return false;
            }
            //if (noiseMap[pos.x,pos.y] < 0.23f || noiseMap[pos.x,pos.y] > 0.8f)
            //{
            //    return false;
            //}
        }
        if (selectedIndex != 0)
        {
            return CalculateBuildingSurroundings(gridPosition, objectSize);
		}
        return true;
    }

    internal int GetRepresentationIndex(Vector3Int gridPosition)
    {
        if (placedObjects.ContainsKey(gridPosition) == false)
        {
            return -1;
        }
        return placedObjects[gridPosition].PlacedObjectIndex;
    }

    internal void RemoveObjectAt(Vector3Int gridPosition)
    {
        foreach (var pos in placedObjects[gridPosition].occupiedPositions)
        {
            placedObjects.Remove(pos);
        }
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedPositions;
    public int ID {  get; private set; }
    public int PlacedObjectIndex {  get; private set; }

    public PlacementData(List<Vector3Int> occupiedPositions, int iD, int placedObjectIndex)
    {
        this.occupiedPositions = occupiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
    }
}