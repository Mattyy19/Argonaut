using UnityEngine;
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

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        playerHealth = player.GetComponent<Health>();
        playerOxygen = player.GetComponent<Oxygen>();
        playerScrap = player.GetComponent<Scrap>();

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
            healthDisplay.text = "Health: " + playerHealth.currentHealth;
        }

        if (playerOxygen != null)
        {
            oxygenDisplay.text = "Oxygen: " + playerOxygen.currentOxygen;
        }

        if (playerScrap != null)
        {
            scrapDisplay.text = "Scrap: " + playerScrap.scrapCount;
        }
    }
}
