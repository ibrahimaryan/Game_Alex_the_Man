using UnityEngine;

public class DoorBoss : MonoBehaviour
{
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private CameraController cam;

    [Header("Enemy Check")]
    [SerializeField] private EnemySpawner[] spawnersToCheck;

    [Header("Boss Check (optional)")]
    [SerializeField] private GameObject bossToCheck;

    [Header("Blocking Collider")]
    [SerializeField] private GameObject blocker;

    private void Update()
    {
        if (blocker != null)
        {
            blocker.SetActive(!IsRoomCleared());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && IsRoomCleared())
        {
            if (collision.transform.position.x < transform.position.x)
                cam.MoveToNewRoom(nextRoom);
            else
                cam.MoveToNewRoom(previousRoom);
        }
    }

    private bool IsRoomCleared()
    {
        // Cek semua spawner (jika ada)
        if (spawnersToCheck != null && spawnersToCheck.Length > 0)
        {
            foreach (var spawner in spawnersToCheck)
            {
                if (spawner != null && !spawner.IsCleared)
                    return false;
            }
        }

        // Cek boss (jika diisi)
        if (bossToCheck != null)
        {
            return !bossToCheck.activeInHierarchy; // boss sudah mati jika tidak aktif
        }

        // Jika tidak ada spawner maupun boss, anggap tidak clear
        return false;
    }
}