using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BlockedOpenSecDoor : MonoBehaviour
{
    private Animator animator;
    private float tiempoApertura = 2;
    private bool sceneChanged = false;
    private bool stayDoor;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (CountCard.count > 0)
            {
                animator.SetBool("Interact", true);
            }
            else
            {
                animator.SetBool("Blocked", true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        animator.SetBool("Interact", false);
        animator.SetBool("Blocked", false);
        stayDoor = false;
        CheckGround.isGround = true;
    }

    private void Update()
    {
        if (Input.GetKey("e") && !sceneChanged && CountCard.count > 0)
        {
            animator.SetBool("Open", true);
            StartCoroutine(TiempoAbrirPuerta());
        }
    }

    public IEnumerator TiempoAbrirPuerta()
    {
        yield return new WaitForSeconds(tiempoApertura);
        if (!sceneChanged)
        {
            sceneChanged = true;
            SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1));
        }
    }
}