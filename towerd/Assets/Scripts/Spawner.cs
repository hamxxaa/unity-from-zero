using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform targetBase;
    [SerializeField] float spawnInterval = 2f;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        if (enemyScript != null)
        {
            enemyScript.SetTarget(targetBase);
        }
    }
}