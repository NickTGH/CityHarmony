using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    private List<GameObject> placedHouses = new();
    List<GameObject> placedGameObjects = new();

    public void DestroyObject(GameObject obj)
    {
        //placedGameObjects.Remove(obj);
        placedHouses.Remove(obj);
        Destroy(obj);
    }
    public void DestroyObjects()
    {
		foreach (var objectPlaced in placedGameObjects)
		{
			DestroyImmediate(objectPlaced);
		}
		placedGameObjects.Clear();
	}
    public int PlaceObject(ObjectData item, Vector3 position,ResourceManager resourceManager, ParticleSystem[] effects)
    {
        GameObject newObject = Instantiate(item.Prefab);
        if (newObject.GetComponentInChildren<FieldScript>() != null)
        {
            newObject.GetComponentInChildren<FieldScript>().resourceManager = resourceManager;
        }
        if (newObject.GetComponentInChildren<HouseScript>() != null)
        {
            newObject.GetComponentInChildren<HouseScript>()._resourceManager = resourceManager;
            placedHouses.Add(newObject);
        }
		if (newObject.GetComponentInChildren<SawmillScript>() != null)
		{
			newObject.GetComponentInChildren<SawmillScript>().resourceManager = resourceManager;
		}
		newObject.transform.position = position;
        placedGameObjects.Add(newObject);
        int itemSize = item.Size.x;
        int particleIndex;
        Vector3 particlePosition = position;
        switch (itemSize)
        {
            case 1:
                //1x1
                particleIndex = 1;
                particlePosition += new Vector3(2.5f, 2.5f, 0);
                break;
            case 2:
                //2x2
                particleIndex = 2;
				particlePosition += new Vector3(5f, 5f, 0);
				break; 
            case 4:
                //4x2
                particleIndex = 2;
				particlePosition += new Vector3(10f, 5f, 0);
				break;
            default:
                particleIndex = 0;
                break;
        }
        Instantiate(effects[particleIndex], particlePosition,Quaternion.identity);
        return placedGameObjects.Count - 1;
    }

	public int PlaceObstacle(GameObject prefab, Vector3 position)
	{
		GameObject newObject = Instantiate(prefab);
		newObject.transform.position = position;
		placedGameObjects.Add(newObject);
		return placedGameObjects.Count - 1;
	}

	internal void RemoveObjectAt(int gameObjectIndex, ParticleSystem effect)
    {
        if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null)
        {
            return;
        }
         Vector3 particlePosition = new Vector3(placedGameObjects[gameObjectIndex].transform.position.x + placedGameObjects[gameObjectIndex].GetComponentInChildren<SpriteRenderer>().transform.localPosition.x*5,
											   placedGameObjects[gameObjectIndex].transform.position.y + placedGameObjects[gameObjectIndex].GetComponentInChildren<SpriteRenderer>().transform.localPosition.y*5,
											   placedGameObjects[gameObjectIndex].transform.position.z);
        Instantiate(effect, particlePosition, Quaternion.identity);
        if (placedGameObjects[gameObjectIndex].GetComponentInChildren<HouseScript>() != null)
        {
            DestroyObject(placedGameObjects[gameObjectIndex]);
            placedGameObjects[gameObjectIndex] = null;
        }
        else
        {
			Destroy(placedGameObjects[gameObjectIndex]);
			placedGameObjects[gameObjectIndex] = null;
		}
    }

    public List<GameObject> GetHousesList()
    {
        return placedHouses;
    }
}
