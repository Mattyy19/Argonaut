using UnityEngine;
using System;


public class Health : MonoBehaviour
{
    [Header("Health Options")]
    public float maxHealth = 100f;
    public float currentHealth;


    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0) { Death(); }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    private void Death()
    {
        Destroy(gameObject, 0.5f);
    }
}
