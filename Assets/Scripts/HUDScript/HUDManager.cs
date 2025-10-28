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
    private int scrap = 0;

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

        // TODO: Update scrap display when collectibles are added
        scrapDisplay.text = "Scrap: " + scrap;
    }
}
