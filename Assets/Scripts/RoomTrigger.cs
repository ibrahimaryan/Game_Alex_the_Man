using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField] private EnemySpawner[] spawnersToActivate;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered && collision.CompareTag("Player"))
        {
            triggered = true;
            foreach (EnemySpawner spawner in spawnersToActivate)
            {
                if (spawner != null)
                    spawner.enabled = true;
            }
        }
    }

}
