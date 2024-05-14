using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CombatPlayer : MonoBehaviour
{
    [SerializeField] private float positionXRespawn;
    [SerializeField] private float positionYRespawn;
    private float health;
    public static float maxHealth = 100;
    [SerializeField] private HealthBar healthBar;
    private MovimientoJugador movimientoJugador;
    [SerializeField] private float tiempoPerdida;
    private Animator animator;
    private BoxCollider2D bc2D;
    private bool canCure = true; // Añadido para controlar si se puede curar

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
        if (canCure && Input.GetKey("q") && CountCigarrete.count > 0) // Añadido para controlar la curación
        {
            Cure();
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

    public void Cure()
    {
        health += 10;
        CountCigarrete.count -= 1; // Resta 1 a la cantidad de cigarrillos
        health = Mathf.Clamp(health, 0, maxHealth); // Limita la salud al máximo de salud
        healthBar.changeActualHealth(health);
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

    private IEnumerator EsperarAntesDeCambiarPosicion()
    {
        movimientoJugador.sePuedeMover = false;
        // Espera unos segundos después de la muerte
        yield return new WaitForSeconds(2.0f);
        CheckGround.isGround = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // Cambiar la posición después de la espera
        bc2D.transform.position = new Vector3(positionXRespawn, positionYRespawn);
        animator.SetBool("Death", false);
        health = maxHealth;
        healthBar.changeActualHealth(health);
        movimientoJugador.sePuedeMover = true;
    }

    private IEnumerator ReactivateCure()
    {
        yield return new WaitForSeconds(1f); // Tiempo de espera antes de reactivar la curación
        canCure = true;
    }
}
