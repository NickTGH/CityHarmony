using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MapGenerator : MonoBehaviour
{
	public enum DrawMode { NoiseMap, ColorMap };
	public DrawMode drawMode;

	public int mapWidth;
	public int mapHeight;
	public float noiseScale;

	public int octaves;
	[Range(0, 1)]
	public float persistance;
	public float lacunarity;

	public int seed;
	public Vector2 offset;

	public bool autoUpdate;
	public bool spawnTrees;
	public bool useWeather;

	public TerrainType[] regions;


	private float[,] lightLevels;

	public DayNightCycle lightCycle;
	public PlacementSystem placementSystem;
	public ObjectPlacer objectPlacer;

	public GameObject treeObstacle;
	public GameObject snowParticles;
	public GameObject rainParticles;

	System.Random random = new System.Random();

	private void Awake()
	{
		if (StaticValues.Seed != 0)
		{ seed = StaticValues.Seed; }
		if (StaticValues.Size != 0)
		{
			mapHeight = StaticValues.Size;
			mapWidth = StaticValues.Size;
		}
		lightCycle = GameObject.Find("Sun").GetComponent<DayNightCycle>();
	}

	private void Start()
	{
		lightLevels = new float[mapWidth, mapHeight];
		GenerateMap();
	}
    private void FixedUpdate()
    {
        UpdateLightLevels();
    }

	public void UpdateLightLevels()
	{
		for (int y = 0; y < mapHeight; y++)
		{
			for (int x = 0; x < mapWidth; x++)
			{
				//Only set light level if the light difference is small enough
				float currentLightLevel = lightCycle.GetLightLevel();
				if (currentLightLevel < 50)
				{
                    if (lightLevels[x, y] - 5 < currentLightLevel)
                    {
                        lightLevels[x, y] = lightCycle.GetLightLevel();
                    }
                }
				else
				{
                    if (lightLevels[x, y] + 5 > currentLightLevel)
                    {
                        lightLevels[x, y] = lightCycle.GetLightLevel();
						continue;
                    }
                }
            }
		}
	}
    public void GenerateMap()
	{
		objectPlacer.DestroyObjects();

		float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);
		Color[] colorMap = new Color[mapHeight * mapWidth];
		for (int y = 0; y < mapHeight; y++)
		{
			for (int x = 0; x < mapWidth; x++)
			{
				float currentHeight = noiseMap[x, y];
				lightLevels[x, y] = lightCycle.GetLightLevel();
				for (int i = 0; i < regions.Length; i++)
				{

					if (currentHeight <= regions[i].height)
					{
						if (i == 0)
						{
							colorMap[y * mapWidth + x] = regions[i].color;
						}
						else
						{
                            SpawnWeather(noiseMap, i, x, y);
                            SpawnTrees(noiseMap, i, x, y);
							colorMap[y * mapWidth + x] = Color.Lerp(regions[i - 1].color, regions[i].color, currentHeight);
						}
						break;
					}
				}
			}
		}
		
		MapDisplay display = FindObjectOfType<MapDisplay>();
		if (drawMode == DrawMode.NoiseMap)
		{
			display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
		}
		else if (drawMode == DrawMode.ColorMap)
		{
			display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
		}
	}

	private void SpawnTrees(float[,] noiseMap, int i, int x, int y)
	{
		if (regions[i].name == "HighLand" && spawnTrees)
		{
			int chanceOfTreeSpawn = random.Next(1001);
			if (chanceOfTreeSpawn > 700 && y <= mapWidth - 2)
			{
				//Add object to placedObjects
				Vector3Int gridPosition = placementSystem.grid.WorldToCell(new Vector3(x - mapWidth * 0.5f, y - mapHeight * 0.5f, 0) * 10);
				bool validity = placementSystem.structureData.CanPlaceObjectAt(gridPosition, new Vector2Int(2, 4), 4, noiseMap, mapWidth);
				if (validity)
				{
					int index = objectPlacer.PlaceObstacle(treeObstacle, placementSystem.grid.CellToWorld(gridPosition));
					placementSystem.structureData.AddObjectAt(gridPosition, new Vector2Int(2, 4), 4, index);
				}
			}
		}
	}
    private void SpawnWeather(float[,] noiseMap, int i, int x, int y)
    {
        if (regions[i].name == "Snow" && useWeather)
        {
            if (y <= mapWidth - 2)
            {
                //Add object to placedObjects
                Vector3Int gridPosition = placementSystem.grid.WorldToCell(new Vector3(x - mapWidth * 0.5f, y - mapHeight * 0.5f, 0) * 10);
                int index = objectPlacer.PlaceObstacle(snowParticles, placementSystem.grid.CellToWorld(new Vector3Int(gridPosition.x,gridPosition.y+2,gridPosition.z)));
            }
        }
		else if (useWeather)
		{
			if (y <= mapWidth - 2)
			{
				//Add object to placedObjects
				Vector3Int gridPosition = placementSystem.grid.WorldToCell(new Vector3(x - mapWidth * 0.5f, y - mapHeight * 0.5f, 0) * 10);
                int index = objectPlacer.PlaceObstacle(rainParticles, placementSystem.grid.CellToWorld(new Vector3Int(gridPosition.x,gridPosition.y+2,gridPosition.z)));
			}
		}
	}

    public float[,] ReturnLightLevels()
	{
		return lightLevels;
	}
	private void OnValidate()
	{
		if (mapWidth < 1)
		{
			mapWidth = 1;
		}
		if (mapHeight < 1)
		{
			mapHeight = 1;
		}
		if (lacunarity < 1)
		{
			lacunarity = 1;
		}
		if (octaves < 0)
		{
			octaves = 0;
		}
	}

	[System.Serializable]
	public struct TerrainType
	{
		public string name;
		public float height;
		public Color color;
	}
}