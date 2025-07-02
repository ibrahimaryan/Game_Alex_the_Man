using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSpawner : MonoBehaviour
{
    [Header("Health Prefab")]
    [SerializeField] private GameObject healthPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private int maxHealthItems = 5;

    private List<GameObject> spawnedItems = new List<GameObject>();
    private BoxCollider2D spawnArea; // opsional jika kamu pakai collider sebagai area

    private void Start()
    {
        StartCoroutine(SpawnHealthRoutine());
    }

    private IEnumerator SpawnHealthRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            spawnedItems.RemoveAll(item => item == null || !item.activeInHierarchy);

            if (spawnedItems.Count < maxHealthItems)
            {
                SpawnHealth();
            }
        }
    }

    private void SpawnHealth()
    {
        Vector2 randomPosition = GetRandomPositionWithinBounds();
        GameObject newHealth = Instantiate(healthPrefab, randomPosition, Quaternion.identity);
        spawnedItems.Add(newHealth);
    }

    private Vector2 GetRandomPositionWithinBounds()
    {
        Vector2 center = transform.position;
        Vector2 size = transform.localScale;

        float randomX = Random.Range(center.x - size.x / 2f, center.x + size.x / 2f);
        float randomY = Random.Range(center.y - size.y / 2f, center.y + size.y / 2f);

        return new Vector2(randomX, randomY);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
