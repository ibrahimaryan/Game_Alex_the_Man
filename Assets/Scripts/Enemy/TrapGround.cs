using System.Collections;
using UnityEngine;

public class TrapGround : MonoBehaviour
{
    [SerializeField] private float damage;

    [Header("Trap Ground Timer")]
    [SerializeField] private float activationDelay = 1f;
    [SerializeField] private float activeTime = 2f;

    private Animator anim;
    private SpriteRenderer spriteRend;
    private bool triggerred;   // Apakah trap sedang aktif (sedang menunggu atau menyala)
    private bool active;       // Trap sedang menyala (dan bisa memberi damage)
    private bool sudahKena;    // Supaya player hanya kena damage sekali per aktivasi

    private void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Triggered by player");

            if (!triggerred)
            {
                StartCoroutine(ActiveTrap());
            }

            // Jika trap sedang aktif dan player belum terkena, berikan damage
            if (active && !sudahKena)
            {
                var health = collision.GetComponent<Health>();
                if (health != null)
                {
                    Debug.Log("Trap is active, dealing damage to player");
                    health.terkenaDamage(damage);
                    sudahKena = true; // Cegah damage ganda
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && active && !sudahKena)
        {
            var health = collision.GetComponent<Health>();
            if (health != null)
            {
                Debug.Log("Trap is active, dealing damage to player (Stay)");
                health.terkenaDamage(damage);
                sudahKena = true;
            }
        }
    }

    private IEnumerator ActiveTrap()
    {
        triggerred = true;
        spriteRend.color = Color.red;

        yield return new WaitForSeconds(activationDelay);

        spriteRend.color = Color.white;
        active = true;
        anim.SetBool("activated", true);
        sudahKena = false; // Reset supaya player bisa kena damage lagi saat trap aktif

        yield return new WaitForSeconds(activeTime);

        active = false;
        triggerred = false;
        anim.SetBool("activated", false);
    }
}