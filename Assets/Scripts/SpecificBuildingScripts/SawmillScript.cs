using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawmillScript : ConstructScript
{
	private ResourceManager _resourceManager;
	[SerializeField]
	private int resourceGain = 2;
	[SerializeField]
	private int cooldown = 5;
	// Start is called before the first frame update
	void Start()
	{
		_resourceManager = base.resourceManager;
		if (_resourceManager == null)
			return;
		StartCoroutine(YieldWood());
	}

	// Update is called once per frame
	void Update()
	{

	}
	private IEnumerator YieldWood()
	{
		_resourceManager.IncreaseResourceAmount(resourceGain);
		yield return new WaitForSeconds(cooldown);
		StartCoroutine(YieldWood());
	}
}
