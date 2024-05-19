using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//Código que controla el combate del Jugador. Ataque, Respawn y Sistema de Vida.
public class CombatPlayer : MonoBehaviour
{
    //Variables para la posicion en X e Y del Respawn del jugador.
    [SerializeField] private float positionXRespawn;
    [SerializeField] private float positionYRespawn;
    
    //Variables para poder controlar el sistema de vida.
    [SerializeField] private HealthBar healthBar;
    private bool canCure = true; // Añadido para controlar si se puede curar
    private float health;
    public static float maxHealth = 100;
    
    //Variable para que el jugadro no pueda moverse al recibir daño.
    [SerializeField] private float tiempoPerdida;
    
    //Variables para controlar el sistema de ataque
    [SerializeField] private Transform controller;
    [SerializeField] private Transform controller2;
    [SerializeField] private float radioHit;
    [SerializeField] private float damage;
    private bool isAttacking = false;
    
    //Variables para controlar al jugador.
    private Rigidbody2D rb2D;
    private Animator animator;
    private BoxCollider2D bc2D;
    private MovimientoJugador movimientoJugador;
    
    //Llamar a los componentes
    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        movimientoJugador = GetComponent<MovimientoJugador>();
        animator = GetComponent<Animator>();
        health = maxHealth;
        healthBar.inicialiteActualHealth(health);
    }

    private void Update()
    {
        //Verificar si no está atacando y se pulsa la tecla asignada a Fire1 (Click izquierdo ratón)
        if (!isAttacking &&Input.GetButton("Fire1"))
        {
            //Cambio parametros para comenzar animación "Punch"
            animator.SetTrigger("Punch");
            animator.SetBool("Run",false);
            isAttacking = true; // El personaje está atacando
            StartCoroutine(PerformAttack());
            StartCoroutine(PerderControl(1f));
        }
        //Verificar si puede cuarase el jugador, pulsa la letra Q y tiene objetos de cura
        if (canCure && Input.GetKey("q") && CountCigarrete.count > 0) // Añadido para controlar la curación
        {
            Cure();
        }
    }
    
    //Código de realización de ataque
    private void Punch()
    {
        //Collider de la círculos de acción del ataque.
        Collider2D[] objets = Physics2D.OverlapCircleAll(controller.position, radioHit);
        Collider2D[] objets2 = Physics2D.OverlapCircleAll(controller2.position, radioHit);

        //Cada Enemy que se encuentre dentro de cada círculo de acción recibe daño.
        foreach (Collider2D collider in objets)
        {
            if (collider.CompareTag("Enemy"))
            {
                collider.transform.GetComponent<PatrullaEnemigo>().GetDamage(damage);
            }
        }
        foreach (Collider2D collider in objets2)
        {
            if (collider.CompareTag("Enemy"))
            {
                collider.transform.GetComponent<PatrullaEnemigo>().GetDamage(damage);
            }
        }
    }
    public void GetDamage(float damage, Vector2 posicion)
    {
        health -= damage;
        healthBar.changeActualHealth(health);
        StartCoroutine(PerderControl(tiempoPerdida));
        movimientoJugador.Rebote(posicion);
        if (health <= 0)
        {
            bc2D = GetComponent<BoxCollider2D>();
            animator.SetBool("Death",true);
            StartCoroutine(HandleDeath());
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
            animator.SetBool("Death",true);
            StartCoroutine(HandleDeath());
        }
    }

    public void Cure()
    {
        if (health < maxHealth)
        {
            health += 10;
            CountCigarrete.count -= 1; // Resta 1 a la cantidad de cigarrillos
            health = Mathf.Clamp(health, 0, maxHealth); // Limita la salud al máximo de salud
            healthBar.changeActualHealth(health);
        }
        canCure = false; // Desactiva la capacidad de curación temporalmente
        StartCoroutine(ReactivateCure()); // Reactiva la capacidad de curación después de un tiempo
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

    private IEnumerator HandleDeath()
    {
        movimientoJugador.sePuedeMover = false;
        // Espera unos segundos después de la muerte
        yield return new WaitForSeconds(2.0f);
        ResetCounters();
        CheckGround.isGround = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // Cambiar la posición después de la espera
        bc2D.transform.position = new Vector3(positionXRespawn, positionYRespawn);
        animator.SetBool("Death", false);
        health = maxHealth;
        healthBar.changeActualHealth(health);
        movimientoJugador.sePuedeMover = true;
    }

    private void ResetCounters()
    {
        CountCard.count = 0;
        CountCigarrete.count = 0;
    }

    private IEnumerator ReactivateCure()
    {
        yield return new WaitForSeconds(1f); // Tiempo de espera antes de reactivar la curación
        canCure = true;
    }
    
    //Corrutina para realizar ataque
    private IEnumerator PerformAttack()
    {
        Punch(); // Realizar el ataque

        // Congelar la posición durante el ataque
        rb2D.constraints = RigidbodyConstraints2D.FreezePosition;

        // Esperar hasta que termine la animación de ataque
        yield return new WaitForSeconds(1f);

        // Descongelar la posición después del ataque
        rb2D.constraints = RigidbodyConstraints2D.None;
        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        isAttacking = false; // El personaje ha terminado de atacar
    }
    
    //Método para dibujar los círculos de acción. Para poder visualizar su funcionamiento. Solo aparece en consola.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controller.position, radioHit);
        Gizmos.DrawWireSphere(controller2.position, radioHit);
    }
}
