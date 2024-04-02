using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class PatrullaEnemigo : MonoBehaviour
{
    [SerializeField] private float velocidad;
    [SerializeField] private Transform[] puntos;
    [SerializeField] private float distanciaMinima;
    private int numAlt;
    private SpriteRenderer sr;

    private void Start()
    {
        numAlt = Random.Range(0, puntos.Length);
        sr = GetComponent<SpriteRenderer>();
        Girar();
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, puntos[numAlt].position, velocidad * Time.deltaTime);

        if (Vector2.Distance(transform.position, puntos[numAlt].position)<distanciaMinima)
        {
            numAlt = Random.Range(0, puntos.Length);
            Girar();
        }
    }

    private void Girar()
    {
        if (transform.position.x < puntos[numAlt].position.x)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }
}
