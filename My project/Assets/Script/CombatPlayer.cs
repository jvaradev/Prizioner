using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPlayer : MonoBehaviour
{
    [SerializeField] public float positionXRespawn;
    [SerializeField] public float positionYRespawn;
    private float health;
    public static float maxHealth = 100;
    [SerializeField]private HealthBar healthBar;
    private MovimientoJugador movimientoJugador;
    [SerializeField] private float tiempoPerdida;
    private Animator animator;
    private BoxCollider2D bc2D;
    
    // Start is called before the first frame update
    private void Start()
    {
        movimientoJugador = GetComponent<MovimientoJugador>();
        animator = GetComponent<Animator>();
        health = maxHealth;
        healthBar.inicialiteActualHealth(health);
    }

    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            StartCoroutine(PerderControl(1f));
        }
    }

    // Update is called once per frame
    
    public void GetDamage(float damage, Vector2 posicion)
    {
        health -= damage;
        healthBar.changeActualHealth(health);
        StartCoroutine(PerderControl(tiempoPerdida));
        movimientoJugador.Rebote(posicion);
        if (health <= 0)
        {
            bc2D = GetComponent<BoxCollider2D>();
            animator.SetBool("Death", true);
            // Espera unos segundos antes de cambiar de posición
            StartCoroutine(EsperarAntesDeCambiarPosicion());
        }
    }
    public void GetDamage(float damage)
    {
        health -= damage;
        healthBar.changeActualHealth(health);
        StartCoroutine(PerderControl(tiempoPerdida));
        if (health <= 0)
        {
            bc2D = GetComponent<BoxCollider2D>();
            animator.SetBool("Death", true);
            // Espera unos segundos antes de cambiar de posición
            StartCoroutine(EsperarAntesDeCambiarPosicion());
        }
    }
    
    public float GetHealth()
    {
        return health;
    }
    
    private IEnumerator PerderControl(float tiempoPerdida)
    {
        movimientoJugador.sePuedeMover = false;
        yield return new WaitForSeconds(tiempoPerdida);
        movimientoJugador.sePuedeMover = true;
    }

    private IEnumerator EsperarAntesDeCambiarPosicion()
    {
        movimientoJugador.sePuedeMover = false;
        // Espera unos segundos después de la muerte
        yield return new WaitForSeconds(2.0f); // Cambia este valor según lo que necesites
        // Cambiar la posición después de la espera
        bc2D.transform.position = new Vector3(positionXRespawn, positionYRespawn);
        animator.SetBool("Death", false);
        health = maxHealth;
        healthBar.changeActualHealth(health);
        movimientoJugador.sePuedeMover = true;
    }

}