// PlayerMovement.cs (versi dibenarkan dan dibersihkan)
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class PlayerMovement : MonoBehaviour
{
    // --- Referensi Komponen ---
    private Rigidbody2D body;
    private Animator anim;
    private SpriteRenderer sr;
    private AudioSource audioSource;

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
    [SerializeField] private float specialComboCooldown = 5f; // waktu cooldown dalam detik
    private float specialComboCooldownTimer = 0f;
    [SerializeField] private float specialComboDelay = 0.4f;
    [Header("Audio Settings")]
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip specialComboSound;
    [Header("Scale Settings")]
    [Tooltip("Skala player saat dalam kondisi normal.")]
    [SerializeField] private Vector3 normalScale = new Vector3(2, 2, 1);
    [Tooltip("Skala player saat dalam kondisi upgrade.")]
    [SerializeField] private Vector3 upgradedScale = new Vector3(3, 3, 1);
   private float attack1Timer = 0f;
    private float attack2Timer = 0f;
    private float attack3Timer = 0f;
    private bool isPerformingCombo = false;

    // --- Status ---
    private bool grounded;
    private bool isUpgraded = true;
    private bool isProtect = false;
    private BoxCollider2D boxCollider;
    private Vector2 originalColliderOffset;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "LastScene"|| SceneManager.GetActiveScene().name == "MainMenu")
        {
            Destroy(gameObject);
        }
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
        Vector3 targetScale = isUpgraded ? upgradedScale : normalScale;

        // Atur skala berdasarkan input horizontal
        if (horizontalInput > 0.01f)
        {
            // Hadap kanan
            transform.localScale = targetScale;
        }
        else if (horizontalInput < -0.01f)
        {
            // Hadap kiri
            transform.localScale = new Vector3(-targetScale.x, targetScale.y, targetScale.z);
        }

        // Lompat (jika upgrade aktif dan di tanah)
        if (Input.GetKeyDown(KeyCode.Space) && isUpgraded && grounded)
        {
            Jump();
        }

        // Mode Proteksi
        // if (Input.GetKey(KeyCode.S))
        // {
        //     if (!isProtect)
        //     {
        //         isProtect = true;
        //         anim.SetBool("isProtect", true);
        //         anim.SetTrigger("protect");
        //     }
        // }
        // else if (isProtect)
        // {
        //     isProtect = false;
        //     anim.SetBool("isProtect", false);
        // }

        // Cooldown timer
          // Cek apakah pemain sedang dalam kondisi menyerang (berdasarkan timer cooldown atau kombo)
        bool isAttacking = attack1Timer > 0 || attack2Timer > 0 || attack3Timer > 0 || isPerformingCombo;

        // --- Update Timer Cooldown (dipindahkan ke atas) ---
        if (attack1Timer > 0) attack1Timer -= Time.deltaTime;
        if (attack2Timer > 0) attack2Timer -= Time.deltaTime;
        if (attack3Timer > 0) attack3Timer -= Time.deltaTime;

        // --- Logika Gerakan (Hanya jika TIDAK menyerang) ---
        if (!isAttacking)
        {
            // Proses input gerakan horizontal
            //float horizontalInput = Input.GetAxis("Horizontal");
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            // Logika membalik arah sprite
            if (horizontalInput > 0.01f)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else if (horizontalInput < -0.01f)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            
            // Atur animasi lari
            anim.SetBool("run", horizontalInput != 0);
        }
        else
        {
            // Jika sedang menyerang, paksa berhenti.
            body.velocity = new Vector2(0, body.velocity.y);
            anim.SetBool("run", false);
        }

        if (specialComboCooldownTimer > 0)
        specialComboCooldownTimer -= Time.deltaTime;
        // Serangan
        if (isUpgraded)
            HandleUpgradedAttacks();
        else
            HandleNormalAttack();

        // Animator
        //anim.SetBool("run", horizontalInput != 0 || verticalInput != 0);
        //anim.SetBool("grounded", grounded);
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
        if (Input.GetKeyDown(KeyCode.I) && !isPerformingCombo && specialComboCooldownTimer <= 0f)
    {
        StartCoroutine(SpecialCombo());
        specialComboCooldownTimer = specialComboCooldown; // mulai cooldown
    }

        if (Input.GetKeyDown(KeyCode.J) && attack1Timer <= 0f)
        {
            anim.SetTrigger("attack1");
            Attack();
            PlayAttackSound(attackSound); 
            attack1Timer = attack1Cooldown;
        }
        else if (Input.GetKeyDown(KeyCode.K) && attack2Timer <= 0f)
        {
            anim.SetTrigger("attack2");
            Attack();
            PlayAttackSound(attackSound); 
            attack2Timer = attack2Cooldown;
        }
        else if (Input.GetKeyDown(KeyCode.L) && attack3Timer <= 0f)
        {
            anim.SetTrigger("attack3");
            Attack();
            PlayAttackSound(attackSound); 
            attack3Timer = attack3Cooldown;
        }
    }
     private void PlayAttackSound(AudioClip clip)
    {
        // Gunakan PlayOneShot agar suara bisa tumpang tindih tanpa memotong satu sama lain
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private IEnumerator SpecialCombo()
    {
        isPerformingCombo = true;

        anim.SetTrigger("attack1");
        Attack();
        PlayAttackSound(attackSound);
        yield return new WaitForSeconds(specialComboDelay);

        anim.SetTrigger("attack3");
        Attack();
        PlayAttackSound(attackSound);
        yield return new WaitForSeconds(specialComboDelay);

        anim.SetTrigger("attack2");
        Attack();
        PlayAttackSound(attackSound);
        yield return new WaitForSeconds(specialComboDelay);

        anim.SetTrigger("attack1");
        Attack();
        PlayAttackSound(attackSound);
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