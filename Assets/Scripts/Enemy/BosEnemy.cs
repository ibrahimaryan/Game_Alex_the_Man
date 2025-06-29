using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BosEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float meleeCooldown;
    [SerializeField] private float rangedCooldown;
    [SerializeField] private float meleeRange;
    [SerializeField] private float rangedRange;
    [SerializeField] private int damage;

    [Header("References")]
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletHolder;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;

    private float meleeTimer = Mathf.Infinity;
    private float rangedTimer = Mathf.Infinity;

    private Animator anim;
    private Health playerHealth;
    private EnemyChasing enemyPatrol;
    private List<GameObject> bulletPool = new List<GameObject>();
    private bool isAttacking = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyChasing>();

        for (int i = 0; i < 5; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletHolder);
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
    }

    private void Update()
    {
        meleeTimer += Time.deltaTime;
        rangedTimer += Time.deltaTime;

        if (playerInSight(meleeRange))
        {
            if (meleeTimer >= meleeCooldown)
            {
                meleeTimer = 0;
                anim.SetTrigger("meleeAttack");
                isAttacking = true;
            }
        }
        else if (playerInSight(rangedRange))
        {
            if (rangedTimer >= rangedCooldown)
            {
                rangedTimer = 0;
                anim.SetTrigger("rangeAttack");
                isAttacking = true;
            }
        }

        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !playerInSight(rangedRange) && !isAttacking;
        }
    }

    // Dipanggil oleh Animation Event
    private void RangedAttack()
    {
        rangedTimer = 0;
        isAttacking = false;

        GameObject bullet = GetBulletFromPool();
        bullet.transform.position = firepoint.position;
        bullet.SetActive(true);

        float direction = transform.localScale.x > 0 ? 1f : -1f;
        bullet.GetComponent<BulletsEnemy>().SetDirection(direction);
    }

    // Dipanggil oleh Animation Event
    private void MeleeDamagePlayer()
    {
        if (playerInSight(meleeRange))
        {
            playerHealth.terkenaDamage(damage);
        }
        isAttacking = false;
    }

    private bool playerInSight(float range)
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * 0.5f,
            new Vector3(boxCollider.bounds.size.x + range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, transform.right * transform.localScale.x, 0, playerLayer);

        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<Health>();
        }

        return hit.collider != null;
    }

    private GameObject GetBulletFromPool()
    {
        foreach (var b in bulletPool)
        {
            if (!b.activeInHierarchy)
            {
                return b;
            }
        }

        GameObject newBullet = Instantiate(bulletPrefab, bulletHolder);
        newBullet.SetActive(false); // supaya nggak langsung aktif
        bulletPool.Add(newBullet);
        return newBullet;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * meleeRange * transform.localScale.x * 0.5f,
            new Vector3(boxCollider.bounds.size.x + meleeRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * rangedRange * transform.localScale.x * 0.5f,
            new Vector3(boxCollider.bounds.size.x + rangedRange, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}
