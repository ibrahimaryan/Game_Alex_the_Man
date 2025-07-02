// PlayerMovement.cs (versi dibenarkan dan dibersihkan)
using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    // --- Referensi Komponen ---
    private Rigidbody2D body;
    private Animator anim;
    private SpriteRenderer sr;

    // --- Statistik Gerakan ---
    [Header("Movement Settings")]
    [SerializeField] private float speed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] public float minY = -2.5f;
    [SerializeField] public float maxY = 2.5f;

    // --- Serangan ---
    [Header("Attack Settings")]
    [SerializeField] private float attackDamage = 1f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Attack Cooldowns")]
    [SerializeField] private float attack1Cooldown = 0.5f;
    [SerializeField] private float attack2Cooldown = 0.7f;
    [SerializeField] private float attack3Cooldown = 1.0f;
    [SerializeField] private float specialComboDelay = 0.4f;
    
    private float attack1Timer;
    private float attack2Timer;
    private float attack3Timer;

    // --- Status ---
    private bool grounded;
    private bool isUpgraded = true;
    private bool isProtect = false;
    private bool isPerformingCombo = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isPerformingCombo)
        {
            body.velocity = new Vector2(0, body.velocity.y);
            anim.SetBool("run", false);
            return;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Gerakan
        body.velocity = new Vector2(horizontalInput * speed, verticalInput * speed);

        // Clamp Y agar tidak keluar layar
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);

        // Flip sprite
        if (horizontalInput > 0.01f)
            sr.flipX = false;
        else if (horizontalInput < -0.01f)
            sr.flipX = true;

        // Lompat (jika upgrade aktif dan di tanah)
        if (Input.GetKeyDown(KeyCode.Space) && isUpgraded && grounded)
        {
            Jump();
        }

        // Mode Proteksi
        if (Input.GetKey(KeyCode.S))
        {
            if (!isProtect)
            {
                isProtect = true;
                anim.SetBool("isProtect", true);
                anim.SetTrigger("protect");
            }
        }
        else if (isProtect)
        {
            isProtect = false;
            anim.SetBool("isProtect", false);
        }

        // Cooldown timer
        if (attack1Timer > 0) attack1Timer -= Time.deltaTime;
        if (attack2Timer > 0) attack2Timer -= Time.deltaTime;
        if (attack3Timer > 0) attack3Timer -= Time.deltaTime;

        // Serangan
        if (isUpgraded)
            HandleUpgradedAttacks();
        else
            HandleNormalAttack();

        // Animator
        anim.SetBool("run", horizontalInput != 0 || verticalInput != 0);
        anim.SetBool("grounded", grounded);
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpForce);
        anim.SetTrigger("jump");
        grounded = false;
    }

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
        if (Input.GetKeyDown(KeyCode.I) && !isPerformingCombo)
        {
            StartCoroutine(SpecialCombo());
        }

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

    private IEnumerator SpecialCombo()
    {
        isPerformingCombo = true;

        anim.SetTrigger("attack1");
        Attack();
        yield return new WaitForSeconds(specialComboDelay);

        anim.SetTrigger("attack3");
        Attack();
        yield return new WaitForSeconds(specialComboDelay);

        anim.SetTrigger("attack2");
        Attack();
        yield return new WaitForSeconds(specialComboDelay);

        anim.SetTrigger("attack1");
        Attack();
        yield return new WaitForSeconds(specialComboDelay);

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
        }
    }

    public bool IsProtecting() => isProtect;
}