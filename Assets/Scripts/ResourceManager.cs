using System.Collections;
using System.Collections.Generic;
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
        
    }
    
    private IEnumerator DecreaseResources()
    {
        yield return new WaitForSeconds(resourceDecreaseInterval);
        FoodAmount -= CitizenAmount;
        if (FoodAmount < 0)
        {
            FoodAmount = 0;
			CitizenAmount -= 1;
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
        FoodAmount += amount;
    }
    private void GameOver() { }
}
