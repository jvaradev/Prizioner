using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//Código para contar las tarjetas que recoge en el nivel el jugador
public class CountCard : MonoBehaviour
{
    public static float count;
    private TextMeshProUGUI textMesh;
    [SerializeField] private Image card;

    private void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.enabled = false;
        card.enabled = false;
    }

    //Si el jugador recoge tarjeta se mostrará el icono en la interfaz de usuario
    private void Update()
    {
        if (count > 0)
        {
            card.enabled = true;
        }

        if (count <= 0)
        {
            card.enabled = false;
        }
    }

    //Aumentar conteo de tarjeta
    public void AddCount(float addNum)
    {
        count += addNum;
    }
    
}
