using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HUDManager : MonoBehaviour
{
    public TextMeshProUGUI healthDisplay;
    public TextMeshProUGUI oxygenDisplay;
    public TextMeshProUGUI scrapDisplay;

    public GameObject player;
    private Health playerHealth;
    private Oxygen playerOxygen;
    private Scrap playerScrap;

    void Awake()
    {
        var huds = FindObjectsByType<HUDManager>(FindObjectsSortMode.None);
        if (huds.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ReacquirePlayer();
    }

    private void ReacquirePlayer()
    {
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerHealth = player.GetComponent<Health>();
            playerOxygen = player.GetComponent<Oxygen>();
            playerScrap = player.GetComponent<Scrap>();
        }
    }

    void Start()
    {
        ReacquirePlayer();
        UpdateHUD();
    }

    void Update()
    {
        UpdateHUD();
    }

    private void UpdateHUD()
    {
        if (playerHealth != null)
        {
            healthDisplay.text = ((int)playerHealth.currentHealth).ToString();
        }

        if (playerOxygen != null)
        {
            oxygenDisplay.text = playerOxygen.currentOxygen.ToString();
        }

        if (playerScrap != null)
        {
            scrapDisplay.text = PersistentInfo.scrapCount.ToString();
        }
    }
}
