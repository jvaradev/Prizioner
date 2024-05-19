using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class TutorialS : MonoBehaviour
{
    private Animator animatorKey; // Referencia al Animator del objeto "KeyS"
    private GameObject keySObject; // Referencia al objeto "KeyS" en la escena

    private void Start()
    {
        // Encuentra el objeto "KeyS" en la escena y asegúrate de que esté desactivado al principio
        keySObject = GameObject.Find("KeyS");
        keySObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Este método se llama cuando otro collider está dentro del trigger collider 2D del objeto actual
        animatorKey = keySObject.GetComponentInChildren<Animator>(); // Obtiene el Animator del objeto "KeyS"
        if (collision.CompareTag("Player"))
        {
            // Si el objeto en colisión tiene la etiqueta "Player"
            keySObject.SetActive(true); // Activa el objeto "KeyS"
            animatorKey.SetBool("Enter", true); // Activa la animación de entrada en el objeto "KeyS"
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Este método se llama cuando otro collider sale del trigger collider 2D del objeto actual
        keySObject.SetActive(false); // Desactiva el objeto "KeyS"
        CheckGround.isGround = true; // Establece la variable estática isGround en true (puede estar relacionada con otro script)
    }
}