using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiationSystem : MonoBehaviour
{
    [SerializeField] private float moveInterval = 4f;
    [SerializeField] private float moveIntervalCap = 0.2f;
    [SerializeField] private float moveIntervalDecreasePercent = 0.25f;
    [SerializeField] private int damagePerTick = 1;
    [SerializeField] private int damageInterval = 1;

    public int YPosition { get; private set; }
    private Player player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        StartCoroutine(RadiationLoop());
    }

    private IEnumerator DamageLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(damageInterval);
            if (player.GridPos.y >= YPosition)
            {
                player.TakeDamage(damagePerTick);
            }
        }
    }

    private IEnumerator RadiationLoop()
    {
        yield return new WaitForSeconds(moveInterval);
        StartCoroutine(DamageLoop());
        while (true)
        {
            moveInterval *= (1.0f - moveIntervalDecreasePercent);
            moveInterval = Mathf.Max(moveInterval, moveIntervalCap);
            GameManager.TerrainSystem.RadiateRow(YPosition);
            YPosition--;
            yield return new WaitForSeconds(moveInterval);
        }
    }
}
