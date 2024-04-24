using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement; 

public class TutorialAD : MonoBehaviour
{
    private Animator animatorKey;
    private GameObject keyADObject;
    private void Start()
    {
        // Encuentra el objeto "Square" en la escena
        keyADObject = GameObject.Find("KeyAD");
        // Asegúrate de que esté desactivado al principio
        keyADObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        animatorKey = keyADObject.GetComponentInChildren<Animator>();
        if (collision.CompareTag("Player"))
        {
            keyADObject.SetActive(true);
            animatorKey.SetBool("Enter", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        keyADObject.SetActive(false);
        CheckGround.isGround = true;
    }

}