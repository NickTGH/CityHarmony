using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorldGenerationInput : MonoBehaviour
{
	[SerializeField]
	private string seed;
	[SerializeField]
	private Toggle smallToggle;
	[SerializeField]
	private Toggle mediumToggle;
	[SerializeField]
	private Toggle largeToggle;
    public void SetSeed(string s)
    {
        seed = s;
    }
    public void SelectSmallMap()
	{
		if (smallToggle.isOn == false)
			return;
		mediumToggle.isOn = false;
		largeToggle.isOn = false;
        StaticValues.Size = 100;
	}

	public void SelectMediumMap()
	{
		if (mediumToggle.isOn == false) 
			return;
		smallToggle.isOn = false;
		largeToggle.isOn = false;
		StaticValues.Size = 200;
	}

	public void SelectLargeMap()
	{
		if (largeToggle.isOn == false)
			return;
		smallToggle.isOn = false;
		mediumToggle.isOn = false;
        StaticValues.Size = 300;
	}

    public void GenerateWorld()
	{
		if (seed != null && seed != string.Empty)
		{
			if (int.TryParse(seed, out int res) == true)
			{
				StaticValues.Seed = int.Parse(seed);
			}
			else
			{
				StaticValues.Seed = Random.Range(int.MinValue, int.MaxValue);
			}
		}
		else
		{
			StaticValues.Seed = Random.Range(int.MinValue,int.MaxValue);
		}
	}
}
