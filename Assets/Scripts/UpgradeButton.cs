using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    [SerializeField] private int cost = 5;

    [SerializeField] private Upgrade upgrade;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void ApplyUpgrade()
    {
        GameManager.UpgradeSystem.RemoveCurrency(cost);
        upgrade.Apply();
    }

    public void UpdateUsability(int currencyTotal)
    {
        button.interactable = currencyTotal >= cost;
    }
}
