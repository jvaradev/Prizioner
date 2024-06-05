using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Código para los elementos que disparan los enemigos
public class Bullet : MonoBehaviour
{
    public float velocity;
    public int damage;

    private void Start()
    {
        // Iniciar la corrutina para destruir el objeto después de 5 segundos
        StartCoroutine(DestroyAfterTime(5.0f));
    }

    private void Update()
    {
        //Movimiento de la bala. Vector2.left debido a que el enemigo dispara a la izquierda
        transform.Translate(Time.deltaTime * velocity * Vector2.left);
    }

    //Verificar si la "bala" golpea al jugador para causar daño
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<CombatPlayer>().GetDamage(damage, collision.GetContact(0).normal);
            collision.transform.GetComponent<PlayerRespawn>().PlayerDamaged();
            Destroy(gameObject);
        }
    }

    //Corrutina para destruir la "bala" pasado segundos y no crear muchos clones de "balas"
    private IEnumerator DestroyAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}