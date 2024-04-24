using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantKill : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<CombatPlayer>().GetDamage(CombatPlayer.maxHealth, collision.GetContact(0).normal);
            collision.transform.GetComponent<PlayerRespawn>().PlayerDamaged();
        }
    }
}
