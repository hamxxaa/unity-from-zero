using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager Instance;

    [Header("Map Settings")]
    [SerializeField] MapGenerator mapGenerator;

    [Header("Wave Settings")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float timeBetweenWaves = 3f;
    [SerializeField] int baseEnemyCount = 3;
    [SerializeField] float difficultyMultiplier = 1.2f;

    [Header("UI References")]
    [SerializeField] GameUI gameUI;

    // Score Tracking
    private int score = 0;
    private int highScore = 0;
    private Health playerHealth;

    // Game State
    private int currentWave = 0;
    private int enemiesAlive = 0;
    private bool isWaveActive = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerHealth = playerObj.GetComponent<Health>();

            playerHealth.OnDeath += OnPlayerDeath;
        }

        StartCoroutine(InitializeGame());
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (gameUI != null) gameUI.UpdateScoreUI(score, highScore);
    }

    void OnPlayerDeath()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.gameOverSound, 1f);

        isWaveActive = false;

        if (gameUI != null)
            gameUI.ShowGameOverPanel();
    }

    public void RestartGame()
    {
        Time.timeScale = 1;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator InitializeGame()
    {
        mapGenerator.GenerateMap();
        yield return new WaitForFixedUpdate();

        StartCoroutine(StartNextWave());
    }

    IEnumerator StartNextWave()
    {
        currentWave++;
        isWaveActive = true;

        if (gameUI != null)
        {
            gameUI.UpdateWaveText(currentWave);
            gameUI.ShowWaveAnnouncement(currentWave);
        }

        playerHealth.HealFull();

        int enemyCount = Mathf.RoundToInt(baseEnemyCount * Mathf.Pow(difficultyMultiplier, currentWave - 1));
        if (gameUI != null) gameUI.UpdateEnemyCount(enemyCount);


        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
    }

    void SpawnEnemy()
    {
        float range = 18f;
        Vector3 spawnPos = Vector3.zero;
        bool validPositionFound = false;

        // Try 10 times to find a valid NavMesh position
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomPoint = new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
            UnityEngine.AI.NavMeshHit hit;

            if (UnityEngine.AI.NavMesh.SamplePosition(randomPoint, out hit, 2.0f, UnityEngine.AI.NavMesh.AllAreas))
            {
                spawnPos = hit.position;
                validPositionFound = true;
                break;
            }
        }

        if (validPositionFound)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            enemiesAlive++;

            Health enemyHealth = newEnemy.GetComponentInChildren<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.OnDeath += OnEnemyKilled;
            }
        }
    }

    void OnEnemyKilled()
    {
        enemiesAlive--;
        if (gameUI != null)
        {
            gameUI.UpdateEnemyCount(enemiesAlive);
        }
        AddScore(100);
        if (enemiesAlive <= 0 && isWaveActive)
        {
            StartCoroutine(WaveCompleted());
        }
    }

    IEnumerator WaveCompleted()
    {
        isWaveActive = false;
        Debug.Log("WAVE TAMAMLANDI! Dinleniliyor...");

        yield return new WaitForSeconds(timeBetweenWaves);

        StartCoroutine(StartNextWave());
    }

    void AddScore(int amount)
    {
        score += amount;
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
        if (gameUI != null) gameUI.UpdateScoreUI(score, highScore);
    }

}