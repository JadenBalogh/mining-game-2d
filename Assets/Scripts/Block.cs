using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Block", menuName = "Block", order = 50)]
public class Block : ScriptableObject
{
    public TileBase tile;
    public float mineDuration;

    public virtual bool CanMine() { return true; }
    public virtual void OnMined() { }
    public virtual void OnSpawned(Vector3Int tilePos) { }
    public virtual void OnUpdated(Vector3Int tilePos) { }
}
