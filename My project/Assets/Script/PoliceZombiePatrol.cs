using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = System.Object;
using Random = UnityEngine.Random;

public class PoliceZombiePatrol : MonoBehaviour
{
    [SerializeField] private float health; // Vida del enemigo
    [SerializeField] private float velocidad; // Velocidad de movimiento del enemigo
    [SerializeField] private Transform[] puntos; // Puntos de patrullaje
    [SerializeField] private float distanciaMinima; // Distancia mínima para cambiar de punto de patrullaje
    [SerializeField] public float damage; // Daño que inflige el enemigo
    private int numAlt; // Índice del punto de patrullaje actual
    private BoxCollider2D bc2D; // Referencia al BoxCollider2D del enemigo
    private Rigidbody2D rb2D;
    private SpriteRenderer sr; // Referencia al SpriteRenderer del enemigo
    private Animator animator; // Referencia al Animator del enemigo
    private bool isDead = false; // Variable para controlar si el enemigo está muerto
    private bool isHurt = false; // Variable para controlar si el enemigo está herido
    
    // Ataque
    [SerializeField] private Transform controllerAttack;
    [SerializeField] private Transform controllerAttack2;
    [SerializeField] private float radioAttack;
    [SerializeField] public float damageAttack;
    private bool isAttacking = false;
    private Coroutine attackCoroutine;
    
    private void Start()
    {
        // Inicializa las variables y componentes necesarios
        numAlt = Random.Range(0, puntos.Length); // Selecciona un punto de patrullaje aleatorio
        animator = GetComponent<Animator>(); // Obtiene el Animator del enemigo
        rb2D = GetComponent<Rigidbody2D>();
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
            if (IsPlayerInAttackRange())
            {
                rb2D.velocity = Vector2.zero; // Detener
                animator.SetBool("Walk", false);

                if (!isAttacking)
                {
                    isAttacking = true;
                    attackCoroutine = StartCoroutine(AttackRoutine());
                }
            }
            else
            {
                if (isAttacking)
                {
                    isAttacking = false;
                    if (attackCoroutine != null)
                    {
                        StopCoroutine(attackCoroutine);
                        attackCoroutine = null;
                    }
                }
                
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
    
    public void Attack()
    {
        // Collider de los círculos de acción del ataque.
        Collider2D[] objects = Physics2D.OverlapCircleAll(controllerAttack.position, radioAttack);
        Collider2D[] objects2 = Physics2D.OverlapCircleAll(controllerAttack2.position, radioAttack);

        // Cada Enemy que se encuentre dentro de cada círculo de acción recibe daño.
        foreach (Collider2D collider in objects)
        {
            if (collider.CompareTag("Player"))
            {
                collider.transform.GetComponent<CombatPlayer>().GetDamage(damage / 2);
            }
        }
        foreach (Collider2D collider in objects2)
        {
            if (collider.CompareTag("Player"))
            {
                collider.transform.GetComponent<CombatPlayer>().GetDamage(damage / 2);
            }
        }
    }
    
    private bool IsPlayerInAttackRange()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(controllerAttack.position, radioAttack);
        Collider2D[] objects2 = Physics2D.OverlapCircleAll(controllerAttack2.position, radioAttack);

        foreach (Collider2D collider in objects)
        {
            if (collider.CompareTag("Player"))
            {
                return true; // El jugador está en el rango de ataque
            }
        }

        foreach (Collider2D collider in objects2)
        {
            if (collider.CompareTag("Player"))
            {
                return true; // El jugador está en el rango de ataque
            }
        }

        return false; // El jugador no está en el rango de ataque
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
    
    private IEnumerator AttackRoutine()
    {
        animator.SetTrigger("Charge");
        while (isAttacking)
        {
            yield return new WaitForSeconds(0.7f);
            if (isAttacking) // Solo atacar si aún está en modo ataque
            {
                animator.SetTrigger("Attack");
                Attack();
            }
        }
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
    
    // Método para dibujar los círculos de acción. Para poder visualizar su funcionamiento. Solo aparece en consola.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controllerAttack.position, radioAttack);
        Gizmos.DrawWireSphere(controllerAttack2.position, radioAttack);
    }   
}
