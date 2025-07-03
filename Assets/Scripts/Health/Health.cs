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
    [SerializeField] private GameObject healthCollectiblePrefab; // Drag prefab dari Inspector

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

            if(!isEnemy) {
                Invoke(nameof(Respawn), 1f); // Respawn player setelah 1 detik
            }
        }
    }
    
    // Dipanggil di akhir animasi 'die' (pakai Animation Event)
    public void DestroyAfterDeath()
    {
         if (isEnemy)
        {
            // Spawn health collectible di posisi musuh sebelum dihancurkan
            Vector3 spawnPos = transform.position + new Vector3(0, -1.5f, 0);
            if (healthCollectiblePrefab != null)
            {
                Instantiate(healthCollectiblePrefab, spawnPos, Quaternion.identity);
            }

            // Hancurkan musuh
            if (transform.root != null)
            {
                Destroy(transform.root.gameObject);
            }
        }
    }

    public void AddHealth(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0, startingHealth);
    }

    public void Respawn() {
        transform.position = CheckpointManager.Instance.GetLastCheckpointPosition();
        currentHealth = startingHealth;
    }
}
