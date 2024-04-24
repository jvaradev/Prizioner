using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement; 

public class OpenSecDoor : MonoBehaviour
{
    private Animator animator;
    private Animator animatorKey;
    private GameObject squareObject;
    private float tiempoApertura = 2;
    private bool sceneChanged = false;
    private bool stayDoor;
    private void Start()
    {
        // Encuentra el objeto "Square" en la escena
        squareObject = GameObject.Find("Square");
        // Asegúrate de que esté desactivado al principio
        squareObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        animator = GetComponent<Animator>();
        animatorKey = squareObject.GetComponentInChildren<Animator>();
        if (collision.CompareTag("Player"))
        {
            squareObject.SetActive(true);
            squareObject.transform.localScale = new Vector2(4, 4);
            animatorKey.SetBool("Start", true);
            stayDoor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        squareObject.SetActive(false);
        stayDoor = false;
        CheckGround.isGround = true;
    }

    private void Update()
    {
        if (Input.GetKey("e") && !sceneChanged && stayDoor)
        {
            Debug.Log("Tecla E presionada");
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