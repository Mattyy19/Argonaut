using UnityEngine;

public class SawbladeSnake : Enemy
{
    public float spinForce = 5f;
    public float spinDuration = 0.25f;
    public float spinSpeed = 1.0f;
    public int damage = 15;

    public BoxCollider2D movementCollider;
    public CircleCollider2D attackCollider;

    private bool isAttacking = false;
    private Animator animator;

    private Rigidbody2D rb;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        movementCollider.enabled = true;
        attackCollider.enabled = false;
    }

    protected override void Attack()
    {
        StartCoroutine(SpinAttack());
    }

    protected override void MoveTowardsPlayer()
    {
        if(!isAttacking)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
        }

        if (spriteRenderer != null)
        {
            if (player.position.x < transform.position.x)
                spriteRenderer.flipX = !flipAnimation;   // facing left
            else
                spriteRenderer.flipX = flipAnimation;  // facing right
        }
    }

    private System.Collections.IEnumerator SpinAttack()
    {
        AudioManager.Instance.Play(AudioManager.SoundType.SawSnk_Hiss);
        isAttacking = true;
        animator.SetBool("isAttacking", true);
        movementCollider.enabled = false;
        attackCollider.enabled = true;

        yield return new WaitForSeconds(0.5f);

        float t = 0f;

        AudioManager.Instance.Play(AudioManager.SoundType.SawSnk_Roll);
        while (t < spinDuration)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * spinForce;

            float spinDirection = direction.x >= 0 ? -1f : 1f;

            transform.Rotate(0, 0, -720f * spinDirection * spinSpeed * Time.deltaTime);

            t += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;

        float r = 0f;
        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.Euler(0, 0, 0);

        while (r < 1f)
        {
            transform.rotation = Quaternion.Slerp(startRot, endRot, r);
            r += Time.deltaTime * 6f;
            yield return null;
        }

        movementCollider.enabled = true;
        attackCollider.enabled = false;

        animator.SetBool("isAttacking", false);
        Debug.Log("Done attacking");
        isAttacking = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isAttacking && collision.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.Play(AudioManager.SoundType.SawSnk_Hit);
            collision.gameObject.GetComponent<Health>()?.TakeDamage(damage);
        }
    }

}