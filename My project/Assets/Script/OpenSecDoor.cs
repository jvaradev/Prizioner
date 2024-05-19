using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenSecDoor : MonoBehaviour
{
    private Animator animator; // Referencia al componente Animator del objeto actual
    private Animator animatorKey; // Referencia al componente Animator del objeto "Square"
    private GameObject squareObject; // Referencia al objeto "Square" en la escena
    private float tiempoApertura = 2; // Tiempo de apertura de la puerta en segundos
    private bool sceneChanged = false; // Indica si la escena ha cambiado

    private void Start()
    {
        // Encuentra el objeto "Square" en la escena y está desactivado al principio
        squareObject = GameObject.Find("Square");
        squareObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Este método se llama cuando otro collider está dentro del trigger collider 2D del objeto actual
        animator = GetComponent<Animator>(); // Obtiene el Animator del objeto actual
        animatorKey = squareObject.GetComponentInChildren<Animator>(); // Obtiene el Animator del objeto "Square"
        if (collision.CompareTag("Player"))
        {
            // Si el objeto en colisión tiene la etiqueta "Player"
            animator.SetBool("Interact", true); // Activa la animación de interacción
            squareObject.SetActive(true); // Activa el objeto "Square"
            squareObject.transform.localScale = new Vector2(4, 4); // Cambia el tamaño del objeto "Square"
            animatorKey.SetBool("Start", true); // Activa la animación de inicio en el objeto "Square"
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Este método se llama cuando otro collider sale del trigger collider 2D del objeto actual
        animator.SetBool("Interact", false); // Desactiva la animación de interacción
        squareObject.SetActive(false); // Desactiva el objeto "Square"
        CheckGround.isGround = true; // Establece la variable estática isGround en true (puede estar relacionada con otro script)
    }

    private void Update()
    {
        // Este método se llama en cada frame
        if (Input.GetKey("e") && !sceneChanged)
        {
            // Si se presiona la tecla "e" y la escena no ha cambiado
            animator.SetBool("Open", true); // Activa la animación de apertura
            StartCoroutine(TiempoAbrirPuerta()); // Inicia la corrutina para esperar y cambiar de escena
        }
    }

    public IEnumerator TiempoAbrirPuerta()
    {
        // Corrutina para esperar un tiempo antes de cambiar de escena
        yield return new WaitForSeconds(tiempoApertura); // Espera el tiempo de apertura
        if (!sceneChanged)
        {
            // Si la escena no ha cambiado
            sceneChanged = true; // Marca que la escena ha cambiado
            SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex + 1)); // Carga la siguiente escena
        }
    }
}
