using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator anim;

    private bool grounded;
    [SerializeField] private float speed;
    private bool isUpgraded = false; // Melacak mode normal atau upgrade
    private bool isProtect = false; // Melacak apakah sedang dalam mode proteksi
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
        if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(2, 2, 1);
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-2, 2, 1);
        }

        // Deteksi lompat
        if (Input.GetKey(KeyCode.Space))
        {
            Jump();
        }

        // Deteksi serangan normal (Jika tidak upgrade)
        if (Input.GetKeyDown(KeyCode.J) && !isUpgraded)
        {
            anim.SetTrigger("attack");
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
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                anim.SetTrigger("attack2");
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                anim.SetTrigger("attack3");
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