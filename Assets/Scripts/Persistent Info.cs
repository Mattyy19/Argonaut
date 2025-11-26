using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class PersistentInfo
{
    private static List<string> sceneOrder = new List<string> { "StartScene", "1st Cave" };
    private static int currIndex = 0;

    public static void LoadNextScene() {
        if (currIndex >= sceneOrder.Count) { return; }
        currIndex++;
        SceneManager.LoadScene(sceneOrder[currIndex]);
    }
}
