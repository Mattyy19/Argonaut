using UnityEngine;
using System.Collections.Generic;

public class ThunderingTriops : BossEnemy
{
    [Header("Movement")]
    public float orbitRadius = 4f;
    public float orbitSpeed = 1.2f;
    public bool clockwise = true;
    public float verticalBobAmount = 0.35f;
    public float verticalBobSpeed = 2f;
    private float orbitAngle = 0f;


    [Header("Summon Clouds")]
    public GameObject stormCloud;
    public float summonChance = 0.25f;
    public Transform[] summonPositions;
    private List<CloudEntry> clouds = new List<CloudEntry>();


    [Header("Summon Lightning")]
    public GameObject lightning;
    public Transform firePoint;

    private Rigidbody2D rb;

    private bool isAttacking = false;
    private bool isSummoning = false;
    private Animator animator;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    //This enemy can fly
    protected override void MoveTowardsPlayer()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (spriteRenderer != null)
        {
            if (player.position.x < transform.position.x)
                spriteRenderer.flipX = !flipAnimation;   // facing left
            else
                spriteRenderer.flipX = flipAnimation;  // facing right
        }

        if (dist < detectionRange)
        {

            float direction = clockwise ? 1 : -1;
            orbitAngle += orbitSpeed * direction * Time.deltaTime;

            // Orbits player
            Vector2 orbitOffset = new Vector2(
                Mathf.Cos(orbitAngle),
                Mathf.Sin(orbitAngle)
            ) * orbitRadius;

            // Adds vertical up and down movement
            orbitOffset.y += Mathf.Sin(Time.time * verticalBobSpeed) * verticalBobAmount;

            Vector2 targetPos = (Vector2)player.position + orbitOffset;
            Vector2 toTarget = targetPos - (Vector2)transform.position;

            rb.linearVelocity = toTarget.normalized * moveSpeed;
        }
    }

    protected override void Attack()
    {
        if (!isAttacking && !isSummoning) { StartCoroutine(ThunderOrLightning()); }
    }

    private System.Collections.IEnumerator ThunderOrLightning()
    {
        isAttacking = true;
        animator.SetBool("IsAttacking", true);
        AudioManager.Instance.Play(AudioManager.SoundType.ThundT_Charge);

        yield return new WaitForSeconds(0.5f);

        float roll = Random.value;

        if (roll < summonChance)
        {
            if(clouds != null)
            {
                if(clouds.Count >= summonPositions.Length - 1)
                {
                    ShootLightning();
                } else
                {
                    SummonCloud();
                }
            } else
            {
                SummonCloud();
            }
        }
        else
        {
            ShootLightning();
        }

        isAttacking = false;
        isSummoning = false;
        animator.SetBool("IsAttacking", false);
    }

    private void ShootLightning()
    {
        AudioManager.Instance.Play(AudioManager.SoundType.ThundT_Lightning);
        Vector2 dir = (player.position - firePoint.position).normalized;
        Vector3 firePos = firePoint.position + (Vector3)(dir * 10f);
        GameObject lightningBolt = Instantiate(lightning, firePos, Quaternion.identity);

        lightningBolt.SetActive(true);

        Projectile proj = lightningBolt.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.SetDirection(dir);
        }

    }

    private void SummonCloud()
    {
        //Removes clouds that have despawned
        clouds.RemoveAll(c => c.cloud == null);
        AudioManager.Instance.Play(AudioManager.SoundType.ThundT_Summon);

        List<int> availablePositions = new List<int>();
        for (int i = 0; i < summonPositions.Length; i++)
        {
            bool occupied = clouds.Exists(c => c.positionIndex == i);
            if (!occupied) availablePositions.Add(i);
        }

        if (availablePositions.Count == 0)
        {
            ShootLightning();
            return;
        }

        int index = availablePositions[Random.Range(0, availablePositions.Count)];
        Transform spawnPos = summonPositions[index];

        GameObject newCloud = Instantiate(stormCloud, spawnPos.position, Quaternion.identity);
        clouds.Add(new CloudEntry(newCloud, index));

        newCloud.SetActive(true);

        isSummoning = true;
        animator.SetBool("IsSummoning", true);
    }

}

public class CloudEntry
{
    public GameObject cloud;
    public int positionIndex;

    public CloudEntry(GameObject cloud, int index)
    {
        this.cloud = cloud;
        this.positionIndex = index;
    }
}