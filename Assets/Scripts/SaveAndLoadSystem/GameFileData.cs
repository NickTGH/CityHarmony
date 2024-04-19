using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFileData
{
    public Dictionary<Vector3Int, PlacementData> placedObjects;
    public List<GameObject> instantiatedGameObjects;
    public List<GameObject> placedHouses;

    public int foodAmount;
    public int peopleAmount;
    public int resourcesAmount;

    public int maxFoodAmount;
    public int maxResourcesAmount;
}
