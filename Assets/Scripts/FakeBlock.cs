using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FakeBlock", menuName = "FakeBlock", order = 52)]
public class FakeBlock : Block
{
    [SerializeField] private int damageOnMined = 1;
    [SerializeField] private int slowDuration = 4;
    [SerializeField] private Color dangerColor = Color.red;

    public override void OnMined()
    {
        GameManager.Player.TakeDamage(damageOnMined);
        GameManager.Player.Slow(slowDuration);
    }

    public override void OnSpawned(Vector3Int tilePos)
    {
        UpdateDangerColor(tilePos);
    }

    public override void OnUpdated(Vector3Int tilePos)
    {
        UpdateDangerColor(tilePos);
    }

    private void UpdateDangerColor(Vector3Int tilePos)
    {
        if (GameManager.Player.CanSeeDanger)
        {
            GameManager.TerrainSystem.SetTileColor(tilePos, dangerColor);
        }
    }
}
