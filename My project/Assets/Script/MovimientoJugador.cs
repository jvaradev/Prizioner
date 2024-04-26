using Script;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    public float speed = 5f;
    public float jumpSpeed = 6f;
    public bool rayCast = true;
    private Rigidbody2D rb2D;
    public Animator animator;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D bc2D;
    private Vector2 originalColliderSize; // Almacena el tamaño original del collider
    private Vector2 originalColliderOffset; // Almacena el desplazamiento original del collider
    public bool sePuedeMover = true;
    [SerializeField] private Vector2 velocidadRebote;

    public bool headBlock;
    [SerializeField] private BoxCollider2D crouchCollider;
    private Vector2 offsetCrouch;
    [SerializeField] private BoxCollider2D idleCollider;
    private Vector2 offsetIdle;
    public float distHead = 0.5f;
    public LayerMask capaPlataforma;

    [SerializeField] private float climbSpeed = 3f;
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bc2D = GetComponent<BoxCollider2D>();

    }

    void FixedUpdate()
    {
        if (sePuedeMover)
        {
            CheckTop();
            Movement();
            Fall();
            Crouch();
            Jump();
            Climb();
        }
    }
    
    
    //Movimiento horizontal con animaciones
    private void Movement()
    {
        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            animator.SetBool("Run", true);
            animator.SetBool("Fall", false);
            rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
            spriteRenderer.flipX = false; // No invertir la imagen
        }
        else if (Input.GetKey("a") || Input.GetKey("left"))
        {
            animator.SetBool("Run", true);
            animator.SetBool("Fall", false);
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

        if (!CheckGround.isGround)
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
        if (!CheckGround.isGround && rb2D.velocity.y < 0)
        {
            animator.SetBool("Fall", true);
        }
        else
        {
            animator.SetBool("Fall", false);
        }
    }

    private void Climb()
    {
        float getDirection = Input.GetAxis("Vertical");
        if (CheckLadder.checkLadder)
        {
            if (Input.GetAxis("Vertical") != 0)
            {
                rb2D.velocity = new Vector2(rb2D.velocity.x, getDirection * climbSpeed);
            }
        }
    }

    public void Rebote(Vector2 puntoGolpe)
    {
        rb2D.velocity = new Vector2(-velocidadRebote.x * puntoGolpe.x, velocidadRebote.y);
    }

    //posiblemente tenga que separar en Crouch y CrouchWalk
    public void Crouch()
    {
        if (Input.GetKey("s") && CheckGround.isGround)
        {
            speed = 3f;
            animator.SetBool("Crouch", true);
            animator.SetBool("CrouchWalk", false);
            idleCollider.enabled = false;
            if (Input.GetKey("d"))
            {
                animator.SetBool("CrouchWalk", true);
                rb2D.velocity = new Vector2(speed, rb2D.velocity.y);
                spriteRenderer.flipX = false;
            }
            if (Input.GetKey("a"))
            {
                animator.SetBool("CrouchWalk", true);
                rb2D.velocity = new Vector2(-speed, rb2D.velocity.y);
                spriteRenderer.flipX = true; // Invertir horizontalmente
            }
        }
        else
        {
            CheckHeadBlock();
        }
    }
    
    private void CheckHeadBlock()
    {
        if (!headBlock)
        {
            speed = 5f;
            animator.SetBool("Crouch", false);
            animator.SetBool("CrouchWalk", false);
            idleCollider.enabled = true;
        }

        if (headBlock && ((!Input.GetKey("d"))||!Input.GetKey("a")))
        {
            animator.SetBool("Crouch", true);
            animator.SetBool("CrouchWalk", false);
        }
            
        if (headBlock && ((Input.GetKey("d"))||Input.GetKey("a")))
        {
            animator.SetBool("Crouch", true);
            animator.SetBool("CrouchWalk", true);
        }
    }

    void CheckTop()
    {
        Vector2 pos = transform.position;
        Vector2 offset = new Vector2(0f, idleCollider.size.y);

        RaycastHit2D checkHead = Physics2D.Raycast(pos + offset, Vector2.up, distHead, capaPlataforma);

        if (checkHead)
        {
            headBlock = true;
        }
        else
        {
            headBlock = false;
        }
    }
}
