using UnityEngine;

public class BattingBat : Enemy
{
    public float attackRadius = 1.5f;
    public int damage = 20;

    protected override void Attack()
    {
        // swing animation could go here
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRadius);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                // assuming player has Health
                hit.GetComponent<Health>()?.TakeDamage(damage);
            }
        }
    }
}
