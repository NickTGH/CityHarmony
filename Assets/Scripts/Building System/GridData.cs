using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridData
{
    public Dictionary<Vector3Int, PlacementData> placedObjects = new();

    public void AddObjectAt(Vector3Int gridPosition,Vector2Int objectSize, int ID, int placedObjectIndex)
    {
        List<Vector3Int> positionToOccupy = CalculatePosition(gridPosition, objectSize);
        PlacementData data = new PlacementData(positionToOccupy, ID, placedObjectIndex);
        foreach (var pos in positionToOccupy)
        {
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
                returnValues.Add(gridPosition * 5 + new Vector3Int(x,y, 0)*5);
            }
        }
        return returnValues;
    }
    private bool CalculateBuildingSurroundings(Vector3Int gridPosition, Vector2Int buildingSize)
    {
        List<Vector3Int> returnValues = new List<Vector3Int>();
        for (int x = -1; x < buildingSize.x + 1; x++)
        {
            for (int y = -1; y < buildingSize.y + 1; y++)
            {
                returnValues.Add((gridPosition + new Vector3Int(x, y, 0)) * 5);
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

    private bool CheckForTrees(Vector3Int gridPosition, Vector2Int buildingSize)
    {
		List<Vector3Int> returnValues = new List<Vector3Int>();
		for (int x = -5; x < buildingSize.x + 5; x++)
		{
			for (int y = -5; y < buildingSize.y + 5; y++)
			{
				returnValues.Add((gridPosition + new Vector3Int(x, y, 0)) * 5);
			}
		}
		foreach (var pos in returnValues)
		{
			if (placedObjects.ContainsKey(pos))
			{
				if (placedObjects[pos].ID == 4)
				{
					return true;
				}
			}
		}
		return false;
	}

    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int selectedIndex, MapGenerator mapGenerator)
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapGenerator.mapWidth, mapGenerator.mapHeight, mapGenerator.seed,mapGenerator.noiseScale,
                                                   mapGenerator.octaves, mapGenerator.persistance, mapGenerator.lacunarity, mapGenerator.offset);
        float[,] lightMap = mapGenerator.ReturnLightLevels();
        List<Vector3Int> positionToOccupy = CalculatePosition(gridPosition, objectSize);
        foreach (var pos in positionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
            {
                return false;
            }
			Vector2Int noiseMapValues = ConvertToNoiseMapValues(pos.x, pos.y, mapGenerator.mapWidth);
			if (noiseMap[noiseMapValues.x,noiseMapValues.y] < 0.33f || noiseMap[noiseMapValues.x,noiseMapValues.y] > 0.78f)
            {
                return false;
            }
            if (lightMap[noiseMapValues.x,noiseMapValues.y] < 10f || lightMap[noiseMapValues.x,noiseMapValues.y] > 90f)
            {
                return false;
            }
        }
        if (selectedIndex != 0)
        {
            if (selectedIndex == 4)
            {
                return CalculateBuildingSurroundings(gridPosition, objectSize) && CheckForTrees(gridPosition, objectSize);
            }
            return CalculateBuildingSurroundings(gridPosition, objectSize);
		}
        return true;
    }
    
    //--------------------------------------------
    //For trees only!!!
	public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int selectedIndex, float[,]noiseMap,int mapWidth)
	{
		List<Vector3Int> positionToOccupy = CalculatePosition(gridPosition, objectSize);
		foreach (var pos in positionToOccupy)
		{
			if (placedObjects.ContainsKey(pos))
			{
				return false;
			}
			Vector2Int noiseMapValues = ConvertToNoiseMapValues(pos.x, pos.y,mapWidth);
			if (noiseMap[noiseMapValues.x, noiseMapValues.y] < 0.3f || noiseMap[noiseMapValues.x, noiseMapValues.y] > 0.8f)
			{
				return false;
			}
		}
		if (selectedIndex != 0 && selectedIndex != 4)
		{
			return CalculateBuildingSurroundings(gridPosition, objectSize);
		}
		return true;
	}
    //---------------------------------------------

	private Vector2Int ConvertToNoiseMapValues(int x, int y, int mapWidth)
    {
        float oldX = float.Parse(x.ToString());
        float oldY = float.Parse(y.ToString());
        float oldRange = (mapWidth-1)*10;
        float oldMin = -oldRange / 2;
        float newX = ((oldX - oldMin)/oldRange) * (mapWidth - 1);
        float newY = ((oldY - oldMin)/oldRange) * (mapWidth - 1);
        newX = Mathf.Floor(Math.Abs(newX));
        newY = Mathf.Floor(Mathf.Abs(newY));
        return new Vector2Int((int)newX, (int)newY);
    }
    internal int GetRepresentationIndex(Vector3Int gridPosition)
    {
        if (placedObjects.ContainsKey(gridPosition*5) == false)
        {
            return -1;
        }
        return placedObjects[gridPosition*5].PlacedObjectIndex;
    }

    internal void RemoveObjectAt(Vector3Int gridPosition)
    {
        foreach (var pos in placedObjects[gridPosition*5].occupiedPositions)
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