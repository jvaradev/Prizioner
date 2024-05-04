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
    [SerializeField] private float health;
    [SerializeField] private float velocidad;
    [SerializeField] private Transform[] puntos;
    [SerializeField] private float distanciaMinima;
    [SerializeField] public float damage;
    private int numAlt;
    private SpriteRenderer sr;
    private Animator animator;
    private bool isDead = false; // Variable para controlar si el enemigo está muerto
    private bool isHurt = false;
    

    private void Start()
    {
        numAlt = Random.Range(0, puntos.Length);
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        Girar();
    }

    private void Update()
    {
        if (health<=0)
        {
            StartCoroutine(Dead());
        }
        if (!isDead && !isHurt) // Solo realiza el movimiento si el enemigo no está muerto
        {
            animator.SetBool("Walk",true);
            transform.position = Vector2.MoveTowards(transform.position, puntos[numAlt].position, velocidad * Time.deltaTime);
            if (Vector2.Distance(transform.position, puntos[numAlt].position) < distanciaMinima)
            {
                numAlt = Random.Range(0, puntos.Length);
                Girar();
            }
        }
    }

    private void Girar()
    {
        if (transform.position.x < puntos[numAlt].position.x)
        {
            sr.flipX = false;
        }
        else
        {
            sr.flipX = true;
        }
    }
    public void GetDamage(float damage)
    {
        health -= damage;
        StartCoroutine(Hurt());
        if (health <= 0)
        {
            StartCoroutine(Dead());
        }
    }

    private IEnumerator Dead()
    {
        isDead = true; // Marcar al enemigo como muerto para detener su movimiento
        animator.SetBool("Dead", true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("Dead", false);
        animator.SetBool("Finish", true);
    }
    
    private IEnumerator Hurt()
    {
        animator.SetTrigger("Hurt");
        yield return new WaitForSeconds(0.5f);
        isHurt = true; // Marcar al enemigo como herido para detener su movimiento
        yield return new WaitForSeconds(0.5f);
        isHurt = false;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player") && health > 0)
        {
            collision.gameObject.GetComponent<CombatPlayer>().GetDamage(damage/2, collision.GetContact(0).normal);
            collision.transform.GetComponent<PlayerRespawn>().PlayerDamaged();
        }
    }
    
}