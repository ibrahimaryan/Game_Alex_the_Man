using UnityEngine;
using System.Collections.Generic;

public class RangedEnemy : MonoBehaviour
{
    [Header ("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;

    [Header("Ranged Attack")]
    [SerializeField] private Transform firepoint;
    [SerializeField] private GameObject[] bullets;

    [Header ("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletHolder; // untuk menyimpan hasil clone
    private List<GameObject> bulletPool = new List<GameObject>();
    private bool isAttacking = false;


    // references 
    private Animator anim;
    private Health playerHealth;
    private EnemyPatrol enemyPatrol;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();

        // Inisialisasi peluru awal jika ingin (optional)
        for (int i = 0; i < 5; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletHolder);
            bullet.SetActive(false);
            bulletPool.Add(bullet);
        }
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (playerInsight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("rangeAttack");
                isAttacking = true;
            }
        }

        if (enemyPatrol != null)
        {
            // jangan aktifkan patrol kalau baru saja attack
            if (!playerInsight() && !isAttacking)
            {
                enemyPatrol.enabled = true;
            }
            else
            {
                enemyPatrol.enabled = false;
            }
        }
    }

    private void rangedAttack()
    {
        cooldownTimer = 0;
        isAttacking = false;

        GameObject bullet = GetBulletFromPool();
        bullet.transform.position = firepoint.position;
        bullet.SetActive(true);

        float direction = transform.localScale.x > 0 ? 1f : -1f;
        bullet.GetComponent<BulletsEnemy>().SetDirection(direction);
    }
    
    private GameObject GetBulletFromPool()
    {
        foreach (var b in bulletPool)
        {
            if (!b.activeInHierarchy)
            {
                Debug.Log("🔁 Gunakan kembali peluru lama");
                return b;
            }
        }

        Debug.Log("✨ Buat peluru baru!");
        GameObject newBullet = Instantiate(bulletPrefab, bulletHolder);
        bulletPool.Add(newBullet);
        return newBullet;
    }

    private int FindBullet()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            if (!bullets[i].activeInHierarchy)
                return i;
        }

        Debug.LogWarning("Semua peluru sedang aktif! Tidak ada peluru tersedia.");
        return -1; // tidak ada peluru yang bisa dipakai
    }


    private bool playerInsight()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
        boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
        new Vector3(boxCollider.bounds.size.x + range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
        0, transform.right * transform.localScale.x, 0, playerLayer);

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
        new Vector3(boxCollider.bounds.size.x + range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }


}
