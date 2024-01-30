using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> placedGameObjects = new();

    public int PlaceObject(GameObject prefab, Vector3 position,ResourceManager resourceManager)
    {
        GameObject newObject = Instantiate(prefab);
        if (newObject.GetComponentInChildren<FieldScript>() != null)
        {
            newObject.GetComponentInChildren<FieldScript>().resourceManager = resourceManager;
        }
        newObject.transform.position = position;
        placedGameObjects.Add(newObject);
        return placedGameObjects.Count - 1;
    }

	public int PlaceObstacle(GameObject prefab, Vector3 position)
	{
		GameObject newObject = Instantiate(prefab);
		newObject.transform.position = position;
		placedGameObjects.Add(newObject);
		return placedGameObjects.Count - 1;
	}

	internal void RemoveObjectAt(int gameObjectIndex)
    {
        if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null)
        {
            return;
        }
        Destroy(placedGameObjects[gameObjectIndex]);
        placedGameObjects[gameObjectIndex] = null;
    }
}
