using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatPlayer : MonoBehaviour
{
    private float health;
    private float maxHealth = 100;
    [SerializeField]private HealthBar healthBar;
    
    // Start is called before the first frame update
    private void Start()
    {
        health = maxHealth;
        healthBar.inicialiteActualHealth(health);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetDamage(float damage)
    {
        health -= damage;
        healthBar.changeActualHealth(health);
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
