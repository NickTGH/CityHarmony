using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LampScript : MonoBehaviour
{
    [SerializeField]
    private int lightRadius;

    [SerializeField]
    private DayNightCycle dayNightCycle;
    [SerializeField]
    private MapGenerator mapGenerator;
    [SerializeField]
    private Light2D light;
    [SerializeField]
    private Grid grid;

    bool activated;
    // Start is called before the first frame update
    void Start()
    {
        dayNightCycle = GameObject.Find("Sun").GetComponent<DayNightCycle>();
        mapGenerator = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        light = GetComponentInChildren<Light2D>();
        activated = false;
        light.pointLightOuterRadius = lightRadius+4;
        light.pointLightInnerRadius = lightRadius * 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        float lightLevel = dayNightCycle.GetLightLevel();
        if (lightLevel < 10 || lightLevel > 90)
        {
            if (!activated)
            {
                LightSurroundings();
            }
            activated = true;
        }
        else
        {
            ResetLighting();
            activated = false;
        }
    }

    private void LightSurroundings()
    {
        float[,] lightMap = mapGenerator.ReturnLightLevels();
        Vector3Int gridPosition = grid.WorldToCell(light.transform.position);
        for (int y = -lightRadius; y < lightRadius; y++)
        {
            for(int x = -lightRadius; x < lightRadius; x++)
            {
                var pos = gridPosition*5 + new Vector3(x, y, 0f);
                if (pos.x % 5 != 0 || pos.y %5 != 0)
                {
                    continue;
                }
                Vector2Int normalizedValues = ConvertToNoiseMapValues((int)pos.x, (int)pos.y, mapGenerator.mapWidth);
                if ((pos - light.transform.position).magnitude-3 >= lightRadius)
                {
                    continue;
                }
                if (dayNightCycle.GetLightLevel() < 10f)
                {
                    if (lightMap[normalizedValues.x, normalizedValues.y] >= dayNightCycle.GetLightLevel() + 20f)
                    {
                        continue;
                    }
                    lightMap[normalizedValues.x, normalizedValues.y] += 40f;
                }
                else if(dayNightCycle.GetLightLevel() > 90f)
                {
                    if (lightMap[normalizedValues.x, normalizedValues.y] <= dayNightCycle.GetLightLevel() - 20f)
                    {
                        continue;
                    }
                    lightMap[normalizedValues.x, normalizedValues.y] -= 40f;
                }
            }
        }
    }
    private void ResetLighting()
    {
        float[,] lightMap = mapGenerator.ReturnLightLevels();
        for (int x = 0; x < lightMap.GetLength(1); x++)
        {
            for (int y = 0; y < lightMap.GetLength(0); y++)
            {
                lightMap[x,y] = dayNightCycle.GetLightLevel();
            }
        }
    }
    private Vector2Int ConvertToNoiseMapValues(int x, int y, int mapWidth)
    {
        float oldX = float.Parse(x.ToString());
        float oldY = float.Parse(y.ToString());
        float oldRange = (mapWidth - 1) * 10;
        float oldMin = -oldRange / 2;
        float newX = ((oldX - oldMin) / oldRange) * (mapWidth - 1);
        float newY = ((oldY - oldMin) / oldRange) * (mapWidth - 1);
        newX = Mathf.Floor(Mathf.Abs(newX));
        newY = Mathf.Floor(Mathf.Abs(newY));
        return new Vector2Int((int)newX, (int)newY);
    }
    private void OnDestroy()
    {
        ResetLighting();
    }
}
