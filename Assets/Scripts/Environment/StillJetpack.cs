using UnityEngine;

public class StillJetpack : MonoBehaviour
{
    private float bobSpeed = 2f;         
    private float bobRange = 0.75f;    
    private float baseY;                 

    private void Start()
    {
        baseY = transform.position.y;
    }

    private void Update()
    {
        float newY = baseY + Mathf.Sin(Time.time * bobSpeed) * (bobRange / 2f);

        transform.position = new Vector3(
            transform.position.x,
            newY,
            transform.position.z
        );

        transform.Rotate(0f, 145f * Time.deltaTime, 0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement2D>().acquired = true;
            Destroy(gameObject);
        }
    }

}
