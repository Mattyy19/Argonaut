using UnityEngine;

public class ScrapCollectible : MonoBehaviour
{
    public int scrapValue = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Scrap scrapManager = other.GetComponent<Scrap>();
            if (scrapManager != null)
            {
                scrapManager.AddScrap(scrapValue);
            }

            Destroy(gameObject);
        }
    }
}
