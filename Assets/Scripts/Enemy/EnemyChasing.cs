using UnityEngine;

public class EnemyChasing : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player;

    [Header("Movement Parameters")]
    [SerializeField] private float speed = 2f;
    private Vector3 initScale;

    [Header("Enemy Animator")]
    [SerializeField] private Animator anim;
    [SerializeField] private float chaseRange = 5f;
    [SerializeField] private float directionChangeDelay = 0.3f; // waktu tunggu setelah berbalik
    private int lastDirection = 0;
    private float directionChangeTimer = 0f;

    private void Awake()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }

        initScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    private void OnDisable()
    {
        anim.SetBool("moving", false);
    }

    private void Update()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;

        // Cek perubahan arah (X)
        int facingDirection = (int)Mathf.Sign(direction.x);
        if (facingDirection != lastDirection)
        {
            directionChangeTimer = directionChangeDelay;
            lastDirection = facingDirection;
        }

        if (directionChangeTimer > 0)
        {
            directionChangeTimer -= Time.deltaTime;
            anim.SetBool("moving", false);
            return;
        }

        // Flip arah berdasarkan X
        if (direction.x != 0)
            transform.localScale = new Vector3(initScale.x * Mathf.Sign(direction.x), initScale.y, initScale.z);

        transform.position += direction * speed * Time.deltaTime;
        anim.SetBool("moving", true);
    }

    private void MoveInDirection(int _direction)
    {
        transform.localScale = new Vector3(initScale.x * _direction, initScale.y, initScale.z);
    }
}