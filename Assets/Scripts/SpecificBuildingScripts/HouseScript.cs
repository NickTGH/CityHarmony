using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HouseScript : MonoBehaviour
{
    public ResourceManager _resourceManager;
    [SerializeField]
    private Light2D houseLight;

    public int residents = 3;
    private bool activatedEffect = false;
    // Start is called before the first frame update
    void Start()
    {
        houseLight.intensity = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!activatedEffect && _resourceManager!=null)
        {
			_resourceManager.IncreasePopulation(residents);
            activatedEffect = true;
		}
    }

	private void OnDestroy()
	{
        if(_resourceManager!=null)
        {
			_resourceManager.IncreasePopulation(residents * -1);
		}
	}
}
