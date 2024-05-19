using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Código para líquidos que infligen daño
public class DamageLiquid : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    private bool isWaiting = false; 
    private bool isPlayerInContact = false;

    // Cuando otro collider permanece en contacto con este collider...
    private void OnCollisionStay2D(Collision2D collision)
    {
        isPlayerInContact = true;
        // Verificamos si el collider pertenece al jugador y si no estamos esperando ya
        if (collision.gameObject.CompareTag("Player") && !isWaiting && isPlayerInContact)
        {
            // Accedemos al script CombatPlayer del jugador y le aplicamos el daño
            CombatPlayer combatPlayer = collision.gameObject.GetComponent<CombatPlayer>();
            if (combatPlayer != null)
            {
                StartCoroutine(ApplyDamageRepeatedly(combatPlayer)); // Llamamos a una corutina para aplicar daño repetidamente
            }
        }
    }

    // Método para aplicar el daño repetidamente mientras esté en contacto
    private IEnumerator ApplyDamageRepeatedly(CombatPlayer player)
    {
        while (isPlayerInContact)
        {
            isWaiting = true; 
            player.GetDamage(damage);
            player.transform.GetComponent<PlayerRespawn>().PlayerDamaged();
            yield return new WaitForSeconds(1f); 
            isWaiting = false; 
        }
    }

    // Cuando otro collider deja de estar en contacto con este collider...
    private void OnCollisionExit2D(Collision2D collision)
    {
        CheckGround.isGround = false;
        // Verificamos si el collider pertenece al jugador
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerInContact = false;
        }
    }
}