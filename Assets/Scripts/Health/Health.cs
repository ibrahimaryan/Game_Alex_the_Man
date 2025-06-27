using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    [SerializeField] private bool isEnemy = false;
    public float currentHealth { get; private set; }
    private Animator anim;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();

        // Kalau lupa centang, tapi pakai tag "Enemy"
        if (CompareTag("Enemy"))
            isEnemy = true;
    }

    public void terkenaDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);
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
}
