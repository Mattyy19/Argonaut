using UnityEngine;

public class PistolPenguin : Enemy
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;

    protected override void Attack()
    {
        if (bulletPrefab && firePoint)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            Vector3 spawnPos = firePoint.position + (Vector3)(dir * 1f);
            GameObject bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
            bullet.SetActive(true);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, angle);

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = dir * bulletSpeed;
            }
        }
    }
}
