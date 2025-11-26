using UnityEngine;

public class ScimitarSlime : Enemy
{
    public float lungeForce = 5f;
    public float lungeDuration = 0.25f;
    public int damage = 15;

    protected override void Attack()
    {
        StartCoroutine(LungeAttack());
    }

    private System.Collections.IEnumerator LungeAttack()
    {
        AudioManager.Instance.Play(AudioManager.SoundType.ScimSl_Attack);
        Vector2 start = transform.position;
        Vector2 target = player.position;
        Vector2 dir = (target - start).normalized;

        float t = 0f;

        while (t < lungeDuration)
        {
            float progress = t / lungeDuration;

            Vector2 horizontal = Vector2.Lerp(start, target, progress);

            float height = Mathf.Sin(progress * Mathf.PI) * lungeForce * 0.3f;

            transform.position = new Vector2(horizontal.x, horizontal.y + height);


            t += Time.deltaTime;
            yield return null;
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 0.8f);
        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                AudioManager.Instance.Play(AudioManager.SoundType.ScimSl_Squish);
                hit.GetComponent<Health>()?.TakeDamage(damage);
            }
        }
    }
}
