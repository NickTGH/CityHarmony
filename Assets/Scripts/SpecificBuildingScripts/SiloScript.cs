using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiloScript : ConstructScript
{
    private ResourceManager _resourceManager;
    private int maxFoodIncrease =25;
    void Start()
    {
        _resourceManager = base.resourceManager;
        if (_resourceManager == null)
            return;
        _resourceManager.IncreaseMaxFood(maxFoodIncrease);
    }

}
