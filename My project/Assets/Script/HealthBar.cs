using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Código para la barra de vida de la interfaz de usuario
public class HealthBar : MonoBehaviour
{
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    //Cambiar el slider según la vida máxima
    public void changeMaxHealth(float maxHealth)
    {
        slider.maxValue = maxHealth;
    }
    
    //Cambiar el slider según la vida que tenga el jugador
    public void changeActualHealth(float cantHealth)
    {
        slider.value = cantHealth;
    }
    
    //Iniciar la barra de vida con la vida que tiene el jugador
    public void inicialiteActualHealth(float cantHealth)
    {
        changeMaxHealth(cantHealth);
        changeActualHealth(cantHealth);
    }
}
