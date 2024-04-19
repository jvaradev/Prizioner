using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public float speed = 2f;
    public float jumpSpeed = 3f;
    private Rigidbody2D rb2D;
    public Animator animator;
    private SpriteRenderer spriteRenderer;
    public bool sePuedeMover = true;
    [SerializeField] private Vector2 velocidadRebote;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (sePuedeMover)
        {
            Movement();
            Jump();
            Fall(); 
        }
        
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

    //Movimiento salto con transición de animaciones
    private void Jump()
    {
        if (Input.GetKey("space") && CheckGround.isGround)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpSpeed);
        }

        if (CheckGround.isGround==false)
        {
            animator.SetBool("Jump", true);
            animator.SetBool("Run", false);
        }
        
        if (CheckGround.isGround)
        {
            animator.SetBool("Jump", false);
            animator.SetBool("Fall", false);
        }
    }
    
    //Movimiento de caer con animaciones
    private void Fall()
    {
        if (rb2D.velocity.y<0)
        {
            animator.SetBool("Fall", true);
        }
        else if (rb2D.velocity.y>0)
        {
            animator.SetBool("Fall", false);
        }
    }

    public void Rebote(Vector2 puntoGolpe)
    {
        rb2D.velocity = new Vector2(-velocidadRebote.x * puntoGolpe.x, velocidadRebote.y);
    }
}