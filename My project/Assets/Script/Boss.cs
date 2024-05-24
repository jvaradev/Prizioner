using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb2D;
    public Transform player;
    private bool isRigth = true;
    private bool isActivated = false;
    private bool isAttacking = false;

    // Vida Boss
    [SerializeField] private float maxHealth;
    [SerializeField] private float health;
    [SerializeField] private HealthBar healthBar;
    private bool isHurt = false;
    private bool isDead = false;

    // Velocidad del jefe
    [SerializeField] private float speed = 2f;

    // Ataque jefe
    [SerializeField] private Transform controllerAttack;
    [SerializeField] private Transform controllerAttack2;
    [SerializeField] private float radioAttack;
    [SerializeField] public float damage;

    // Disparo jefe
    public Transform controllerShot; // Transform que define la posición desde donde se disparan los proyectiles
    public Transform controllerShot2; // Transform que define la posición desde donde se disparan los proyectiles
    public float distancePlayer; // Distancia a la que se detecta al jugador
    public LayerMask layerPlayer; // Capa del jugador para detectar colisiones
    public bool playerInRange; // Indica si el jugador está en rango de disparo
    public bool playerInRange2; // Indica si el jugador está en rango de disparo
    public GameObject bullet; // Prefab de la bala que se dispara
    public GameObject bulletRight; // Prefab de la bala que se dispara
    public float timeShots; // Tiempo entre disparos
    private float timeLastShot; // Tiempo del último disparo
    public float timeWaiting; // Tiempo de espera antes de disparar

    private Coroutine attackCoroutine;
    [SerializeField] private GameObject wallScene;
    private BoxCollider2D bcWall;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        bcWall = wallScene.GetComponent<BoxCollider2D>();
        health = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        if (isActivated)
        {
            float distPlayer = Vector2.Distance(transform.position, player.position);
            animator.SetFloat("DistPlayer", distPlayer);

            if (IsPlayerInAttackRange())
            {
                rb2D.velocity = Vector2.zero; // Detener al jefe
                animator.SetBool("Walk", false);

                if (!isAttacking)
                {
                    isAttacking = true;
                    attackCoroutine = StartCoroutine(AttackRoutine());
                }
            }
            else
            {
                animator.SetBool("Walk", true);
                if (attackCoroutine != null)
                {
                    StopCoroutine(attackCoroutine);
                    attackCoroutine = null;
                }
                isAttacking = false; // Reiniciar el estado de ataque
                ChasePlayer();
            }

            playerInRange = Physics2D.Raycast(controllerShot.position, -transform.right, distancePlayer, layerPlayer);
            if (playerInRange)
            {
                // Si el jugador está en rango y ha pasado suficiente tiempo desde el último disparo
                if (Time.time > timeLastShot + timeShots)
                {
                    // Actualiza el tiempo del último disparo
                    timeLastShot = Time.time;
                    // Activa la animación de disparo
                    animator.SetTrigger("Shoot");
                    // Invoca el método Shoot después de un tiempo de espera
                    Invoke(nameof(Shoot), timeWaiting);
                }
            }

            playerInRange2 = Physics2D.Raycast(controllerShot2.position, transform.right, distancePlayer, layerPlayer);
            if (playerInRange2)
            {
                // Si el jugador está en rango y ha pasado suficiente tiempo desde el último disparo
                if (Time.time > timeLastShot + timeShots)
                {
                    // Actualiza el tiempo del último disparo
                    timeLastShot = Time.time;
                    // Activa la animación de disparo
                    animator.SetTrigger("Shoot");
                    // Invoca el método Shoot2 después de un tiempo de espera
                    Invoke(nameof(Shoot2), timeWaiting);
                }
            }
        }
        else
        {
            rb2D.velocity = Vector2.zero; // Mantén la velocidad en cero si no está activado
        }
    }

    private bool IsPlayerInAttackRange()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(controllerAttack.position, radioAttack);
        Collider2D[] objects2 = Physics2D.OverlapCircleAll(controllerAttack2.position, radioAttack);

        foreach (Collider2D collider in objects)
        {
            if (collider.CompareTag("Player"))
            {
                return true; // El jugador está en el rango de ataque
            }
        }

        foreach (Collider2D collider in objects2)
        {
            if (collider.CompareTag("Player"))
            {
                return true; // El jugador está en el rango de ataque
            }
        }

        return false; // El jugador no está en el rango de ataque
    }

    private void ChasePlayer()
    {
        animator.SetBool("Walk", true);
        Vector2 direction = (player.position - transform.position).normalized;
        rb2D.velocity = new Vector2(direction.x * speed, rb2D.velocity.y);

        // Voltear al jefe según la posición del jugador
        if ((player.position.x > transform.position.x && isRigth) || (player.position.x < transform.position.x && !isRigth))
        {
            Flip();
        }
    }

    private void Flip()
    {
        isRigth = !isRigth;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    public void ActivateBoss()
    {
        isActivated = true;
    }

    public void GetDamage(float damage)
    {
        health -= damage;
        healthBar.ChangeActualHealth(health);
        StartCoroutine(Hurt());
        if (health <= 0)
        {
            StartCoroutine(Dead());
        }
    }

    public void Attack()
    {
        // Collider de los círculos de acción del ataque.
        Collider2D[] objects = Physics2D.OverlapCircleAll(controllerAttack.position, radioAttack);
        Collider2D[] objects2 = Physics2D.OverlapCircleAll(controllerAttack2.position, radioAttack);

        // Cada Enemy que se encuentre dentro de cada círculo de acción recibe daño.
        foreach (Collider2D collider in objects)
        {
            if (collider.CompareTag("Player"))
            {
                collider.transform.GetComponent<CombatPlayer>().GetDamage(damage / 2);
            }
        }
        foreach (Collider2D collider in objects2)
        {
            if (collider.CompareTag("Player"))
            {
                collider.transform.GetComponent<CombatPlayer>().GetDamage(damage / 2);
            }
        }
    }
    
    // Método para instanciar la bala
    private void Shoot()
    {
        StartCoroutine(Freeze());
        // Instancia una bala en la posición y rotación del controllerShot
        Instantiate(bullet, controllerShot.position, controllerShot.rotation);
    }
    
    // Método para instanciar la bala para el segundo controlador de disparo
    private void Shoot2()
    {
        StartCoroutine(Freeze());
        // Instancia una bala en la posición y rotación del controllerShot2
        Instantiate(bulletRight, controllerShot2.position, controllerShot2.rotation);
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(2f);
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            if (isAttacking) // Solo atacar si aún está en modo ataque
            {
                Attack();
                animator.SetTrigger("Attack");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player") && health > 0)
        {
            // Si el enemigo colisiona con el jugador y tiene vida
            collision.gameObject.GetComponent<CombatPlayer>().GetDamage(damage / 2, collision.GetContact(0).normal); // Inflige daño al jugador
            collision.transform.GetComponent<PlayerRespawn>().PlayerDamaged(); // Llama al método de daño del jugador
        }
    }
    
    // Corrutina para manejar el estado de herida del enemigo
    private IEnumerator Hurt()
    {
        animator.SetTrigger("Hurt"); // Activa la animación de herida
        yield return new WaitForSeconds(0.25f); // Espera 0.5 segundos
        isHurt = true; // Marca al enemigo como herido para detener su movimiento
        isHurt = false; // Desmarca al enemigo como herido
    }
    
    private IEnumerator Freeze()
    {
        rb2D.velocity = Vector2.zero; // Mantén la velocidad en cero si no está activado
        animator.SetBool("Walk", false);
        yield return new WaitForSeconds(0.5f); // Espera 0.5 segundos
        animator.SetBool("Walk", true);
        rb2D.velocity = new Vector2(speed, 0);
    }
    private IEnumerator Dead()
    {
        isDead = true; // Marca al enemigo como muerto para detener su movimiento
        animator.SetBool("Death", true); // Activa la animación de muerte
        yield return new WaitForSeconds(0.75f); // Espera 0.75 segundos
        Destroy(gameObject); // Destruye el objeto del enemigo
        bcWall.enabled = false;
    }
    

    // Método para dibujar los círculos de acción. Para poder visualizar su funcionamiento. Solo aparece en consola.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controllerAttack.position, radioAttack);
        Gizmos.DrawWireSphere(controllerAttack2.position, radioAttack);
        Gizmos.DrawLine(controllerShot.position, controllerShot.position + transform.right * distancePlayer * -1);
        Gizmos.DrawLine(controllerShot2.position, controllerShot2.position + transform.right * distancePlayer * 1);
    }   
}
