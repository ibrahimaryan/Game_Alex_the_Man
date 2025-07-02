using UnityEngine;
using System.Collections; // Diperlukan untuk Coroutine

public class PlayerMovement : MonoBehaviour
{
    // --- Referensi Komponen ---
    private Rigidbody2D body;
    private Animator anim;
    private SpriteRenderer sr; // REVISI: Variabel untuk cache SpriteRenderer

    // --- Statistik & Pengaturan Gerakan ---
    [Header("Movement Settings")]
    [SerializeField] private float speed = 7f;
    [SerializeField] private float jumpForce = 14f; // REVISI: Variabel khusus untuk lompat

    // --- Pengaturan Serangan ---
    [Header("Attack Settings")]
    [SerializeField] private float attackDamage = 1f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private LayerMask enemyLayer;

    // --- Pengaturan Cooldown Serangan Upgrade ---
    [Header("Attack Cooldowns")]
    [SerializeField] private float attack1Cooldown = 0.5f;
    [SerializeField] private float attack2Cooldown = 0.7f;
    [SerializeField] private float attack3Cooldown = 1.0f;
    [SerializeField] private float specialComboDelay = 0.4f; // REVISI: Jeda antar serangan kombo
    private float attack1Timer = 0f;
    private float attack2Timer = 0f;
    private float attack3Timer = 0f;

    // --- Status Player ---
    private bool grounded;
    private bool isUpgraded = true;
    private bool isProtect = false;
    private bool isPerformingCombo = false; // REVISI: Mencegah kombo dijalankan berulang kali

    private void Awake()
    {
        // Get reference for components from object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>(); // REVISI: Meng-cache komponen SpriteRenderer saat start
    }
    private void Jump()
    {
        // REVISI: Menggunakan jumpForce dan memicu animasi
        body.velocity = new Vector2(body.velocity.x, jumpForce);
        anim.SetTrigger("jump");
        //grounded = false;
    }
    void Update()
    {
        // Jangan proses input jika sedang dalam kombo spesial
        if (isPerformingCombo)
        {
            // Menghentikan gerakan horizontal saat kombo
            body.velocity = new Vector2(0, body.velocity.y);
            anim.SetBool("run", false); // Pastikan animasi lari mati
            return;
        }

        // --- Gerakan Horizontal ---
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        // --- Membalik Arah Sprite ---
        // REVISI: Menggunakan 'sr' yang sudah di-cache, bukan GetComponent()
        if (horizontalInput > 0.01f)
        {
            sr.flipX = false; // Hadap kanan
        }
        else if (horizontalInput < -0.01f)
        {
            sr.flipX = true; // Hadap kiri
        }

        // --- Update Timer Cooldown ---
        if (attack1Timer > 0) attack1Timer -= Time.deltaTime;
        if (attack2Timer > 0) attack2Timer -= Time.deltaTime;
        if (attack3Timer > 0) attack3Timer -= Time.deltaTime;

        // --- Input Aksi Player ---

        // 1. Lompat
        // REVISI: Lompat hanya bisa dilakukan saat di darat ('grounded')
        if (Input.GetKey(KeyCode.Space) && isUpgraded)
        {
            Jump();
        }

        // 2. Mode Bertahan (Protect)
        // REVISI: Logika disatukan, tidak ada duplikasi
        if (Input.GetKey(KeyCode.S))
        {
            if (!isProtect)
            {
                isProtect = true;
                anim.SetBool("isProtect", true);
                anim.SetTrigger("protect");
            }
        }
        else
        {
            if (isProtect)
            {
                isProtect = false;
                anim.SetBool("isProtect", false);
            }
        }
        
        // 3. Serangan (Berdasarkan status 'isUpgraded')
        if (isUpgraded)
        {
            HandleUpgradedAttacks();
        }
        else
        {
            HandleNormalAttack();
        }
        
        // --- Set Parameter Animator ---
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", grounded); // REVISI: Diaktifkan kembali untuk animasi lompat/jatuh
    }

    // --- Logika Serangan ---

    private void HandleNormalAttack()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            anim.SetTrigger("attack");
            Attack();
        }
    }

    private void HandleUpgradedAttacks()
    {
        // Kombo Spesial
        if (Input.GetKeyDown(KeyCode.I) && !isPerformingCombo)
        {
            StartCoroutine(SpecialCombo());
        }

        // Serangan Biasa (Mode Upgrade)
        if (Input.GetKeyDown(KeyCode.J) && attack1Timer <= 0f)
        {
            anim.SetTrigger("attack1");
            Attack();
            attack1Timer = attack1Cooldown;
        }
        else if (Input.GetKeyDown(KeyCode.K) && attack2Timer <= 0f)
        {
            anim.SetTrigger("attack2");
            Attack();
            attack2Timer = attack2Cooldown;
        }
        else if (Input.GetKeyDown(KeyCode.L) && attack3Timer <= 0f)
        {
            anim.SetTrigger("attack3");
            Attack();
            attack3Timer = attack3Cooldown;
        }
    }
    
    // REVISI: Menggunakan Coroutine untuk kombo spesial
    private IEnumerator SpecialCombo()
    {
        isPerformingCombo = true;

        // Serangan 1
        anim.SetTrigger("attack1");
        Attack();
        yield return new WaitForSeconds(specialComboDelay);

        // Serangan 2
        anim.SetTrigger("attack3");
        Attack();
        yield return new WaitForSeconds(specialComboDelay);

        // Serangan 3
        anim.SetTrigger("attack2");
        Attack();
        yield return new WaitForSeconds(specialComboDelay);
        
        // Serangan 4 (Final)
        anim.SetTrigger("attack1");
        Attack();
        yield return new WaitForSeconds(specialComboDelay); // Tunggu animasi selesai

        isPerformingCombo = false;
    }

    private void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            Health enemyHealth = enemy.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.terkenaDamage(attackDamage);
            }
        }
    }

    // --- Logika Fisik & Status ---

   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }

    public bool IsProtecting()
    {
        return isProtect;
    }
}