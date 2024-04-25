using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement; 

public class TutorialSpace : MonoBehaviour
{
    private Animator animatorKey;
    private GameObject keySpaceObject;
    private void Start()
    {
        // Encuentra el objeto "Square" en la escena
        keySpaceObject = GameObject.Find("KeySpace");
        // Asegúrate de que esté desactivado al principio
        keySpaceObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        animatorKey = keySpaceObject.GetComponentInChildren<Animator>();
        if (collision.CompareTag("Player"))
        {
            keySpaceObject.SetActive(true);
            animatorKey.SetBool("Enter", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        keySpaceObject.SetActive(false);
        CheckGround.isGround = true;
    }

}