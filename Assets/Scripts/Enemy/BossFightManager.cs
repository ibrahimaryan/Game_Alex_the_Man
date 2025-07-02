using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Jika ingin restart level

public class BossFightManager : MonoBehaviour
{
    [SerializeField] private float timeLimit = 300f; // 5 menit = 300 detik
    [SerializeField] private TextMeshProUGUI waktuText;
    [SerializeField] private GameObject timerCanvas;

    private float timer;
    private bool fightStarted = false;
    private bool playerLost = false;

    private void Start()
    {
        if (waktuText != null)
        {
            waktuText.text = "00:00";
        }
    }

    private void Update()
    {
        if (fightStarted && !playerLost)
        {
            timer -= Time.deltaTime;
            timer = Mathf.Max(timer, 0f); // Hindari negatif

            // Format waktu: menit:detik
            int menit = Mathf.FloorToInt(timer / 60);
            int detik = Mathf.FloorToInt(timer % 60);
            waktuText.text = menit.ToString("00") + ":" + detik.ToString("00");

            if (timer <= 0)
            {
                playerLost = true;
                HandlePlayerLose();
            }
        }
    }

    public void StartBossFight()
    {
        timer = timeLimit;
        fightStarted = true;
        playerLost = false;

        // Bisa tambahkan musik boss atau cutscene
        Debug.Log("Pertarungan dimulai!");
    }

    private void HandlePlayerLose()
    {
        Debug.Log("Waktu habis! Player kalah.");

        // Sembunyikan UI timer
        if (timerCanvas != null)
            timerCanvas.SetActive(false);

        // Kalau pakai sistem checkpoint, panggil respawn di sini
        // Kalau pakai scene reload, pastikan canvas off saat Start
    }
}