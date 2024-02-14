using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawmillScript : MonoBehaviour
{
	[SerializeField]
	public ResourceManager resourceManager;
	[SerializeField]
	private int resourceGain = 5;
	[SerializeField]
	private int cooldown = 5;
	// Start is called before the first frame update
	void Start()
	{
		if (resourceManager == null)
			return;
		StartCoroutine(YieldWood());
	}

	// Update is called once per frame
	void Update()
	{

	}
	private IEnumerator YieldWood()
	{
		resourceManager.IncreaseResourceAmount(resourceGain);
		yield return new WaitForSeconds(cooldown);
		StartCoroutine(YieldWood());
	}
}
