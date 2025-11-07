using UnityEngine;
using TMPro; // We need this line to work with TextMeshPro

public class GameManager : MonoBehaviour
{
    // This is a special variable called a "singleton". It provides a global, public
    // access point to this specific script from anywhere in our code.
    public static GameManager instance;

    [Header("UI References")]
    public TextMeshProUGUI currencyText; // The text element we will update.

    [Header("Game Variables")]
    public int startingCurrency = 100;
    
    // This is a private variable to hold our current currency.
    private int currentCurrency;

    // Awake is called before Start(). It's the perfect place to set up singletons.
    void Awake()
    {
        // Set up the singleton pattern.
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            // If another GameManager already exists, destroy this one.
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Set our currency at the start of the game.
        currentCurrency = startingCurrency;
        UpdateCurrencyUI();
    }

    // A public function that other scripts can call to give the player money.
    public void AddCurrency(int amount)
    {
        currentCurrency += amount;
        UpdateCurrencyUI();
    }
    
    // A private function to update the text on the screen.
    private void UpdateCurrencyUI()
    {
        currencyText.text = "Currency: " + currentCurrency.ToString();
    }
}