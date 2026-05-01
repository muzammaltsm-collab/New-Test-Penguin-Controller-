using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
public class GemsUpdater : MonoBehaviour
{


    public static GemsUpdater Instance;
    private long currentGems = 0; // Using long for large numbers
    //public Text amountShowText; // Reference to the UI Text component to display coins
    public Text amountShowText; // Reference to the UI Text component to display coins
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("FirstTimePlaying", 1) == 1)
        {
            AddGems(1500);
            PlayerPrefs.SetInt("FirstTimePlaying", 0); // Mark that the player has played for the first time
        }
        LoadGems(); // Load Gems from saved data or initialize
        UpdateUI();
    }
    private void Awake()
    {
        if (Instance != null)
        {
            Instance = this;
        }
    }


    // Add Gems to the player's balance
    public void AddGems(long amount)
    {
        currentGems += amount;
        SaveGems(); // Save updated Gems to storage
        UpdateUI(); // Update UI with the new coin balance
        //Debug.Log("Added " + FormatGems(amount) + " Gems. Total Gems: " + FormatGems(currentGems));
    }

    // Deduct Gems from the player's balance
    public void DeductGems(long amount)
    {
        if (currentGems >= amount)
        {
            currentGems -= amount;
            UpdateUI(); // Update UI with the new coin balance
            SaveGems(); // Save updated Gems to storage
            Debug.Log("Deducted " + FormatGems(amount) + " Gems. Total Gems: " + FormatGems(currentGems));
        }
        else
        {
            Debug.LogWarning("Not enough Gems to deduct " + FormatGems(amount));
        }
    }

    // Format the Gem amount into thousands, millions, or billions
    private string FormatGems(long Gems)
    {
        if (Gems >= 1000000000)
        {
            return (Gems / 1000000000f).ToString("0.#") + "B";
        }
        else if (Gems >= 1000000)
        {
            return (Gems / 1000000f).ToString("0.#") + "M";
        }
        else if (Gems >= 1000)
        {
            return (Gems / 1000f).ToString("0.#") + "K";
        }

        else
        {
            return Gems.ToString();
        }
    }

    // Save current Gems to storage (PlayerPrefs in this example)
    private void SaveGems()
    {
        PlayerPrefs.SetString("PlayerGems", currentGems.ToString());
        PlayerPrefs.Save(); // Save changes immediately
    }

    // Load saved Gems from storage
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
}
