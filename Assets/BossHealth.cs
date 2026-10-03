using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour
{
   public int maxHealth = 1;
   public HealthBar healthBar;

    int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Panggil Game Manager untuk hancurkan gerbang
        if (GameManager.instance != null)
        {
            GameManager.instance.BossHasDied();
        }

        // Efek mati boss
        Destroy(gameObject);
    }
}
