using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Código para crear CheckPoint de jugador.
public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerRespawn>().ReachedCheckPoint(transform.position.x,transform.position.y);
        }
    }
}