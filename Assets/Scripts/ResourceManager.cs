using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
	[SerializeField]
	private ObjectPlacer objectPlacer;
	public int FoodAmount;
	public int BuildingResourceAmount;
	public int CitizenAmount;

	[SerializeField]
	private int defaultFoodAmount = 100;
	[SerializeField]
	private int defaultBuildingResourceAmount = 100;
	[SerializeField]
	private int defaultCitizenAmount = 1;
	[SerializeField]
	private int resourceDecreaseInterval = 1;

	[HideInInspector]
	public int MaxFoodAmount;
	[HideInInspector]
	public int MaxBuildingResourceAmount;

	public List<GameObject> placedHouses;

	public TextMeshProUGUI foodText;
	public TextMeshProUGUI resourceText;
	public TextMeshProUGUI citizenText;
	public GameObject gameOverScreen;

	private void Awake()
	{
		gameOverScreen.SetActive(false);
		Time.timeScale = 1.0f;
	}
	// Start is called before the first frame update
	void Start()
	{
		FoodAmount = defaultFoodAmount;
		BuildingResourceAmount = defaultBuildingResourceAmount;
		CitizenAmount = defaultCitizenAmount;

		placedHouses = objectPlacer.GetHousesList();

		MaxFoodAmount = defaultFoodAmount;
		MaxBuildingResourceAmount = defaultBuildingResourceAmount;
		StartCoroutine(DecreaseResources());
	}

	// Update is called once per frame
	void Update()
	{
		DisplayStats();
		placedHouses = objectPlacer.GetHousesList();
	}

	private IEnumerator DecreaseResources()
	{
		yield return new WaitForSeconds(resourceDecreaseInterval);
		FoodAmount -= 5 * CitizenAmount;
		if (FoodAmount < 0)
		{
			FoodAmount = 0;
			Debug.Log(placedHouses.Count);
			if (placedHouses.Count == 0)
			{
				CitizenAmount -= 1;
			}
			else
			{
				DecreasePopulation();
			}


			if (CitizenAmount == 0)
			{
				GameOver();
			}
		}
		StartCoroutine(DecreaseResources());
	}

	public bool CanAffordStructure(int cost)
	{
		return BuildingResourceAmount >= cost;
	}
	public void DecreaseResourcesAfterPlacement(int cost)
	{
		BuildingResourceAmount -= cost;
	}
	public void IncreaseFoodAmount(int amount)
	{
		int increase = (int)Math.Round(amount * (1 + CitizenAmount * 0.5));
		if (FoodAmount + increase >= MaxFoodAmount)
		{
			FoodAmount = MaxFoodAmount;
			return;
		}
		FoodAmount += increase;
	}
	public void IncreaseResourceAmount(int amount)
	{
		int increase = (int)Math.Round(amount * (1 + CitizenAmount * 0.5));
		if (BuildingResourceAmount + increase >= MaxBuildingResourceAmount)
		{
			BuildingResourceAmount = MaxBuildingResourceAmount;
			return;
		}
		BuildingResourceAmount += increase;
	}
	public void IncreasePopulation(int people)
	{
		CitizenAmount += people;
	}

	public void DisplayStats()
	{
		foodText.text = FoodAmount.ToString();
		resourceText.text = BuildingResourceAmount.ToString();
		citizenText.text = CitizenAmount.ToString();
	}
	private void DecreasePopulation()
	{
		System.Random rnd = new();
		int index = rnd.Next(placedHouses.Count);
		//if (placedHouses[index] == null)
		//{
		//	return;
		//}
		HouseScript houseScript = placedHouses[index].GetComponentInChildren<HouseScript>();
		houseScript.residents -= 1;
		CitizenAmount -= 1;
		if (houseScript.residents == 0)
		{
			objectPlacer.DestroyObject(placedHouses[index]);
		}
	}
	private void GameOver()
	{
		//stop time; turn on gameOverScreen, block movement
		Time.timeScale = 0;
		gameOverScreen.SetActive(true);
	}
}
