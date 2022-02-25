using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GemBlock", menuName = "GemBlock", order = 51)]
public class GemBlock : Block
{
    [SerializeField] private int currencyOnMine = 2;

    public override void OnMined()
    {
        GameManager.UpgradeSystem.AddCurrency(currencyOnMine);
    }
}
