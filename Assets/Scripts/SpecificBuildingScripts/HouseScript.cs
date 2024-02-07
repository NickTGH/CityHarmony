using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseScript : MonoBehaviour
{
    public ResourceManager _resourceManager;
    [SerializeField]
    private int peopleCapacity = 3;
    private bool activatedEffect = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!activatedEffect && _resourceManager!=null)
        {
			_resourceManager.IncreasePopulation(peopleCapacity);
            activatedEffect = true;
		}
    }
}
