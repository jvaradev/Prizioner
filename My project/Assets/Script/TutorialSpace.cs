using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class TutorialSpace : MonoBehaviour
{
    private Animator animatorKey; // Referencia al Animator del objeto "KeySpace"
    private GameObject keySpaceObject; // Referencia al objeto "KeySpace" en la escena
    private GameObject player; // Referencia al objeto del jugador

    private void Start()
    {
        // Encuentra el objeto "KeySpace" en la escena y asegúrate de que esté desactivado al principio
        keySpaceObject = GameObject.Find("KeySpace");
        keySpaceObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Este método se llama cuando otro collider está dentro del trigger collider 2D del objeto actual
        animatorKey = keySpaceObject.GetComponentInChildren<Animator>(); // Obtiene el Animator del objeto "KeySpace"
        if (collision.CompareTag("Player"))
        {
            // Si el objeto en colisión tiene la etiqueta "Player"
            keySpaceObject.SetActive(true); // Activa el objeto "KeySpace"
            animatorKey.SetBool("Enter", true); // Activa la animación de entrada en el objeto "KeySpace"
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Este método se llama cuando otro collider sale del trigger collider 2D del objeto actual
        keySpaceObject.SetActive(false); // Desactiva el objeto "KeySpace"
        CheckGround.isGround = true; // Establece la variable estática isGround en true (puede estar relacionada con otro script)
    }
}