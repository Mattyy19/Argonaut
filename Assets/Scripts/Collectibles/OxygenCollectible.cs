using UnityEngine;

public class OxygenCollectible : MonoBehaviour
{
    public int oxygenLevel = 2;
    
    private int rotationSpeed = 45;

    private void OnEnable()
    {
        rotationSpeed = Random.Range(45, 181);

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
            AudioManager.Instance.Play(AudioManager.SoundType.Oxygen_Collected);
            Oxygen oxygen = other.GetComponent<Oxygen>();
            if (oxygen != null)
            {
                oxygen.increaseOxygen(oxygenLevel);
            }

            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }

}
