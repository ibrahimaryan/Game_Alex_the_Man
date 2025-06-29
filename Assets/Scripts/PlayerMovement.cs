using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator anim;

    private bool grounded;
    [SerializeField] private float speed;
    private bool isUpgraded = false; // Melacak mode normal atau upgrade
    private bool isProtect = false; // Melacak apakah sedang dalam mode proteksi
    [SerializeField] private float attackDamage = 1f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private LayerMask enemyLayer;
    private void Attack()
    {
        // Deteksi enemy di sekitar player (misal pakai overlap circle)
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
    private void Awake()
    {
        // Get reference for rigidbody from object
        body = GetComponent<Rigidbody2D>();

        // Get reference for animator from object
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        // Flip player when moving left-right
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (horizontalInput > 0.01f)
        {
            sr.flipX = false; // Hadap kanan
        }
        else if (horizontalInput < -0.01f)
        {
            sr.flipX = true; // Hadap kiri
        }

        // Deteksi lompat
        if (Input.GetKey(KeyCode.Space)&& isUpgraded)
        {
            Jump();
            anim.SetTrigger("jump");
        }

        // Deteksi serangan normal (Jika tidak upgrade)
        if (Input.GetKeyDown(KeyCode.J) && !isUpgraded)
        {
            anim.SetTrigger("attack");
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.S) && !isUpgraded && !isProtect)
        {
            isProtect = true;
            anim.SetTrigger("protect");
            anim.SetBool("isProtect", isProtect);
        }
        else if (Input.GetKeyUp(KeyCode.S) && isProtect)
        {
            isProtect = false;
            anim.SetBool("isProtect", isProtect);
        }
        {
            anim.SetTrigger("protect");
            anim.SetBool("isProtect", isProtect);
        }

        // Deteksi upgrade (tombol I)
        if (Input.GetKeyDown(KeyCode.I))
        {
            isUpgraded = !isUpgraded;
            anim.SetBool("IsUpgraded", isUpgraded);
        }

        // Deteksi serangan upgrade (hanya jika isUpgraded)
        if (isUpgraded)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                anim.SetTrigger("attack1");
                Attack();
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                anim.SetTrigger("attack2");
                Attack();
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                anim.SetTrigger("attack3");
                Attack();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                isProtect = true;
                anim.SetTrigger("protect");
                anim.SetBool("isProtect", isProtect);
            }
            else if (Input.GetKeyUp(KeyCode.S) && isProtect)
            {
                isProtect = false;
                anim.SetBool("isProtect", isProtect);
            }
        }

        // Set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        //anim.SetBool("grounded", grounded);
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, speed);
        grounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }
}