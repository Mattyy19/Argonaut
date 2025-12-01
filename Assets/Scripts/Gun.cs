using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Projectile")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;

    [Header("Ammo")]
    public int mag = 12;
    public float reloadTime = 5f;

    [Header("Rate")]
    public float fireRate = 6f;

    private int currMag;
    private float fireCooldown = 0f;
    private bool isReloading = false;
    protected SpriteRenderer spriteRenderer;

    void Start()
    {
        currMag = mag;
        fireCooldown = 0f;
        spriteRenderer = GetComponentInParent<SpriteRenderer>();
    }


    void Update()
    {
        fireCooldown -= Time.deltaTime; 
        if(isReloading)
        {
            return;
        }

        bool fireInput = Input.GetMouseButton(0);
        if (fireInput)
        {
            shoot();
        }
        if (Input.GetKey(KeyCode.R))
        {
            StartCoroutine(reload());
        }
    }

    void shoot()
    {
        // Throttles fire rate
        if (fireCooldown > 0f)
        {
            return;
        }
        if (currMag <= 0)
        {
            StartCoroutine(reload());
            return;
        }

        // Shoots bullet
        if (bulletPrefab && firePoint)
        {
            AudioManager.Instance.Play(AudioManager.SoundType.Player_Shoots);
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            bullet.SetActive(true);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = firePoint.right * bulletSpeed;
                Debug.Log("Bullet shot");
            }
        }

        currMag--;
        fireCooldown = 1f / fireRate;
    }

    IEnumerator reload()
    {
        // If it's already relaoding
        if (isReloading)
        {
            yield break;
        }

        Debug.Log("reloading");
        isReloading = true;
        yield return new WaitForSeconds(reloadTime);

        currMag = mag;
        isReloading = false;
    }
}
