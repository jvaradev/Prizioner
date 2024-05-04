using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;

public class CombatMelee : MonoBehaviour
{
    [SerializeField] private Transform controller;
    [SerializeField] private Transform controller2;
    [SerializeField] private float radioHit;
    [SerializeField] private float damage;
    public Animator animator;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Punch");
            Punch();
        }
    }

    private void Punch()
    {
        Collider2D[] objets = Physics2D.OverlapCircleAll(controller.position, radioHit);
        Collider2D[] objets2 = Physics2D.OverlapCircleAll(controller2.position, radioHit);

        foreach (Collider2D collider in objets)
        {
            if (collider.CompareTag("Enemy"))
            {
                collider.transform.GetComponent<PatrullaEnemigo>().GetDamage(damage);
            }
        }
        foreach (Collider2D collider in objets2)
        {
            if (collider.CompareTag("Enemy"))
            {
                collider.transform.GetComponent<PatrullaEnemigo>().GetDamage(damage);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(controller.position, radioHit);
        Gizmos.DrawWireSphere(controller2.position, radioHit);
    }
}
