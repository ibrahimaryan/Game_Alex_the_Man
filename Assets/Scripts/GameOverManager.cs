using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("UI Panel")]
    [SerializeField] private GameObject gameOverPanel;

    [Header("Respawn")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform defaultSpawnPoint;

    private void Start()
    {
        Time.timeScale = 1f;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void TriggerGameOver()
    {
        Debug.Log("☠️ Player mati, Game Over panel muncul");
        Time.timeScale = 0f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void Retry()
    {
        Time.timeScale = 1f; // Pastikan ini ada dan aktif
        // Hapus player lama (jika masih ada)
        GameObject oldPlayer = GameObject.FindGameObjectWithTag("Player");
        if (oldPlayer != null)
        {
            Destroy(oldPlayer);
        }

        // Ambil checkpoint atau spawn awal
        Vector3 spawnPos = defaultSpawnPoint.position;
        if (CheckpointManager.Instance != null)
        {
            Vector3 cp = CheckpointManager.Instance.GetLastCheckpointPosition();
            if (cp != Vector3.zero)
                spawnPos = cp;
        }

        // Spawn ulang player
        GameObject newPlayer = Instantiate(playerPrefab, spawnPos, Quaternion.identity);

        // Sambungkan GameOverManager ke Health player baru
        Health health = newPlayer.GetComponent<Health>();
        if (health != null)
        {
            health.SetGameOverManager(this);
        }

        // 👉 Tambahkan ini agar kamera langsung pindah ke ruangan checkpoint
        if (CheckpointManager.Instance != null && Camera.main != null)
        {
            Transform checkpointRoom = CheckpointManager.Instance.GetCheckpointRoom();
            if (checkpointRoom != null)
            {
                Camera.main.GetComponent<CameraController>().SnapToRoom(checkpointRoom);
            }
        }

        // Tutup panel game over
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);


    }


    public void ExitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
