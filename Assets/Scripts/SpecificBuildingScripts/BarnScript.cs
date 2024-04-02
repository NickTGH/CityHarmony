using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarnScript : ConstructScript
{
    private ResourceManager _resourceManager;
    private int maxResourceIncrease = 50;
    void Start()
    {
        _resourceManager = base.resourceManager;
        if (_resourceManager == null)
            return;
        _resourceManager.IncreaseMaxFood(maxResourceIncrease);
    }

}
