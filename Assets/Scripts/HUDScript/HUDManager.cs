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
            healthDisplay.text = playerHealth.currentHealth.ToString();
        }

        if (playerOxygen != null)
        {
            oxygenDisplay.text = playerOxygen.currentOxygen.ToString();
        }

        if (playerScrap != null)
        {
            scrapDisplay.text = playerScrap.scrapCount.ToString();
        }
    }
}
