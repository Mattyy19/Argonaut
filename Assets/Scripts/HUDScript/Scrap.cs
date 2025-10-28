using UnityEngine;

public class Scrap : MonoBehaviour
{
    public int scrapCount = 0;

    public void AddScrap(int amount)
    {
        scrapCount += amount;
    }
}
