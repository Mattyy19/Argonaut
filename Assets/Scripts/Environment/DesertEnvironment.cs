using System.Collections;
using UnityEngine;

public class DesertEnvironment : MonoBehaviour
{
    private bool safe;
    private float time;
    private Health health;

    public GameObject player;
    public float sunDamage = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = player.GetComponent<Health>();
        StartCoroutine(DelayedSunCycle());
    }

    private IEnumerator DelayedSunCycle()
    {
        yield return new WaitForFixedUpdate();
        StartCoroutine(SunCycle());
    }

    private IEnumerator SunCycle()
    {
        while (true)
        {
            //Random interval until player takes damage from sun
            float waitTime = Random.Range(5f, 20f);
            Debug.Log($"{waitTime}s until the harsh sun");
            yield return new WaitForSeconds(waitTime);

            if (!safe)
            {
               yield return StartCoroutine(DamageFromSun());
            }
        }
    }

    private IEnumerator DamageFromSun()
    {
        time = Random.Range(5f, 15f);
        float elapsed = 0f;
        Debug.Log($"Harsh sun for {time}s");

        //Player takes 10 damage a second until they get to shade or to boss area or harsh sun stops
        while (elapsed < time)
        {
            if (!safe)
            {
                health.TakeDamage(sunDamage * Time.deltaTime);
                Debug.Log("Took damage from the sun");
            }

            elapsed += Time.deltaTime;

            yield return null;
        }

        Debug.Log("Harsh sun over");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        safe = true;
        //Debug.Log("Player entered safe zone");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        safe = false;
        //Debug.Log("Player left safe zone");
    }

    public void setSafe(bool safe)
    {
        this.safe = safe;
    }
}
