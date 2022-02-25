using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "XrayVisionUpgrade", menuName = "XrayVisionUpgrade", order = 62)]
public class XrayVisionUpgrade : Upgrade
{
    public override void Apply()
    {
        GameManager.Player.CanSeeDanger = true;
    }
}
