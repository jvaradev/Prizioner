using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private float checkPointPositionX, checkPointPoisitionY;
    private MovimientoJugador movimientoJugador;

    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        movimientoJugador = GetComponent<MovimientoJugador>();
        if (PlayerPrefs.GetFloat("checkPointPositionX")!=0)
        {
            transform.position =
                (new Vector2(PlayerPrefs.GetFloat("checkPointPositionX"), PlayerPrefs.GetFloat("checkPointPoisitionY")));
        }
    }
    

    public void ReachedCheckPoint(float x, float y)
    {
        PlayerPrefs.SetFloat("checkPointPositionX", x);
        PlayerPrefs.SetFloat("checkPointPositionY", y);

    }

    public void PlayerDamaged()
    {
        movimientoJugador.sePuedeMover = false;
        animator.Play("Hit");
    }
    
}