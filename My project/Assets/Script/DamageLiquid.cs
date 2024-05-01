using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageLiquid : MonoBehaviour
{
    [SerializeField] private float damage = 10f; // Daño a infligir al jugador
    private bool isWaiting = false; // Variable para controlar si se está esperando
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
    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator ApplyDamageRepeatedly(CombatPlayer player)
    {
        while (isPlayerInContact)
        {
            isWaiting = true; // Marcamos que estamos esperando
            player.GetDamage(damage); // Aplicamos el daño al jugador
            player.transform.GetComponent<PlayerRespawn>().PlayerDamaged();
            yield return new WaitForSeconds(1f); // Esperamos un segundo
            isWaiting = false; // Marcamos que ya no estamos esperando
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