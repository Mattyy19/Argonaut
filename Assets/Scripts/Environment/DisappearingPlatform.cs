using UnityEngine;

public class DissapearingPlatform : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Disappear();
        }
    }

    private void Disappear()
    {
        gameObject.SetActive(false);
    }
}
