using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public float speed = 5f;
    public float jumpSpeed = 4f;
    private Rigidbody2D rb2D;
    public Animator animator;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D bc2D;
    private Vector2 originalColliderSize; // Almacena el tamaño original del collider
    private Vector2 originalColliderOffset; // Almacena el desplazamiento original del collider
    public bool sePuedeMover = true;
    [SerializeField] private Vector2 velocidadRebote;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bc2D = GetComponent<BoxCollider2D>();

        // Almacenar el tamaño y el offset originales del collider
        originalColliderSize = bc2D.size;
        originalColliderOffset = bc2D.offset;
    }

    void FixedUpdate()
    {
        if (sePuedeMover)
        {
            Movement();
            Jump();
            Fall();
            Crouch();
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
        if (Input.GetKey("w") && CheckGround.isGround)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpSpeed);
        }

        if (CheckGround.isGround == false)
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
        if (rb2D.velocity.y < 0)
        {
            animator.SetBool("Fall", true);
        }
        else if (rb2D.velocity.y > 0)
        {
            animator.SetBool("Fall", false);
        }
    }

    public void Rebote(Vector2 puntoGolpe)
    {
        rb2D.velocity = new Vector2(-velocidadRebote.x * puntoGolpe.x, velocidadRebote.y);
    }

    public void Crouch()
    {
        speed = 3f;
        if (Input.GetKey("s") && CheckGround.isGround)
        {
            // Reducir el tamaño del collider
            bc2D.size = new Vector2(originalColliderSize.x, originalColliderSize.y * 0.5f);
            // Mover el collider hacia abajo
            bc2D.offset = new Vector2(originalColliderOffset.x, originalColliderOffset.y - (originalColliderSize.y * 0.25f));
            animator.SetBool("Crouch", true);
            if (Input.GetKey("d"))
            {
                animator.SetBool("CrouchWalk", true);
                rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
                spriteRenderer.flipX = false;
            }
            else if (Input.GetKey("a"))
            {
                animator.SetBool("CrouchWalk", true);
                rb2D.velocity = new Vector2(-speed, rb2D.velocity.y);
                spriteRenderer.flipX = true; // Invertir horizontalmente
            }
            else
            {
                animator.SetBool("CrouchWalk", false);
            }
        }
        else
        {
            // Restaurar el tamaño y el offset originales del collider
            bc2D.size = originalColliderSize;
            bc2D.offset = originalColliderOffset;
            animator.SetBool("Crouch", false);
        }
    }
}
