using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private Transform enemy;

    [Header("Target")]
    [SerializeField] private Transform player;

    [Header("Movement Parameters")]
    [SerializeField] private float speed = 2f;
    private Vector3 initScale;

    [Header("Enemy Animator")]
    [SerializeField] private Animator anim;

    private void Awake()
    {
        initScale = enemy.localScale;
    }

    private void OnDisable()
    {
        anim.SetBool("moving", false);
    }

    private void Update()
    {
        if (player == null) return;

        float distanceX = player.position.x - enemy.position.x;
        int direction = distanceX > 0 ? 1 : -1;

        // Gerak ke arah player
        MoveInDirection(direction);
    }

    private void MoveInDirection(int _direction)
    {
        anim.SetBool("moving", true);

        // Flip arah sesuai posisi player
        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction, initScale.y, initScale.z);

        // Bergerak ke arah player di sumbu X saja
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed,
                                     enemy.position.y,
                                     enemy.position.z);
    }
}
