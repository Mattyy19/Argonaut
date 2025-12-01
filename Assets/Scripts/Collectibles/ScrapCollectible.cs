using UnityEngine;

public class ScrapCollectible : MonoBehaviour
{
    public int scrapValue = 1;

    private int rotationSpeed = 45;

    private void OnEnable()
    {
        rotationSpeed = Random.Range(20, 90);

        // randomly choose the rotation direction
        if (Random.Range(0, 2) == 0)
        {
            rotationSpeed = -rotationSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.Play(AudioManager.SoundType.Scrap_Collected);
            Scrap scrapManager = other.GetComponent<Scrap>();
            if (scrapManager != null)
            {
                scrapManager.AddScrap(scrapValue);
            }

            PersistentInfo.IncreaseScrapCount(scrapValue);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime, 0f, 0f, Space.Self);
    }

}
