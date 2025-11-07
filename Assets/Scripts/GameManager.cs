using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one GameManager in scene!");
            return;
        }
        instance = this;
    }

    //UI References
    public TextMeshProUGUI CurrencyText;

    //Game Variables
    public int StartingCurrency;
    private int currentCurrency;

    void Start()
    {
        currentCurrency = StartingCurrency;
        UpdateCurrencyText();
    }

    public void AddCurrency(int amount)
    {
        currentCurrency += amount;
        UpdateCurrencyText();
    }

    public void SpendCurrency(int amount)
    {
        currentCurrency -= amount;
        UpdateCurrencyText();
    }

    void UpdateCurrencyText()
    {
        if (CurrencyText != null)
        {
            CurrencyText.text = "CURRENCY: " + currentCurrency;
        }
    }
}