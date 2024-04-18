using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    public void changeMaxHealth(float maxHealth)
    {
        slider.maxValue = maxHealth;
    }
    
    public void changeActualHealth(float cantHealth)
    {
        slider.value = cantHealth;
    }
    
    public void inicialiteActualHealth(float cantHealth)
    {
        changeMaxHealth(cantHealth);
        changeActualHealth(cantHealth);
    }
}
