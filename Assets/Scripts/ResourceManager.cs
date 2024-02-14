using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
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
    [HideInInspector]
    public int MaxCitizenAmount;

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
        StartCoroutine(DecreaseResources());
    }

    // Update is called once per frame
    void Update()
    {
        DisplayStats();
    }
    
    private IEnumerator DecreaseResources()
    {
        yield return new WaitForSeconds(resourceDecreaseInterval);
        FoodAmount -= 5*CitizenAmount;
        if (FoodAmount < 0)
        {
            FoodAmount = 0;
			CitizenAmount -= 1;
            if (CitizenAmount == 0)
            {
                GameOver();
            }
		}
        StartCoroutine(DecreaseResources()  );
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
        FoodAmount += (int)Math.Round(amount * (1+CitizenAmount * 0.5));
    }
	public void IncreaseResourceAmount(int amount)
	{
        Debug.Log("Increased");
		BuildingResourceAmount += (int)Math.Round(amount * (1+CitizenAmount * 0.5));
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
    private void GameOver() 
    {
        //stop time; turn on gameOverScreen, block movement
        Time.timeScale = 0;
        gameOverScreen.SetActive(true);
    }
}
