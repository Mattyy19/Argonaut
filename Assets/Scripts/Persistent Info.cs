using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class PersistentInfo
{
    private static List<string> sceneOrder = new List<string> { "StartScene", "1st Cave", "Ice Mountain", "Slime Forest", "Desert" };
    private static int currIndex = 0;
    public static int scrapCount = 0;

    public static void LoadNextScene() {
        if (currIndex >= sceneOrder.Count) { return; }
        currIndex++;
        SceneManager.LoadScene(sceneOrder[currIndex]);
    }

    public static void IncreaseScrapCount(int count)
    {
        scrapCount += count;
    }
}
