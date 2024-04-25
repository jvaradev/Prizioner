using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement; 

public class TutorialS : MonoBehaviour
{
    private Animator animatorKey;
    private GameObject keySObject;
    private void Start()
    {
        // Encuentra el objeto "Square" en la escena
        keySObject = GameObject.Find("KeyS");
        // Asegúrate de que esté desactivado al principio
        keySObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        animatorKey = keySObject.GetComponentInChildren<Animator>();
        if (collision.CompareTag("Player"))
        {
            keySObject.SetActive(true);
            animatorKey.SetBool("Enter", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        keySObject.SetActive(false);
        CheckGround.isGround = true;
    }

}