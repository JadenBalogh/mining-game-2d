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

    public float MineSpeedMultiplier { get; set; }
    public bool CanSeeDanger { get; set; }

    public Vector3Int GridPos { get; set; }

    private bool isAlive = true;
    private Vector2 targetPos;
    private Vector2 currentVelocity;
    private bool isMining = false;
    private bool isSlowed = false;
    private bool canMine = true;
    private int health = 0;
    private int score = 0;

    private Coroutine slowRoutine;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

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
        if (!isAlive) return;

        if (!isMining && canMine)
        {
            if (Input.GetKeyDown(KeyCode.A)) // left
            {
                Mine(GridPos + Vector3Int.left);
            }
            if (Input.GetKeyDown(KeyCode.S)) // down
            {
                Mine(GridPos + Vector3Int.down);
            }
            if (Input.GetKeyDown(KeyCode.D)) // right
            {
                Mine(GridPos + Vector3Int.right);
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
            isAlive = false;
            GameManager.EndGame();
        }
    }

    public void Slow(float duration)
    {
        // TODO: implement slowing
        if (slowRoutine != null) StopCoroutine(slowRoutine);
        slowRoutine = StartCoroutine(SlowTimer(duration));
    }

    private void Mine(Vector3Int gridPos)
    {
        if (GameManager.TerrainSystem.CanMine(gridPos))
        {
            StartCoroutine(MineRoutine(gridPos));
        }
    }

    private IEnumerator MineRoutine(Vector3Int gridPos)
    {
        GridPos = gridPos;

        animator.SetBool("IsMining", true);
        isMining = true;
        float mineDuration = GameManager.TerrainSystem.GetMineDuration(gridPos) * (1 - MineSpeedMultiplier);
        yield return new WaitForSeconds(mineDuration);
        isMining = false;
        animator.SetBool("IsMining", false);
        StartCoroutine(MineCooldownWait());

        GameManager.TerrainSystem.MineBlock(gridPos);
        targetPos = GameManager.TerrainSystem.TileToWorld(gridPos);
        GameManager.TerrainSystem.RefreshBiomes(GridPos.y);

        // update score
        score = -GridPos.y;
        scoreText.text = "Score: " + score;
    }

    private IEnumerator MineCooldownWait()
    {
        canMine = false;
        yield return new WaitForSeconds(MineCooldown);
        canMine = true;
    }

    private IEnumerator SlowTimer(float duration)
    {
        isSlowed = true;
        yield return new WaitForSeconds(duration);
        isSlowed = false;
    }
}
