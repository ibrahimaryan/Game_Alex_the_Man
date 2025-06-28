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

        float distanceX = player.position.x - transform.position.x;
        int direction = (int)Mathf.Sign(distanceX);

        // Cek jika arah berubah
        if (direction != lastDirection)
        {
            directionChangeTimer = directionChangeDelay;
            lastDirection = direction;
        }

        // Kurangi timer jika masih ada delay
        if (directionChangeTimer > 0)
        {
            directionChangeTimer -= Time.deltaTime;
            anim.SetBool("moving", false);
            return;
        }

        // Flip arah
        transform.localScale = new Vector3(initScale.x * direction, initScale.y, initScale.z);

        if (Mathf.Abs(distanceX) > 0.05f)
        {
            anim.SetBool("moving", true);
            transform.position = new Vector3(
                transform.position.x + Time.deltaTime * direction * speed,
                transform.position.y,
                transform.position.z
            );
        }
        else
        {
            anim.SetBool("moving", false);
        }
    }

    private void MoveInDirection(int _direction)
    {
        transform.localScale = new Vector3(initScale.x * _direction, initScale.y, initScale.z);
    }
}