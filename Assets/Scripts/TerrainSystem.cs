using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TerrainSystem : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private int width = 8;
    [SerializeField] private Biome[] biomes;
    [SerializeField] private Color radiationColor = Color.yellow;

    private int lastThreshold = 0;
    private int xOffset;
    private Biome currentBiome;
    private int biomeDepthOffset = -1;
    private int currBiomeIndex = 0;
    public Block[,] Blocks { get; set; }

    private void Start()
    {
        // initialize biomes
        Blocks = new Block[width, 10000];
        currentBiome = biomes[0];

        // initialize tiles
        xOffset = width / 2;
        LoadBiome(0);
    }

    public void RefreshBiomes(int playerY)
    {
        int threshold = -currentBiome.depth * currBiomeIndex - 1;
        if (playerY == threshold && playerY != lastThreshold)
        {
            currBiomeIndex++;
            lastThreshold = threshold;
            LoadBiome(currBiomeIndex);
        }
    }

    public Vector2 TileToWorld(Vector3Int tilePos)
    {
        return tilemap.CellToWorld(tilePos) + new Vector3(0.5f, 0.5f);
    }

    private Vector2Int GridToArray(Vector3Int gridPos)
    {
        int i = gridPos.x + xOffset;
        int j = -gridPos.y - 1;
        return new Vector2Int(i, j);
    }

    public bool CanMine(Vector3Int gridPos)
    {
        Vector2Int arrPos = GridToArray(gridPos);
        return arrPos.x >= 0 && arrPos.x < width && arrPos.y >= 0 && arrPos.y < 10000 && (Blocks[arrPos.x, arrPos.y] == null || Blocks[arrPos.x, arrPos.y].CanMine());
    }

    public float GetMineDuration(Vector3Int gridPos)
    {
        Vector2Int arrPos = GridToArray(gridPos);
        return Blocks[arrPos.x, arrPos.y].mineDuration;
    }

    public void MineBlock(Vector3Int gridPos)
    {
        Vector2Int arrPos = GridToArray(gridPos);
        if (Blocks[arrPos.x, arrPos.y] == null) return;
        tilemap.SetTile(gridPos, null);
        Blocks[arrPos.x, arrPos.y].OnMined();
        Blocks[arrPos.x, arrPos.y] = null;
    }

    public void RadiateRow(int yPos)
    {
        for (int i = 0; i < width; i++)
        {
            Vector3Int tilePos = new Vector3Int(i - xOffset, yPos);
            SetTileColor(tilePos, radiationColor);
        }
    }

    // public void UpdateBiomes()
    // {
    //     if (currBiomeIndex > 0)
    //     {
    //         int prevArrIdx = currBiomeIndex - 1;
    //         Biome prevBiome = biomes[prevArrIdx];
    //         for (int i = 0; i < width; i++)
    //         {
    //             for (int j = 0; j < prevBiome.depth; j++)
    //             {
    //                 Vector3Int tilePos = new Vector3Int(i - xOffset, -j + biomeDepthOffset + prevBiome.depth);
    //                 Blocks[i, j - (biomeDepthOffset + prevBiome.depth + 1)].OnUpdated(tilePos);
    //             }
    //         }
    //     }

    //     currentBiome = biomes[currBiomeIndex];
    // }

    private void LoadBiome(int biomeIndex)
    {
        int arrIdx = Mathf.Min(biomeIndex, biomes.Length - 1);
        if (biomeIndex > 0)
        {
            biomeDepthOffset -= currentBiome.depth;
        }
        currentBiome = biomes[arrIdx];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < currentBiome.depth; j++)
            {
                Vector3Int tilePos = new Vector3Int(i - xOffset, -j + biomeDepthOffset);
                List<Block> blockOptions = new List<Block>();
                foreach (BlockData blockData in currentBiome.blockTypes)
                {
                    for (int c = 0; c < blockData.spawnWeight; c++)
                    {
                        blockOptions.Add(blockData.block);
                    }
                }
                int randTileIdx = Random.Range(0, blockOptions.Count);
                Block selectedBlock = blockOptions[randTileIdx];
                tilemap.SetTile(tilePos, selectedBlock.tile);
                Blocks[i, j - (biomeDepthOffset + 1)] = selectedBlock;
                selectedBlock.OnSpawned(tilePos);
            }
        }
    }

    public void SetTileColor(Vector3Int tilePos, Color tileColor)
    {
        tilemap.RemoveTileFlags(tilePos, TileFlags.LockColor);
        tilemap.SetColor(tilePos, tileColor);
        tilemap.SetTileFlags(tilePos, TileFlags.LockColor);
    }

    [System.Serializable]
    private class Biome
    {
        public int depth = 100;
        public BlockData[] blockTypes;
    }

    [System.Serializable]
    private class BlockData
    {
        public Block block;
        public int spawnWeight = 1;
    }
}
