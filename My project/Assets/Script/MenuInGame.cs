using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Código para el menú de pausa dentro del juego
public class MenuInGame : MonoBehaviour
{
    [SerializeField] private GameObject buttonPause;
    [SerializeField] private GameObject menuInGame;
    [SerializeField] private GameObject menuOptions;

    private bool gamePaused = false;

    //Si el jugador pulsa en Escape entra en el menú si no está dentro. Si está dentro sale al pulsarlo.
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    //Método para pausar el juego
    public void Pause()
    {
        gamePaused = true;
        Time.timeScale = 0;
        buttonPause.SetActive(false);
        menuInGame.SetActive(true);

    }

    //Método para volver al juego
    public void Resume()
    {
        gamePaused = false;
        Time.timeScale = 1f;
        buttonPause.SetActive(true);
        menuInGame.SetActive(false);
        menuOptions.SetActive(false);
    }

    //Método para reiniciar la escena.
    public void Restart()
    {
        gamePaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //Método para salir del juego
    public void Exit()
    {
        Application.Quit();
    }
}
