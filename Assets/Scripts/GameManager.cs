using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static TerrainSystem TerrainSystem { get; private set; }
    public static UpgradeSystem UpgradeSystem { get; private set; }

    public static Player Player { get; private set; }

    [SerializeField] private CanvasGroup gameOverPanel;

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

        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        Time.timeScale = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public static void EndGame() => instance.IEndGame();
    private void IEndGame()
    {
        Time.timeScale = 0;
        gameOverPanel.alpha = 1;
    }
}
