using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

//Código para añadir conteo de tarjeta que recoge el jugador
public class Card : MonoBehaviour
{
    [SerializeField] private CountCard count;
    
    //Si jugador coincide con el collider de la tarjeta, se añade uno y se destruye el objeto de la escena.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            count.AddCount(1f);
            Destroy(gameObject);
        }
    }
}
