using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    public float maxHealth => startingHealth;

    [SerializeField] private bool isEnemy = false;

    [Header("Prefab (For Enemy Only)")]
    [SerializeField] private GameObject healthCollectiblePrefab; // Optional untuk enemy

    [Header("Game Over")]
    [SerializeField] private GameOverManager gameOverManager;
    [SerializeField] private Transform defaultSpawnTransform;



    private Animator anim;

    private void Awake()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();

        // Auto detect jika tag-nya "Enemy"
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

            if (!isEnemy)
            {
                // Respawn player di checkpoint
                Vector3 checkpointPos = CheckpointManager.Instance.GetLastCheckpointPosition();
                if (checkpointPos == Vector3.zero && defaultSpawnTransform != null)
                {
                    checkpointPos = defaultSpawnTransform.position;
                }
                Transform checkpointRoom = CheckpointManager.Instance.GetCheckpointRoom();

                transform.position = checkpointPos;
                currentHealth = maxHealth;

                // Reset animasi dan state player
                // if (anim != null)
                anim.Rebind(); // reset animasi
                anim.SetTrigger("upgrade_idle");

                // Optional: atur ulang kamera ke ruangan checkpoint
                if (checkpointRoom != null)
                {
                    CameraController cam = FindObjectOfType<CameraController>();
                    if (cam != null)
                    {
                        cam.MoveToNewRoom(checkpointRoom, true); // langsung snap kamera
                    }
                }

                Debug.Log("Player respawned at checkpoint.");
            }

        }
    }

    // Dipanggil melalui Animation Event di akhir animasi 'die'
    public void DestroyAfterDeath()
    {
        if (isEnemy)
        {
            // Spawn item (jika prefab tersedia)
            Vector3 spawnPos = transform.position + new Vector3(0, -1.5f, 0);
            if (healthCollectiblePrefab != null)
            {
                Instantiate(healthCollectiblePrefab, spawnPos, Quaternion.identity);
            }

            // Hancurkan musuh (jika punya parent holder)
            if (transform.root != null)
            {
                Destroy(transform.root.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void AddHealth(float value)
    {
        currentHealth = Mathf.Clamp(currentHealth + value, 0, startingHealth);
    }

    public void SetGameOverManager(GameOverManager manager)
    {
        gameOverManager = manager;
    }

}