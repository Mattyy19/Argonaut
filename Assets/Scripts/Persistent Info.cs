using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class PersistentInfo
{
    private static List<string> sceneOrder = new List<string> { "StartScene", "1st Cave", "Ice Mountain", "Slime Forest", "Desert" };
    public static int scrapCount = 0;

    public static void LoadNextScene() {
        string currentScene = SceneManager.GetActiveScene().name;
        int index = sceneOrder.IndexOf(currentScene);
        if (index == -1)
        {
            Debug.LogError("Current scene not in sceneOrder!");
            return;
        }

        if (index >= sceneOrder.Count - 1)
        {
            SceneManager.LoadScene("Main Menu");
            return;
        }

        string nextScene = sceneOrder[index + 1];
        SceneManager.LoadScene(nextScene);
    }

    public static void IncreaseScrapCount(int count)
    {
        scrapCount += count;
    }
}
