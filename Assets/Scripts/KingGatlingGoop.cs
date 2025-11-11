using UnityEngine;

public class KingGatlingGoop : BossEnemy
{
    [Header("Gatling Settings")]
    public GameObject bulletPrefab;
    public Transform[] gunPoints;
    public float burstDuration = 2f;
    public float burstRate = 0.08f;

    [Header("Movement")]
    public float hopForce = 6f;
    public float hopCooldown = 1.5f;
    public float crawlSpeed = 2f;
    public float hopRange = 6f;

    [Header("Summon Settings")]
    public GameObject slimePrefab;
    public int summonCount = 3;

    private bool firing = false;
    private Animator animator;

    private Rigidbody2D rb;

    private float nextHopTime = 0f;
    private bool isGrounded = false;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    protected override void MoveTowardsPlayer()
    {
        float dist = Vector2.Distance(transform.position, player.position);

        //Hop if player is close
        if (dist <= attackDistance)
        {
            if (Time.time >= nextHopTime && isGrounded)
            {
                Vector2 dir = (player.position - transform.position).normalized;
                dir.y = Mathf.Abs(dir.y) + 0.6f;
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(dir * hopForce, ForceMode2D.Impulse);

                nextHopTime = Time.time + hopCooldown;
            }
        } 
        //Crawl if player is far
        else
        {
            Vector2 dir = (player.position - transform.position).normalized;
            dir.y = 0;
            rb.linearVelocity = new Vector2(dir.x * crawlSpeed, rb.linearVelocity.y);
        }

            if (spriteRenderer != null)
        {
            if (player.position.x < transform.position.x)
                spriteRenderer.flipX = false;   // facing left
            else
                spriteRenderer.flipX = true;  // facing right
        }
    }

    protected override void Attack()
    {
        if (!firing) { StartCoroutine(FireBurst()); }
    }

    private System.Collections.IEnumerator FireBurst()
    {
        firing = true;
        animator.SetBool("IsFiring", true);
        float t = 0f;

        while (t < burstDuration)
        {
            bool playerOnRight = player.position.x > transform.position.x;
            float sideDir = playerOnRight ? 1f : -1f;

            // Fire from each gun point
            foreach (Transform gp in gunPoints)
            {
                if (gp == null) continue;
                yield return new WaitForSeconds(0.02f);

                Vector2 dir = gp.right * sideDir;
                dir.Normalize();

                Vector3 spawnPos = gp.position + (Vector3)(dir * 4.4f);
                GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
                bullet.SetActive(true);

                Projectile proj = bullet.GetComponent<Projectile>();
                if (proj != null) { proj.SetDirection(dir); }
            }

            t += burstRate;
            yield return new WaitForSeconds(burstRate);
        }

        
        animator.SetBool("IsFiring", false);
        yield return new WaitForSeconds(6.0f);
        firing = false;
    }

    protected override void EnterNextPhase()
    {
        base.EnterNextPhase();

        // Spawn mini slimes
        for (int i = 0; i < summonCount; i++)
        {
            Vector3 pos = transform.position + (Vector3)(Random.insideUnitCircle * 1.5f);
            Instantiate(slimePrefab, pos, Quaternion.identity);
        }

        // Increase aggression
        burstRate *= 0.75f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
