using UnityEngine;

public class StabbingSquid : Enemy
{
    public float dashSpeed = 8f;
    public float dashTime = 0.5f;
    public float retreatSpeed = 4f;
    public float retreatTime = 0.3f;
    public float damage = 5f;


    private Rigidbody2D rb;
    private bool dashing = false;
    private bool canAttack = true;
    private Vector2 dashDirection;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Attack()
    {
        if(canAttack && !dashing) { StartCoroutine(DashAttack()); }
    }

    private System.Collections.IEnumerator DashAttack()
    {
        dashing = true;
        canAttack = false;

        dashDirection = (player.position - transform.position).normalized;
        rb.linearVelocity = dashDirection * dashSpeed;

        yield return new WaitForSeconds(dashTime);

        rb.linearVelocity = Vector2.zero;

        dashing = false;

        float retreatElapsed = 0f;
        while (retreatElapsed < retreatTime)
        {
            Vector2 retreatDir = (transform.position - player.position).normalized;
            rb.linearVelocity = retreatDir * retreatSpeed;

            retreatElapsed += Time.deltaTime;
            yield return null;
        }

        rb.linearVelocity = Vector2.zero;

        canAttack = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && dashing)
        {
            Health health = other.gameObject.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }

            rb.linearVelocity = Vector2.zero;
            dashing = false;
        }
    }
}
