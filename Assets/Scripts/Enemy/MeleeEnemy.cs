using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Timeline;

public class MeleeEnemy : MonoBehaviour
{
    [Header ("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [Header ("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private int damage;
    [Header ("Player Layer")]
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;
    // references 
    private Animator anim;
    private Health playerHealth;
    private EnemyPatrol enemyPatrol;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        // attack if the player is in sight
        if (playerInsight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("meleeAttack");

            }
        }
        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !playerInsight();
        }
    }

    private bool playerInsight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
        new Vector3(boxCollider.bounds.size.x + range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
        0, Vector2.left, 0, playerLayer);
        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<Health>();
        }
        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
        new Vector3(boxCollider.bounds.size.x + range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void damagePlayer()
    {
        //if player still in damage area
        if (playerInsight())
        {
            playerHealth.terkenaDamage(damage);
        }
    }
}
