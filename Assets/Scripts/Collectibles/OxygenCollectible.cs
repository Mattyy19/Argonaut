using UnityEngine;

public class OxygenCollectible : MonoBehaviour
{
    public int oxygenLevel = 2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Oxygen oxygen = other.GetComponent<Oxygen>();
            if (oxygen != null)
            {
                oxygen.increaseOxygen(oxygenLevel);
            }

            Destroy(gameObject);
        }
    }
}
