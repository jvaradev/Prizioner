using UnityEngine;

namespace Script
{
    public class CheckLadder : MonoBehaviour
    {
        private Animator animatorPlayer;
        public static bool checkLadder;

        private void OnTriggerStay2D(Collider2D collision)
        {
            animatorPlayer = GetComponent<Animator>();
            if (collision.CompareTag("Escalera"))
            {
                animatorPlayer.SetBool("Climb", true);
                animatorPlayer.SetBool("Jump",false);
                animatorPlayer.SetBool("Fall",false);
                checkLadder = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            animatorPlayer.SetBool("Climb", false);
            checkLadder = false;
            CheckGround.isGround = true;
        }
    }
}