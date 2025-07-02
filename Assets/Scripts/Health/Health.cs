using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    [SerializeField] private bool isEnemy = false;
    public float currentHealth { get; private set; }
    private Animator anim;
    public float maxHealth => startingHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();

        // Kalau lupa centang, tapi pakai tag "Enemy"
        if (CompareTag("Enemy"))
            isEnemy = true;
    }

    public void terkenaDamage(float damage)
    {
        // Jika ini player dan sedang protect, tidak menerima damage
        PlayerMovement player = GetComponent<PlayerMovement>();
        if (player != null && player.IsProtecting())
        {
            return;
        }
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
        }
        else
        {
            anim.SetTrigger("die");
        }
    }
    
    // Dipanggil di akhir animasi 'die' (pakai Animation Event)
    public void DestroyAfterDeath()
    {
        if (isEnemy && transform.root != null)
        {
            Destroy(transform.root.gameObject);
        }
    }

    public void AddHealth(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0, startingHealth);
    }
}
