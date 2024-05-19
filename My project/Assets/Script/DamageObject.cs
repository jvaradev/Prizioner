using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Código para objetos que infligen daño
public class DamageObject : MonoBehaviour
{
    [SerializeField] public float damage;

    //Verificamos si el jugador entra en contacto con el objeto y entonces inflinge el daño.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<CombatPlayer>().GetDamage(damage/2, collision.GetContact(0).normal);
            collision.transform.GetComponent<PlayerRespawn>().PlayerDamaged();
        }
    }
}
