using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
   [Header("Enemy Prefabs")]
    [SerializeField] private GameObject ikerPrefab;
    [SerializeField] private GameObject eskimoPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float ikerInterval = 5f;
    [SerializeField] private int ikerCount = 5;

    [SerializeField] private float eskimoInterval = 7f;
    [SerializeField] private int eskimoCount = 3;

    [Header("Spawn Point")]
    [SerializeField] private Transform spawnPoint;

    void Start()
    {
        StartCoroutine(SpawnEnemy(ikerInterval, ikerPrefab, ikerCount));
        StartCoroutine(SpawnEnemy(eskimoInterval, eskimoPrefab, eskimoCount));
    }

    private IEnumerator SpawnEnemy(float interval, GameObject enemyPrefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return new WaitForSeconds(interval);
            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        }
    }
}
