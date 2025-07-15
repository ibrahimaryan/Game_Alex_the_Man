using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject ikerPrefab;
    [SerializeField] private GameObject ikerCollectiblePrefab;
    [SerializeField] private GameObject eskimoPrefab;
    [SerializeField] private GameObject eskimoCollectiblePrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float ikerInterval = 5f;
    [SerializeField] private int ikerCount = 5;

    [SerializeField] private float ikerCollectibleInterval = 5f;
    [SerializeField] private int ikerCollectibleCount = 5;

    [SerializeField] private float eskimoInterval = 7f;
    [SerializeField] private int eskimoCount = 3;

    [SerializeField] private float eskimoCollectibleInterval = 7f;
    [SerializeField] private int eskimoCollectibleCount = 3;

    [Header("Spawn Point")]
    [SerializeField] private Transform spawnPoint;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private int totalEnemiesToSpawn;
    private int enemiesSpawned = 0;
    private int enemiesDefeated = 0;
    public bool IsCleared => enemiesSpawned == totalEnemiesToSpawn && enemiesDefeated == totalEnemiesToSpawn;

    private void Awake()
    {
        enabled = false;
    }

    void OnEnable()
    {
        // Hitung total musuh yang akan keluar
        totalEnemiesToSpawn = ikerCount + eskimoCount + ikerCollectibleCount + eskimoCollectibleCount;
        enemiesSpawned = 0;
        enemiesDefeated = 0;

        StartCoroutine(SpawnEnemy(ikerInterval, ikerPrefab, ikerCount));
        StartCoroutine(SpawnEnemy(ikerCollectibleInterval, ikerCollectiblePrefab, ikerCollectibleCount));
        StartCoroutine(SpawnEnemy(eskimoInterval, eskimoPrefab, eskimoCount));
        StartCoroutine(SpawnEnemy(eskimoCollectibleInterval, eskimoCollectiblePrefab, eskimoCollectibleCount));
    }

    private void Update()
    {
        activeEnemies.RemoveAll(enemy => enemy == null); // Hapus musuh yang sudah dihancurkan
    }

    private IEnumerator SpawnEnemy(float interval, GameObject enemyPrefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(interval);

            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
            activeEnemies.Add(enemy);
            enemiesSpawned++;

            EnemyDeathNotifier notifier = enemy.GetComponent<EnemyDeathNotifier>();
            if (notifier == null)
            {
                notifier = enemy.AddComponent<EnemyDeathNotifier>();
            }
            notifier.spawner = this;
        }
    }

    public void NotifyEnemyDefeated()
    {
        enemiesDefeated++;
    }

}
