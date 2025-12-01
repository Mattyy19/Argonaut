using UnityEngine;
using System.Collections.Generic;

public class ShieldingSnail : BossEnemy
{
    [Header("Shield Settings")]
    public float shieldDuration = 5.0f;
	public float shieldCooldown = 4.0f;

	private Animator animator;

	//[Header("Animation Collider collections")]
	//public AnimationCollider[] animationColliders;
	private PolygonCollider2D poly;
	private Sprite lastSprite;

	private bool isShielding = false;
	private float shieldTimer = 0f;
	private float cooldownTimer = 0f;

	public enum ColliderType
	{
		Body,   // can take damage
		Shell   // blocks damage
	}

	protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();


		if (poly == null)
			poly = gameObject.AddComponent<PolygonCollider2D>();

		UpdateCollider();
	}

	protected override void Update()
	{
		base.Update();

		if (isShielding)
		{
			shieldTimer += Time.deltaTime;
			if (shieldTimer >= shieldDuration)
			{
				ExitShield();
			}
		}
		else
		{
			cooldownTimer += Time.deltaTime;
		}
	}

	void Awake()
	{
		poly = GetComponent<PolygonCollider2D>();
	}

	protected override void Attack()
    {
		if (!isShielding)
			EnterShield();
	}

	void LateUpdate()
	{
		if (spriteRenderer.sprite != lastSprite)
			UpdateCollider();
	}


	private void EnterShield()
	{
		health.invulnerable = true;
		health.TakeDamageSound = "ShiSnl_Deflect";
		isShielding = true;
		AudioManager.Instance.Play(AudioManager.SoundType.ShiSnl_Guard);
		shieldTimer = 0f;
		animator.SetBool("isShielding", isShielding);
	}

	private void ExitShield()
	{
		health.invulnerable = false;
		health.TakeDamageSound = "ShiSnl_Hurt";
		isShielding = false;
		cooldownTimer = 0f;
		animator.SetBool("isShielding", isShielding);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.CompareTag("Player"))
		{
			// snail’s only “attack”
			collision.collider.GetComponent<Health>()?.TakeDamage(999999999f);
		}
	}

		bool IsPointInPolygon(Vector2[] polyPoints, Vector2 p)
	{
		bool inside = false;
		int j = polyPoints.Length - 1;

		for (int i = 0; i < polyPoints.Length; i++)
		{
			if (((polyPoints[i].y > p.y) != (polyPoints[j].y > p.y)) &&
				 (p.x < (polyPoints[j].x - polyPoints[i].x) *
				  (p.y - polyPoints[i].y) /
				  (polyPoints[j].y - polyPoints[i].y) + polyPoints[i].x))
			{
				inside = !inside;
			}
			j = i;
		}

		return inside;
	}

	void UpdateCollider()
	{
		// Remove old collider
		if (poly != null)
			Destroy(poly);

		// Create new one matching this frame
		poly = gameObject.AddComponent<PolygonCollider2D>();
		poly.isTrigger = true;

		lastSprite = spriteRenderer.sprite;
	}
}

