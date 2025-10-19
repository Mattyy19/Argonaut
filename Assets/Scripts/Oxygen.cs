using UnityEngine;
using System.Collections;

public class Oxygen : MonoBehaviour
{
    public int maxOxygen = 20;
    public int currentOxygen;

    void Awake()
    {
        currentOxygen = maxOxygen;
    }

    void Start()
    {
        StartCoroutine(DecreaseCurrentOxygenOverTime());
    }

    private IEnumerator DecreaseCurrentOxygenOverTime()
    {
        while (true) // loop forever
        {
            yield return new WaitForSeconds(5f); // wait 5 seconds
            currentOxygen = Mathf.Max(0, currentOxygen - 1); // decrease oxygen but don’t go below 0
            if (currentOxygen <= 0)
            {
                Suffocate();
            }
        }
    }

    private void Suffocate()
    {
        Destroy(gameObject, 0.5f);
    }
}
