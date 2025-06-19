using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private Animator anim;

    private bool grounded;
    [SerializeField] private float speed;
    private bool isUpgraded = false; // Melacak mode normal atau upgrade

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

        body.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, body.velocity.y);

        //Flip player when moving left-right
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

        // Deteksi serangan
        if (Input.GetKeyDown(KeyCode.J))
        {
            if(isUpgraded)
            {
                anim.SetTrigger("upgrade_attack");
            }
            else
            {
                anim.SetTrigger("attack");
            }
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            anim.SetTrigger("upgrade_attack2");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            anim.SetTrigger("upgrade_attack3");
        }


        // Deteksi upgrade (tombol I)
        if (Input.GetKeyDown(KeyCode.I))
        {
            isUpgraded = !isUpgraded; // Toggle antara normal dan upgrade
            anim.SetBool("IsUpgraded", isUpgraded); // Perbarui parameter Animator
        }

        // Set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("upgrade_run", horizontalInput != 0);
        anim.SetBool("grounded", grounded);
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