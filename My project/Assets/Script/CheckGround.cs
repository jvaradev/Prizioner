using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Código para verificar que el jugador está en el suelo. Sirve para evitar salto infinito
public class CheckGround : MonoBehaviour
{
    public static bool isGround;

    //Si el Collider de CheckGround está dentro de otro, significa que el jugador está sobre el suelo.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("TileMap"))
        {
            isGround = true;
        }
    }

    //Si el Collider de CheckGround no está dentro de otro, significa que el jugador no está sobre el suelo.
    private void OnTriggerExit2D(Collider2D collision)
    {
        isGround = false;
    }
    
}
