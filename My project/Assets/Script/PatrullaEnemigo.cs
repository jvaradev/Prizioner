using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class PatrullaEnemigo : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float velocidad;
    [SerializeField] private Transform[] puntos;
    [SerializeField] private float distanciaMinima;
    private int numAlt;
    private SpriteRenderer sr;
    private Animator animator;
    private bool isDead = false; // Variable para controlar si el enemigo está muerto

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
            Dead();
        }
        if (!isDead) // Solo realiza el movimiento si el enemigo no está muerto
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
        if (health <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        animator.SetTrigger("Dead");
        isDead = true; // Marcar al enemigo como muerto para detener su movimiento
    }
}