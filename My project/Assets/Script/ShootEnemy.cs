using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEnemy : MonoBehaviour
{
    public Transform controllerShot; // Transform que define la posición desde donde se disparan los proyectiles
    public float distancePlayer; // Distancia a la que se detecta al jugador
    public LayerMask layerPlayer; // Capa del jugador para detectar colisiones
    public bool playerInRange; // Indica si el jugador está en rango de disparo
    public GameObject bullet; // Prefab de la bala que se dispara
    public float timeShots; // Tiempo entre disparos
    public float timeLastShot; // Tiempo del último disparo
    public float timeWaiting; // Tiempo de espera antes de disparar
    public Animator animator; // Referencia al componente Animator

    // Método que se llama en cada frame
    private void Update()
    {
        // Realiza un raycast para detectar si el jugador está en rango
        playerInRange = Physics2D.Raycast(controllerShot.position, -transform.right, distancePlayer, layerPlayer);
        if (playerInRange)
        {
            // Si el jugador está en rango y ha pasado suficiente tiempo desde el último disparo
            if (Time.time > timeShots + timeLastShot)
            {
                // Actualiza el tiempo del último disparo
                timeLastShot = Time.time;
                // Activa la animación de disparo
                animator.SetTrigger("Shoot");
                // Invoca el método Shoot después de un tiempo de espera
                Invoke(nameof(Shoot), timeWaiting);
            }
        }
    }

    // Método para instanciar la bala
    private void Shoot()
    {
        // Instancia una bala en la posición y rotación del controllerShot
        Instantiate(bullet, controllerShot.position, controllerShot.rotation);
    }

    // Método para dibujar líneas de depuración en el editor
    private void OnDrawGizmos()
    {
        // Dibuja una línea roja desde el controllerShot para representar el rango de detección del jugador
        Gizmos.color = Color.red;
        Gizmos.DrawLine(controllerShot.position, controllerShot.position + transform.right * distancePlayer * -1);
    }
}
