using System;
using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;

public class CombatMelee : MonoBehaviour
{
    [SerializeField] private Transform controller;
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

        foreach (Collider2D collider in objets)
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
    }
}
