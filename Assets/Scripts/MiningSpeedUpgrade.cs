using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MiningSpeedUpgrade", menuName = "MiningSpeedUpgrade", order = 61)]
public class MiningSpeedUpgrade : Upgrade
{
    [SerializeField] private float speedIncrease = 0.2f;

    public override void Apply()
    {
        GameManager.Player.MineSpeedMultiplier += speedIncrease;
    }
}
