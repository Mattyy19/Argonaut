using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject HUDUI; // Assigned per scene
    public GameObject pauseMenuUI;

    public static bool GameIsPaused;

    void Start()
    {
        // Make sure it's off at start
        pauseMenuUI.SetActive(false);
        GameIsPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused) Resume();
            else Pause();
        }
    }

    public void Resume()
    {
        Debug.Log("Resume Clicked");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void QuitToMenu()
    {
        Debug.Log("Quit Clicked");
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
        HUDUI.SetActive(false);
    }
}
