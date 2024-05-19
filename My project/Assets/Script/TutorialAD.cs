using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class TutorialAD : MonoBehaviour
{
    private Animator animatorKey; // Referencia al Animator del objeto "KeyAD"
    private GameObject keyADObject; // Referencia al objeto "KeyAD" en la escena

    private void Start()
    {
        // Encuentra el objeto "KeyAD" en la escena y asegúrate de que esté desactivado al principio
        keyADObject = GameObject.Find("KeyAD");
        keyADObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Este método se llama cuando otro collider está dentro del trigger collider 2D del objeto actual
        animatorKey = keyADObject.GetComponentInChildren<Animator>(); // Obtiene el Animator del objeto "KeyAD"
        if (collision.CompareTag("Player"))
        {
            // Si el objeto en colisión tiene la etiqueta "Player"
            keyADObject.SetActive(true); // Activa el objeto "KeyAD"
            animatorKey.SetBool("Enter", true); // Activa la animación de entrada en el objeto "KeyAD"
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Este método se llama cuando otro collider sale del trigger collider 2D del objeto actual
        keyADObject.SetActive(false); // Desactiva el objeto "KeyAD"
        CheckGround.isGround = true; // Establece la variable estática isGround en true (puede estar relacionada con otro script)
    }
}