using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveTime = 1f;

    [SerializeField] private float baseMineCooldown = 0.25f;
    public float MineCooldown { get; set; }

    [SerializeField] private int maxHealth = 5;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI scoreText;

    public Vector3Int GridPos { get; set; }

    private Vector2 targetPos;
    private Vector2 currentVelocity;
    private bool canMine = true;
    private int health = 0;
    private int score = 0;

    private void Start()
    {
        MineCooldown = baseMineCooldown;
        health = maxHealth;
        healthText.text = "Health: " + health;
        scoreText.text = "Score: " + score;
        targetPos = GameManager.TerrainSystem.TileToWorld(Vector3Int.zero);
        transform.position = targetPos;
    }

    private void Update()
    {
        if (canMine)
        {
            bool inputFound = false;
            if (Input.GetKeyDown(KeyCode.A)) // left
            {
                Mine(GridPos + Vector3Int.left);
                inputFound = true;
            }
            if (Input.GetKeyDown(KeyCode.S)) // down
            {
                Mine(GridPos + Vector3Int.down);
                inputFound = true;
            }
            if (Input.GetKeyDown(KeyCode.D)) // right
            {
                Mine(GridPos + Vector3Int.right);
                inputFound = true;
            }
            if (inputFound)
            {
                StartCoroutine(MineCooldownWait());
            }
        }

        transform.position = Vector2.SmoothDamp(transform.position, targetPos, ref currentVelocity, moveTime);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthText.text = "Health: " + health;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void Mine(Vector3Int gridPos)
    {
        if (GameManager.TerrainSystem.CanMine(gridPos))
        {
            GameManager.TerrainSystem.MineBlock(gridPos);
            targetPos = GameManager.TerrainSystem.TileToWorld(gridPos);
            GridPos = gridPos;
            GameManager.TerrainSystem.RefreshBiomes(GridPos.y);

            // update score
            score = -GridPos.y;
            scoreText.text = "Score: " + score;
        }
    }

    private IEnumerator MineCooldownWait()
    {
        canMine = false;
        yield return new WaitForSeconds(baseMineCooldown);
        canMine = true;
    }
}
