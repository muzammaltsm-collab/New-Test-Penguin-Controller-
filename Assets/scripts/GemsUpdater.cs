using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GemsUpdater : MonoBehaviour
{
    public static GemsUpdater Instance;
    private long currentGems = 0;
    public Text amountShowText;
    private CoinUIController coinAnimation; // Reference to animation script

    void Start()
    {
        coinAnimation = GetComponent<CoinUIController>(); // Auto-find animation script

        if (PlayerPrefs.GetInt("FirstTimePlaying", 1) == 1)
        {
            AddGems(1500);
            PlayerPrefs.SetInt("FirstTimePlaying", 0);
        }
        LoadGems();
        UpdateUI();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void AddGems(long amount)
    {
        currentGems += amount;
        SaveGems();
        UpdateUI();
        PlayAnimation(); // Play pop animation
    }

    public void DeductGems(long amount)
    {
        if (currentGems >= amount)
        {
            currentGems -= amount;
            UpdateUI();
            SaveGems();
            PlayAnimation(); // Play pop animation
            Debug.Log("Deducted " + FormatGems(amount) + " Gems. Total Gems: " + FormatGems(currentGems));
        }
        else
        {
            Debug.LogWarning("Not enough Gems to deduct " + FormatGems(amount));
        }
    }

    private string FormatGems(long Gems)
    {
        if (Gems >= 1000000000)
            return (Gems / 1000000000f).ToString("0.#") + "B";
        else if (Gems >= 1000000)
            return (Gems / 1000000f).ToString("0.#") + "M";
        else if (Gems >= 1000)
            return (Gems / 1000f).ToString("0.#") + "K";
        else
            return Gems.ToString();
    }

    private void SaveGems()
    {
        PlayerPrefs.SetString("PlayerGems", currentGems.ToString());
        PlayerPrefs.Save();
    }

    private void LoadGems()
    {
        if (PlayerPrefs.HasKey("PlayerGems"))
        {
            currentGems = long.Parse(PlayerPrefs.GetString("PlayerGems"));
            Debug.Log("Loaded Gems: " + FormatGems(currentGems));
        }
        else
        {
            Debug.Log("No saved Gems found. Initializing to zero.");
        }
    }

    private void UpdateUI()
    {
        if (amountShowText != null)
        {
            amountShowText.text = FormatGems(currentGems);
        }
    }

    private void PlayAnimation()
    {
        if (coinAnimation != null)
        {
            coinAnimation.UpdateCoinAmount((int)currentGems); // Trigger the pop animation
        }
    }
}
