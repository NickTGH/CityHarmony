using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldScript : ConstructScript
{
    private ResourceManager _resourceManager;
    [SerializeField]
    private int cropGain = 3;
    [SerializeField]
    private int cooldown = 5;
    // Start is called before the first frame update
    void Start()
    {
        _resourceManager = base.resourceManager;
        if (resourceManager == null)
            return;
        StartCoroutine(YieldCrops());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator YieldCrops()
    {
        _resourceManager.IncreaseFoodAmount(cropGain);
        yield return new WaitForSeconds(cooldown);
        StartCoroutine(YieldCrops());
    }
}
