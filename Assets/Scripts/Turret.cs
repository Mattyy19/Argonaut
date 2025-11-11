using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("Turret Settings")]
    public GameObject projectilePrefab;
    public Transform[] firePoints;
    public float fireRate = 1f;
    public bool temporary = true;
    public float timeToDecay = 10f;

    [Header("Fire Modes")]
    public bool targetPlayer = true;
    public float fixedAngle = 0f;
    public bool randomFire = true;

    private Transform player;
    private float fireTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        fireTimer = 1f / fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        timeToDecay -= Time.deltaTime;
        fireTimer -= Time.deltaTime;
        if (fireTimer <= 0f)
        {
            Fire();
            fireTimer = 1f / fireRate;
        }
        if(timeToDecay <= 0f)
        {
            Destroy(gameObject);
        }
    }

    void Fire()
    {
        if(firePoints.Length == 0) { return;  }

        if(randomFire)
        {
            Transform fp = firePoints[Random.Range(0, firePoints.Length)];
            FireFromPoint(fp);
        } else
        {
            foreach (Transform fp in firePoints)
            {
                FireFromPoint(fp);
            }
        }
    }

    void FireFromPoint(Transform fp)
    {
        Vector2 direction = Vector2.right;

        if (targetPlayer && player != null)
        {
            direction = (player.position - fp.position).normalized;
        }
        else
        {
            float rad = fixedAngle * Mathf.Deg2Rad;
            direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        }

        GameObject projectile = Instantiate(projectilePrefab, fp.position, Quaternion.identity);

        projectile.SetActive(true);

        Projectile proj = projectile.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.SetDirection(direction);
        }
    }
}
