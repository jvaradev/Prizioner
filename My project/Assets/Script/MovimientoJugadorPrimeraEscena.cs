using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoJugadorPrimeraEscena : MonoBehaviour
{
    public float speed = 2f;
    private Rigidbody2D rb2D;
    public Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        Movement();
    }

    //Movimiento horizontal con animaciones
    private void Movement()
    {
        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            animator.SetBool("Run", true);
            rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
            spriteRenderer.flipX = false; // No invertir la imagen
        }
        else if (Input.GetKey("a") || Input.GetKey("left"))
        {            
            animator.SetBool("Run", true);
            rb2D.velocity = new Vector2(-speed, rb2D.velocity.y);
            spriteRenderer.flipX = true; // Invertir horizontalmente
        }
        else
        {
            animator.SetBool("Run", false);
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
        }
    }

    
}