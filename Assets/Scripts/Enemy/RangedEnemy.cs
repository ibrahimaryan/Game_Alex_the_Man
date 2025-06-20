using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    // [Header ("Attack Parameters")]
    // [SerializeField] private float attackCooldown;
    // [SerializeField] private float range;

    // [Header ("Ranged Attack")]
    // [SerializeField] private Transform firepoint;
    // [SerializeField] private GameObject[] bullets;

    // [Header ("Collider Parameters")]
    // [SerializeField] private float colliderDistance;
    // [SerializeField] private int damage;

    // [Header ("Player Layer")]
    // [SerializeField] private BoxCollider2D boxCollider;
    // [SerializeField] private LayerMask playerLayer;
    // private float cooldownTimer = Mathf.Infinity;

    // // references 
    // private Animator anim;
    // private Health playerHealth;
    // private EnemyPatrol enemyPatrol;

    // private void Awake()
    // {
    //     anim = GetComponent<Animator>();
    //     enemyPatrol = GetComponentInParent<EnemyPatrol>();
    // }

    // private void Update()
    // {
    //     cooldownTimer += Time.deltaTime;

    //     // attack if the player is in sight
    //     if (playerInsight())
    //     {
    //         if (cooldownTimer >= attackCooldown)
    //         {
    //             cooldownTimer = 0;
    //             anim.SetTrigger("rangeAttack");

    //         }
    //     }
    //     if (enemyPatrol != null)
    //     {
    //         enemyPatrol.enabled = !playerInsight();
    //     }
    // }

    // private void rangedAttack()
    // {
    //     cooldownTimer = 0;
    //     //shoot projectile
    //     bullets[FindBullet()].transform.position = firepoint.position;
    //     bullets[FindBullet()].GetComponent<enemyBullets>().ActivateBullet();
    // }

    // private int FindBullet()
    // {
    //     for (int i = 0; i < bullets.Length; i++)
    //     {
    //         if (!bullets[i].activeInHierarchy)
    //         {
    //             // bullets[i].SetActive(true);
    //             return i;
    //         }
    //     }
    //     return 0;
    // }

    // private bool playerInsight()
    // {
    //     RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
    //     new Vector3(boxCollider.bounds.size.x + range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
    //     0, Vector2.left, 0, playerLayer);
        
    //     return hit.collider != null;
    // }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
    //     new Vector3(boxCollider.bounds.size.x + range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    // }


}
