using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = System.Object;
using Random = UnityEngine.Random;

public class PatrullaEnemigo : MonoBehaviour
{
    [SerializeField] private float health; // Vida del enemigo
    [SerializeField] private float velocidad; // Velocidad de movimiento del enemigo
    [SerializeField] private Transform[] puntos; // Puntos de patrullaje
    [SerializeField] private float distanciaMinima; // Distancia mínima para cambiar de punto de patrullaje
    [SerializeField] public float damage; // Daño que inflige el enemigo
    private int numAlt; // Índice del punto de patrullaje actual
    private BoxCollider2D bc2D; // Referencia al BoxCollider2D del enemigo
    private SpriteRenderer sr; // Referencia al SpriteRenderer del enemigo
    private Animator animator; // Referencia al Animator del enemigo
    private bool isDead = false; // Variable para controlar si el enemigo está muerto
    private bool isHurt = false; // Variable para controlar si el enemigo está herido
    
    private void Start()
    {
        // Inicializa las variables y componentes necesarios
        numAlt = Random.Range(0, puntos.Length); // Selecciona un punto de patrullaje aleatorio
        animator = GetComponent<Animator>(); // Obtiene el Animator del enemigo
        sr = GetComponent<SpriteRenderer>(); // Obtiene el SpriteRenderer del enemigo
        bc2D = GetComponent<BoxCollider2D>(); // Obtiene el BoxCollider2D del enemigo
        Girar(); // Ajusta la dirección del sprite
    }

    private void Update()
    {
        // Actualiza el estado del enemigo cada frame
        if (health <= 0)
        {
            StartCoroutine(Dead()); // Inicia la corrutina de muerte si la vida es menor o igual a 0
        }
        if (!isDead && !isHurt) // Solo realiza el movimiento si el enemigo no está muerto o herido
        {
            animator.SetBool("Walk", true); // Activa la animación de caminar
            transform.position = Vector2.MoveTowards(transform.position, puntos[numAlt].position, velocidad * Time.deltaTime); // Mueve al enemigo hacia el punto de patrullaje actual
            if (Vector2.Distance(transform.position, puntos[numAlt].position) < distanciaMinima)
            {
                // Si el enemigo alcanza el punto de patrullaje actual, selecciona un nuevo punto y gira el sprite
                numAlt = Random.Range(0, puntos.Length);
                Girar();
            }
        }
    }

    // Ajusta la dirección del sprite según la posición del próximo punto de patrullaje
    private void Girar()
    {
        if (transform.position.x < puntos[numAlt].position.x)
        {
            sr.flipX = false; // No voltea el sprite
        }
        else
        {
            sr.flipX = true; // Voltea el sprite horizontalmente
        }
    }

    // Método para recibir daño
    public void GetDamage(float damage)
    {
        health -= damage; // Reduce la vida del enemigo
        StartCoroutine(Hurt()); // Inicia la corrutina de herida
        if (health <= 0)
        {
            StartCoroutine(Dead()); // Inicia la corrutina de muerte si la vida es menor o igual a 0
        }
    }

    // Corrutina para manejar la muerte del enemigo
    private IEnumerator Dead()
    {
        isDead = true; // Marca al enemigo como muerto para detener su movimiento
        animator.SetBool("Dead", true); // Activa la animación de muerte
        yield return new WaitForSeconds(0.75f); // Espera 0.75 segundos
        sr.enabled = false; // Desactiva el SpriteRenderer
        bc2D.enabled = false; // Desactiva el BoxCollider2D
        animator.SetBool("Dead", false); // Desactiva la animación de muerte
        animator.SetBool("Finish", true); // Activa la animación de finalización
        Destroy(gameObject); // Destruye el objeto del enemigo
    }
    
    // Corrutina para manejar el estado de herida del enemigo
    private IEnumerator Hurt()
    {
        animator.SetTrigger("Hurt"); // Activa la animación de herida
        yield return new WaitForSeconds(0.5f); // Espera 0.5 segundos
        isHurt = true; // Marca al enemigo como herido para detener su movimiento
        yield return new WaitForSeconds(0.5f); // Espera otros 0.5 segundos
        isHurt = false; // Desmarca al enemigo como herido
    }
    
    // Maneja las colisiones del enemigo con otros objetos
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player") && health > 0)
        {
            // Si el enemigo colisiona con el jugador y tiene vida
            collision.gameObject.GetComponent<CombatPlayer>().GetDamage(damage / 2, collision.GetContact(0).normal); // Inflige daño al jugador
            collision.transform.GetComponent<PlayerRespawn>().PlayerDamaged(); // Llama al método de daño del jugador
        }
    }
}
