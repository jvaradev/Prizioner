using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private float checkPointPositionX, checkPointPositionY; // Variables para almacenar la posición del punto de control
    private MovimientoJugador movimientoJugador; // Referencia al script MovimientoJugador

    public Animator animator; // Referencia al componente Animator

    // Start is called before the first frame update
    void Start()
    {
        // Inicializa la referencia al script MovimientoJugador
        movimientoJugador = GetComponent<MovimientoJugador>(); 

        // Si hay una posición de punto de control guardada en PlayerPrefs, mueve al jugador a esa posición
        if (PlayerPrefs.GetFloat("checkPointPositionX") != 0)
        {
            transform.position = new Vector2(PlayerPrefs.GetFloat("checkPointPositionX"), PlayerPrefs.GetFloat("checkPointPositionY"));
        }
    }

    // Método para actualizar la posición del punto de control
    public void ReachedCheckPoint(float x, float y)
    {
        // Guarda la nueva posición del punto de control en PlayerPrefs
        PlayerPrefs.SetFloat("checkPointPositionX", x);
        PlayerPrefs.SetFloat("checkPointPositionY", y);
    }

    // Método para manejar el daño al jugador
    public void PlayerDamaged()
    {
        // Desactiva el movimiento del jugador y reproduce la animación de "Hit"
        movimientoJugador.sePuedeMover = false;
        animator.Play("Hit");
    }
}