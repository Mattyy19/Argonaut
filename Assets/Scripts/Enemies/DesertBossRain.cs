using UnityEngine;

public class DesertBossRain : MonoBehaviour
{
    //Use for rain environment
    private bool rain;
    private bool isOver;

    public GameObject shade;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rain = false;
        isOver = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        rain = true;
        isOver = false;
        shade.GetComponent<DesertEnvironment>().setSafe(true);
        Debug.Log("Player entered boss area");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && !isOver)
        {
            return;
        }

        rain = false;
        isOver = true;
        shade.GetComponent<DesertEnvironment>().setSafe(false);
        Debug.Log("Player left boss area");
    }
}
