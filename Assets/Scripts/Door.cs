using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform previousRoom;
    [SerializeField] private Transform nextRoom;
    [SerializeField] private CameraController cam;

    [Header("Enemy Check")]
    [SerializeField] private EnemySpawner spawnerToCheck;

    [Header("Blocking Collider")]
    [SerializeField] private GameObject blocker; // child yang punya BoxCollider2D (non-trigger)

    private void Update()
    {
        if (spawnerToCheck != null && blocker != null)
        {
            bool cleared = spawnerToCheck.IsCleared;
            Debug.Log("Spawner cleared? " + cleared);
            blocker.SetActive(!cleared);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && spawnerToCheck != null && spawnerToCheck.IsCleared)
        {
            if (collision.transform.position.x < transform.position.x)
                cam.MoveToNewRoom(nextRoom);
            else
                cam.MoveToNewRoom(previousRoom);
        }
    }
}
