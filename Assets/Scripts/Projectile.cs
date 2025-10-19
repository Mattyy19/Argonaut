using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 10f;
    public float lifetime = 3f; //How long it will be in the air
    public float decay = 0f; //How long after hiting will it be alive
    public int damage = 10;
    public string targetTag = "Player"; //Player or Enemy
    public bool gravityAffected = false;
    public float gravityModifier = 10f;

    private Vector2 direction;
    private Rigidbody2D rb;

    void Start()
    {
        Destroy(gameObject, lifetime);

        rb = GetComponent<Rigidbody2D>();

        // Creates a rigid body if one does not exist
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        rb.gravityScale = gravityAffected ? gravityModifier : 0f;

    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            Health health = other.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
            Destroy(gameObject, decay);
        }
        else if (!other.isTrigger)
        {
            Destroy(gameObject, decay);
        }
    }
}
