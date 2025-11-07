using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI References")]
    public TextMeshProUGUI currencyText;

    [Header("Game Variables")]
    public int startingCurrency = 100;
    
    // We make this a public "property" so other scripts can read the value
    // but can't accidentally change it. The "private set" protects it.
    public int CurrentCurrency { get; private set; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        CurrentCurrency = startingCurrency;
        UpdateCurrencyUI();
    }

    public void AddCurrency(int amount)
    {
        CurrentCurrency += amount;
        UpdateCurrencyUI();
    }
    
    // NEW FUNCTION! This allows other scripts to spend money.
    public void SpendCurrency(int amount)
    {
        CurrentCurrency -= amount;
        UpdateCurrencyUI();
    }

    private void UpdateCurrencyUI()
    {
        currencyText.text = "Currency: " + CurrentCurrency.ToString();
    }
}