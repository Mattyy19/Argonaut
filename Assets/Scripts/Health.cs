using UnityEngine;
using System;


public class Health : MonoBehaviour
{
    [Header("Health Options")]
    public float maxHealth = 100f;
    public float currentHealth;


    public event Action OnTakeDamage;
    public String TakeDamageSound;
    public event Action OnHeal;
    public String OnHealSound;
    public event Action OnDeath;
    public String OnDeathSound;

    private Vector3 checkpointPos;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0) 
        {
            Death(); 
        } else if (OnTakeDamage != null)
        {
            AudioManager.Instance.Play(AudioManager.getSound(TakeDamageSound));
            OnTakeDamage.Invoke();
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        if (OnHeal != null) 
        {
            AudioManager.Instance.Play(AudioManager.getSound(OnHealSound));
            OnHeal.Invoke(); 
        }
    }

    public void Death()
    {
        if(OnDeath != null) 
        {
            AudioManager.Instance.Play(AudioManager.getSound(OnDeathSound));
            OnDeath.Invoke(); 
        }

        transform.position = checkpointPos;
        currentHealth = maxHealth;
        gameObject.GetComponent<Rigidbody2D>().linearVelocity = new Vector3(0, 0, 0);
        
        // destroy the player Destroy(gameObject, 0.5f);
    }

    public void setCheckpointPos(Vector3 pos)
    {
        checkpointPos = pos;
    }
}
