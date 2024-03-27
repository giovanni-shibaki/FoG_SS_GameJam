using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Slider resourceBar;

    private void Start()
    {
        resourceBar.maxValue = 100;
        resourceBar.minValue = 0;
        resourceBar.value = 100;
    }

    public void TakeDamage(int damage)
    {
        resourceBar.value -= damage;
    }
}
