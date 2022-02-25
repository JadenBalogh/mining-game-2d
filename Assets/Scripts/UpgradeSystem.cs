using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currencyText;
    [SerializeField] private UpgradeButton[] upgradeButtons;

    private int currency = 0;

    private void Start()
    {
        UpdateCurrencyUI();
    }

    public void AddCurrency(int amount)
    {
        currency += amount;
        UpdateCurrencyUI();
    }

    public void RemoveCurrency(int amount)
    {
        currency -= amount;
        UpdateCurrencyUI();
    }

    private void UpdateCurrencyUI()
    {
        currencyText.text = "Gems: " + currency;
        foreach (UpgradeButton upgradeButton in upgradeButtons)
        {
            upgradeButton.UpdateUsability(currency);
        }
    }
}
