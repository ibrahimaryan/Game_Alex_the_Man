using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField] private EnemySpawner spawnerToActivate;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered && collision.CompareTag("Player"))
        {
            triggered = true;
            if (spawnerToActivate != null)
                spawnerToActivate.enabled = true;
        }
    }

}
