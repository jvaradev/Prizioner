using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMelee : MonoBehaviour
{
    [SerializeField] private Transform controller;
    [SerializeField] private Transform controller2;
    [SerializeField] private float radioHit;
    [SerializeField] private float damage;
    private Rigidbody2D rb2D;
    public Animator animator;
    private bool isAttacking = false;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isAttacking && Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Punch");
            animator.SetBool("Run",false);
            isAttacking = true; // El personaje está atacando
            StartCoroutine(PerformAttack());
        }
    }

    private IEnumerator PerformAttack()
    {
        Punch(); // Realizar el ataque

        // Congelar la posición durante el ataque
        rb2D.constraints = RigidbodyConstraints2D.FreezePosition;

        // Esperar hasta que termine la animación de ataque
        yield return new WaitForSeconds(0f);

        // Descongelar la posición después del ataque
        rb2D.constraints = RigidbodyConstraints2D.None;
        rb2D.constraints = RigidbodyConstraints2D.FreezeRotation;

        isAttacking = false; // El personaje ha terminado de atacar
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
