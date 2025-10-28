using UnityEngine;

[RequireComponent(typeof(Health))]
public abstract class Enemy : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float moveSpeed = 2f;
    public float detectionRange = 6f;
    public float attackDistance = 2f;
    public float attackCooldown = 1.5f;
    private float attackTimer;

    protected Transform player;
    protected SpriteRenderer spriteRenderer;
    private Health health;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        health = GetComponent<Health>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= attackDistance && attackTimer <= 0)
        {
            Attack();
            attackTimer = attackCooldown;
        }

        else if (dist <= detectionRange)
        {
            MoveTowardsPlayer();
        }

        if (attackTimer > 0) { attackTimer -= Time.deltaTime;  }
    }

    protected virtual void MoveTowardsPlayer()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        transform.Translate(dir * moveSpeed * Time.deltaTime);

        if (spriteRenderer != null)
        {
            if (player.position.x < transform.position.x)
                spriteRenderer.flipX = true;   // facing left
            else
                spriteRenderer.flipX = false;  // facing right
        }
    }

    protected abstract void Attack();
}