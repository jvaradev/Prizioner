using Unity.VisualScripting;
using UnityEngine;

//Código para controlar el movimiento del jugador
public class MovimientoJugador : MonoBehaviour
{
    // Variables públicas ajustables desde el editor
    public float speed = 5f; // Velocidad de movimiento
    public float jumpSpeed = 6f; // Velocidad de salto
    public bool rayCast = true; // Activar o desactivar raycast
    private Rigidbody2D rb2D; // Referencia al componente Rigidbody2D
    public Animator animator; // Referencia al componente Animator
    private SpriteRenderer spriteRenderer; // Referencia al componente SpriteRenderer
    private BoxCollider2D bc2D; // Referencia al componente BoxCollider2D
    private Vector2 originalColliderSize; // Tamaño original del collider
    private Vector2 originalColliderOffset; // Desplazamiento original del collider
    public bool sePuedeMover = true; // Determina si el jugador puede moverse
    [SerializeField] private Vector2 velocidadRebote; // Velocidad de rebote tras un golpe

    public bool headBlock; // Bloqueo de la cabeza del jugador
    [SerializeField] private BoxCollider2D crouchCollider; // Collider para cuando el jugador está agachado
    private Vector2 offsetCrouch; // Desplazamiento del collider cuando está agachado
    [SerializeField] private BoxCollider2D idleCollider; // Collider para cuando el jugador está de pie
    private Vector2 offsetIdle; // Desplazamiento del collider cuando está de pie
    public float distHead = 0.5f; // Distancia para detectar bloqueo en la cabeza
    public LayerMask capaPlataforma; // Capa de la plataforma para detectar colisiones

    [SerializeField] private float climbSpeed = 3f; // Velocidad de trepar
    private float inicialGravity; // Gravedad inicial del Rigidbody2D

    void Start()
    {
        // Inicializa las referencias a los componentes necesarios y almacena la gravedad inicial
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bc2D = GetComponent<BoxCollider2D>();
        inicialGravity = rb2D.gravityScale;
    } 

    void FixedUpdate()
    {
        // Llama a los métodos de movimiento si el jugador puede moverse
        rb2D.gravityScale = inicialGravity;
        if (sePuedeMover)
        {
            CheckTop(); // Comprueba si hay un obstáculo encima del jugador
            Movement(); // Maneja el movimiento horizontal
            Fall(); // Maneja la animación de caída
            Crouch(); // Maneja el estado de agacharse
            Jump(); // Maneja el salto
            Climb(); // Maneja la escalada
        }
    }
    
    // Maneja el movimiento horizontal del jugador y las animaciones correspondientes
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

    // Maneja el salto del jugador y las transiciones de animaciones de salto
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
    
    // Maneja la animación de caída cuando el jugador no está en el suelo
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

    // Maneja el movimiento de trepar cuando el jugador está en una escalera
    private void Climb()
    {
        bc2D.isTrigger = true;
        crouchCollider.isTrigger = true;
        float getDirection = Input.GetAxis("Vertical");
        if (CheckLadder.checkLadder)
        {
            if (Input.GetAxis("Vertical") != 0)
            {                
                animator.SetBool("IdleClimb", false);
                rb2D.velocity = new Vector2(rb2D.velocity.x, getDirection * climbSpeed);
            }

            if (Input.GetAxis("Vertical") == 0)
            {
                animator.SetBool("IdleClimb", true);
                rb2D.gravityScale = 0;
                rb2D.velocity = new Vector2(rb2D.velocity.x, 0);
            }
        }

        if (!CheckLadder.checkLadder)
        {
            bc2D.isTrigger = false;
            crouchCollider.isTrigger = false;
        }
    }
    
    // Aplica una fuerza de rebote al jugador tras un golpe
    public void Rebote(Vector2 puntoGolpe)
    {
        rb2D.velocity = new Vector2(-velocidadRebote.x * puntoGolpe.x, velocidadRebote.y);
    }

    // Maneja el estado de agacharse del jugador y activa las animaciones correspondiente
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
    
    // Comprueba si hay un bloqueo en la cabeza del jugador utilizando raycasting
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

    // Realiza un raycast hacia arriba desde la posición del jugador para detectar si hay una plataforma encima
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
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        //Cambiar velocidad si entra dentro del agua
        if (collision.CompareTag("Water"))
        {
            speed = 3f;
            jumpSpeed = 9f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            speed = 5f;
            jumpSpeed = 7f;
        }
    }
    
}
