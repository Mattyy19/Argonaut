using UnityEngine;

public class MorningstarMucus : BossEnemy
{
    [Header("Morningstar Mucus")]
    [Header("Roll Attack")]
    public float damageAmount = 10;
    public float spinSpeed = 1f;
    public float spinWindup = 0.7f;
    public float spinDuration = 1.2f;
    public float rollSpeed = 6f;
    public float rollDuration = 0.9f;
    public float resetTime = 0.3f;
    [Header("Goo Attack")]
    public GameObject gooPrefab;
    //public Transform gooPoint;
    public int gooOnHit = 3;
    public float gooSpread = 5f;
    public float gooCooldownTime = 0.5f;

    private bool spinning = false;
    private bool canShootGoo = true;
    private Rigidbody2D rb;

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        

        if(bossHealth != null)
        {
            bossHealth.OnTakeDamage += OnDamageAttack;
        }
    }
    
    protected override void Attack()
    {
        if(!spinning) { StartCoroutine(SpinAndRoll()); }
    }

    private System.Collections.IEnumerator SpinAndRoll()
    {
        spinning = true;
        float spinDir = (player.position.x < transform.position.x) ? 1f : -1f;
        Vector2 dir = (player.position - transform.position).normalized;

        yield return new WaitForSeconds(spinWindup);

        //Spin and Lunge
        StartCoroutine(Spin(spinDuration, spinDir));
        yield return StartCoroutine(Lunge(dir, rollSpeed, rollDuration));

        //Resets slime rotation
        yield return StartCoroutine(ResetRotation());

        spinning = false;
    }

    private System.Collections.IEnumerator Spin(float duration, float spinDir)
    {
        float t = 0f;
        while (t < duration)
        {
            transform.Rotate(Vector3.forward, 360f * spinSpeed * spinDir * Time.deltaTime);
            t += Time.deltaTime;
            yield return null;
        }
    }

    private System.Collections.IEnumerator Lunge(Vector2 dir, float speed, float duration)
    {
        rb.linearVelocity = dir * speed;
        yield return new WaitForSeconds(duration);
        rb.linearVelocity = Vector2.zero;
    }

    private System.Collections.IEnumerator ResetRotation()
    {
        Quaternion startRot = transform.rotation;
        Quaternion targetRot = Quaternion.identity;
        float t = 0f;

        while (t < resetTime)
        {
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t / resetTime);
            t += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRot;
    }


    //Throws out goo from the slime
    private void OnDamageAttack()
    {
        if (!canShootGoo) { return; }

        StartCoroutine(GooCooldown());

        //Get player position to fire in a cone
        Vector2 baseDir = (player.position - transform.position).normalized;

        float sprayAngle = gooSpread;
        float halfAngle = sprayAngle / 2f;

        for (int i = 0; i < gooOnHit; i++)
        {
            float angleOffset = Mathf.Lerp(-halfAngle, halfAngle, (float)i / (gooOnHit - 1));
            angleOffset += Random.Range(-5f, 5f);

            Vector2 dir = Quaternion.Euler(0, 0, angleOffset) * baseDir;

            dir.y += 0.2f;
            dir.Normalize();

            Vector3 spawnPos = transform.position + (Vector3)(dir * 1.2f);

            GameObject goo = Instantiate(gooPrefab, spawnPos, Quaternion.identity);
            goo.SetActive(true);

            goo.transform.right = dir;

            Projectile proj = goo.GetComponent<Projectile>();
            if (proj != null) { proj.SetDirection(dir); }
        }
    }

    //Stops goo from emitting for a time
    private System.Collections.IEnumerator GooCooldown()
    {
        canShootGoo = false;
        yield return new WaitForSeconds(gooCooldownTime);
        canShootGoo = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (spinning && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Health>()?.TakeDamage(damageAmount);
        }
    }
}
