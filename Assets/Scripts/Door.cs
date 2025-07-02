using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private CameraController cam;

    [Header("Enemy Check")]
    [SerializeField] private EnemySpawner[] spawnersToCheck; // array untuk banyak spawner

    [Header("Blocking Collider")]
    [SerializeField] private GameObject blocker;

    private void Update()
    {
        if (spawnersToCheck != null && spawnersToCheck.Length > 0 && blocker != null)
        {
            bool allCleared = true;
            foreach (var spawner in spawnersToCheck)
            {
                if (spawner != null && !spawner.IsCleared)
                {
                    allCleared = false;
                    break;
                }
            }
            blocker.SetActive(!allCleared);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && AllSpawnersCleared())
        {
            if (collision.transform.position.x < transform.position.x)
                cam.MoveToNewRoom(nextRoom);
            else
                cam.MoveToNewRoom(previousRoom);
        }
    }

    private bool AllSpawnersCleared()
    {
        foreach (var spawner in spawnersToCheck)
        {
            if (spawner != null && !spawner.IsCleared)
                return false;
        }
        return true;
    }
}
