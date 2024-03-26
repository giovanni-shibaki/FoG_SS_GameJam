using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Type
{
    Iron,
    Gold,
    Water
}

public class ResourceBarManager : MonoBehaviour
{
    public Type type;
    public Slider resourceBar;
    private PlayerResources playerResources;

    private void Start()
    {
        resourceBar.maxValue = 100;
        resourceBar.value = 0;
        playerResources = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerResources>();
    }

    private void Update()
    {
        if(type == Type.Iron)
        {
            resourceBar.value = playerResources.getIronCount();
        }
        else if(type == Type.Gold)
        {
            resourceBar.value = playerResources.getGoldCount();
        }
        else
        {
            resourceBar.value = playerResources.getWaterCount();
        }
    }

    public void SetResourceAmount(int amount)
    {
        resourceBar.value = amount;
    }
}
