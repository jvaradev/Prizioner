using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPlayer : MonoBehaviour
{
    private float health;
    private float maxHealth = 100;
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetDamage(float damage, Vector2 posicion)
    {
        health -= damage;
        healthBar.changeActualHealth(health);
        StartCoroutine(PerderControl());
        movimientoJugador.Rebote(posicion);
        if (health <= 0)
        {
            bc2D = GetComponent<BoxCollider2D>();
            animator.SetBool("Death", true);
        }
    }
    
    public IEnumerator PerderControl()
    {
        movimientoJugador.sePuedeMover = false;
        yield return new WaitForSeconds(tiempoPerdida);
        movimientoJugador.sePuedeMover = true;

    }
}
