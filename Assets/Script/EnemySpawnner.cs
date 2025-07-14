using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnner : MonoBehaviour
{
    public GameObject blackWerePrefab;
    public Transform spawnPoint;
    public EnemySpawnner nextSpawner; // spawner berikutnya

    private GameObject currentEnemy;
    private int totalSpawned = 0;
    public int maxEnemies = 5;

    private float spawnTimer = 0f;
    private bool waitingForSpawn = false;
    public float spawnTimeout = 10f;

    private float enemyDeathCheckTimer = 0f;
    private float enemyDeathCheckInterval = 0.5f;

    private void Start()
    {
        SpawnEnemy();
    }

    private void Update()
    {
        // Cek apakah currentEnemy sudah tidak aktif (mati), lalu null-kan
        if (currentEnemy != null)
        {
            enemyDeathCheckTimer += Time.deltaTime;
            if (enemyDeathCheckTimer >= enemyDeathCheckInterval)
            {
                if (!currentEnemy.activeInHierarchy)
                {
                    currentEnemy = null;
                }
                enemyDeathCheckTimer = 0f;
            }
        }

        // Spawn musuh baru jika yang sebelumnya mati
        if (currentEnemy == null && totalSpawned < maxEnemies && !waitingForSpawn)
        {
            StartCoroutine(SpawnAfterDelay(1f));
        }

        // Timeout jika musuh tidak muncul
        if (waitingForSpawn)
        {
            spawnTimer += Time.deltaTime;

            if (spawnTimer >= spawnTimeout)
            {
                waitingForSpawn = false;
                spawnTimer = 0f;
                totalSpawned++; // lewati slot ini
                Debug.Log("Enemy tidak muncul selama 10 detik, lanjut ke berikutnya.");
            }
        }

        // Setelah selesai spawn semua dan enemy terakhir mati → aktifkan next spawner
        if (totalSpawned >= maxEnemies && currentEnemy == null && nextSpawner != null && !nextSpawner.gameObject.activeSelf)
        {
            nextSpawner.gameObject.SetActive(true);
        }
    }

    void SpawnEnemy()
    {
        currentEnemy = Instantiate(blackWerePrefab, spawnPoint.position, Quaternion.identity);
        totalSpawned++;
        spawnTimer = 0f;
        waitingForSpawn = false;
    }

    IEnumerator SpawnAfterDelay(float delay)
    {
        waitingForSpawn = true;
        spawnTimer = 0f;
        yield return new WaitForSeconds(delay);

        if (currentEnemy == null && totalSpawned < maxEnemies)
        {
            SpawnEnemy();
        }
    }
}
