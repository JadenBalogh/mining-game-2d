using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MiningCooldownUpgrade", menuName = "MiningCooldownUpgrade", order = 60)]
public class MiningCooldownUpgrade : Upgrade
{
    [SerializeField] private float cooldownReduction = 0.1f;

    public override void Apply()
    {
        GameManager.Player.MineCooldown -= cooldownReduction;
    }
}
