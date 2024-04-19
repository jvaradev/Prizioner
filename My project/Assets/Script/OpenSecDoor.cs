using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class OpenSecDoor : MonoBehaviour
{
    private Animator animator;
    private float tiempoApertura = 1;
    private bool sceneChanged = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        animator = GetComponent<Animator>();
        if (collision.CompareTag("Player") && Input.GetKey("e") && !sceneChanged)
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