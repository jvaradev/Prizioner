using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class Tobacco : MonoBehaviour
{
    [SerializeField] private CountCigarrete count; // Referencia al script CountCigarrete para contar cigarrillos

    // Método que se llama cuando otro objeto entra en el trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Comprueba si el objeto que entró en el trigger tiene la etiqueta "Player"
        if (other.CompareTag("Player"))
        {
            // Incrementa el contador de cigarrillos en 0.5
            count.AddCount(0.5f);
            // Destruye el objeto Tobacco
            Destroy(gameObject);
        }
    }
}