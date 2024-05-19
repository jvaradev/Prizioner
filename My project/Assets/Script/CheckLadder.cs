using UnityEngine;

//Codigo para verificar si el jugador se topa con una escalera.
    public class CheckLadder : MonoBehaviour
    {
        private Animator animatorPlayer;
        public static bool checkLadder;

        //Verificar si el jugador entra en contacto con un Trigger
        private void OnTriggerStay2D(Collider2D collision)
        {
            animatorPlayer = GetComponent<Animator>();
            //Si el Trigger tiene el Tag "Escalera"
            if (collision.CompareTag("Escalera"))
            {
                //Cambio de parametros para iniciar animación Climb
                animatorPlayer.SetBool("Climb", true);
                animatorPlayer.SetBool("Jump",false);
                animatorPlayer.SetBool("Fall",false);
                checkLadder = true;
            }
        }

        //Verificar si el jugador deja de estar en contacto con un Trigger
        private void OnTriggerExit2D(Collider2D collision)
        {
            //Cambio de parametros para terminar animación Climb
            animatorPlayer.SetBool("Climb", false);
            animatorPlayer.SetBool("IdleClimb", false);
            checkLadder = false;
            CheckGround.isGround = true;
        }
    }