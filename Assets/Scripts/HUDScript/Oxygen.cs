using UnityEngine;
using System.Collections;

public class Oxygen : MonoBehaviour
{
    public int maxOxygen;
    public int currentOxygen;
    public GameObject player;
    private Health playerHealth;
    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerHealth = player.GetComponent<Health>();
        currentOxygen = maxOxygen;
    }

    void Start()
    {
        StartCoroutine(DecreaseCurrentOxygenOverTime());
    }

    public void increaseOxygen(int amount)
    {
        int newAmount = currentOxygen += amount;
        if (newAmount > maxOxygen)
        {
            currentOxygen = maxOxygen;
        }
        else
        {
            currentOxygen += amount;
        }
    }

    private IEnumerator DecreaseCurrentOxygenOverTime()
    {
        while (true) // loop forever
        {
            yield return new WaitForSeconds(5f); // wait 5 seconds
            currentOxygen = Mathf.Max(0, currentOxygen - 1); // decrease oxygen but don’t go below 0
            if (currentOxygen <= 0)
            {
                playerHealth.Death();
            }
        }
    }
}
