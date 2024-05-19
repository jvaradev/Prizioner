using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Codigo para el MainMenu del juego
public class MainMenu : MonoBehaviour
{
    //New Game carga la siguiente escena
    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //Exit cierra el juego.
    public void Exit()
    {
        Application.Quit();
    }
}
