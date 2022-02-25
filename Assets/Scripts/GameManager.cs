using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static TerrainSystem TerrainSystem { get; set; }
    public static UpgradeSystem UpgradeSystem { get; set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        TerrainSystem = GetComponent<TerrainSystem>();
        UpgradeSystem = GetComponent<UpgradeSystem>();
    }
}
