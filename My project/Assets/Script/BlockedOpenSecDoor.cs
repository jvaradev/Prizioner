using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

//Método puertas bloqueadas
public class BlockedOpenSecDoor : MonoBehaviour
{
    private Animator animator;
    private float tiempoApertura = 2;
    private bool sceneChanged = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Verificar que si el jugador está dentro del Collider de la puerta
        if (collision.CompareTag("Player"))
        {
            //Si el jugddor tiene la tarjeta se puede abrir (Contorno en blanco)
            if (CountCard.count > 0)
            {
                animator.SetBool("Interact", true);
            }//Si el jugador no tiene la tarjeta no se puede abrir (Contorno en rojo)
            else
            {
                animator.SetBool("Blocked", true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //Cuando el jugador sale del Collider de la puerta se quitan controno y se verifica que el jugador está en el suelo
        animator.SetBool("Interact", false);
        animator.SetBool("Blocked", false);
        CheckGround.isGround = true;
    }

    private void Update()
    {
        //Verificar que el jugador pulsa la tecla e, no se ha cambiado de escena y que tiene tarjeta
        if (Input.GetKey("e") && !sceneChanged && CountCard.count > 0)
        {
            //Se cambia Open a true para cambiar la animación y empieza la Corrutina
            animator.SetBool("Open", true);
            StartCoroutine(TiempoAbrirPuerta());
        }
    }

    //Corrutina para esperar a abrir la puerta
    public IEnumerator TiempoAbrirPuerta()
    {
        yield return new WaitForSeconds(tiempoApertura);
        if (!sceneChanged)
        {
            sceneChanged = true;
            SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1));
            CountCard.count = 0;
        }
    }
}