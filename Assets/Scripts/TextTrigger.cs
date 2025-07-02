using UnityEngine;
using TMPro; // WAJIB: Gunakan ini untuk TextMeshPro
using UnityEngine.UI;
using System.Collections;

public class TextTrigger : MonoBehaviour
{
    [Header("Referensi UI")]
    [Tooltip("Masukkan objek Canvas yang berisi panel informasi.")]
    [SerializeField] private GameObject infoCanvas;
    [Tooltip("Masukkan komponen TextMeshProUGUI untuk menampilkan teks.")]
    [SerializeField] private Text infoText;

    [Header("Konten Teks")]
    [Tooltip("Tulis pesan yang ingin ditampilkan di sini. Setiap baris baru akan dihitung.")]
    [TextArea(10, 20)] // Membuat area teks lebih besar di Inspector
    [SerializeField] private string message;

    [Header("Pengaturan Efek")]
    [Tooltip("Kecepatan efek mengetik (detik per karakter). Lebih kecil lebih cepat.")]
    [SerializeField] private float typingSpeed = 1f;
    [Tooltip("Jumlah baris maksimal yang ditampilkan sebelum layar dibersihkan.")]
    [SerializeField] private int maxLinesOnScreen = 3;
    [Tooltip("Jeda waktu (detik) sebelum halaman teks berikutnya muncul.")]
    [SerializeField] private float pagePauseDuration = 1.5f;
    [Tooltip("Jika true, trigger ini hanya akan aktif satu kali saja.")]
    [SerializeField] private bool triggerOnce = true;

    private bool hasBeenTriggered = false;
    private Coroutine displayCoroutine; // Untuk menyimpan referensi coroutine

    private void Awake()
    {
        // Pastikan canvas tidak aktif dan teks kosong saat game dimulai
        if (infoCanvas != null)
        {
            infoCanvas.SetActive(false);
        }
        if (infoText != null)
        {
            infoText.text = "";
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            ShowInfo();
            hasBeenTriggered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HideInfo();
        }
    }

    public void ShowInfo()
    {
        if (infoCanvas != null && infoText != null)
        {
            infoCanvas.SetActive(true);
            // Hentikan coroutine sebelumnya jika masih berjalan, lalu mulai yang baru
            if (displayCoroutine != null)
            {
                StopCoroutine(displayCoroutine);
            }
            displayCoroutine = StartCoroutine(DisplayMessagePaged(message));
        }
    }

    public void HideInfo()
    {
        if (infoCanvas != null)
        {
            // Hentikan coroutine saat canvas disembunyikan
            if (displayCoroutine != null)
            {
                StopCoroutine(displayCoroutine);
                displayCoroutine = null;
            }
            infoCanvas.SetActive(false);
        }
    }

    private IEnumerator DisplayMessagePaged(string fullMessage)
    {
        infoText.text = ""; // Selalu kosongkan teks di awal
        string[] lines = fullMessage.Split('\n'); // Pecah pesan menjadi baris-baris
        int linesDisplayed = 0;

        foreach (string line in lines)
        {
            // Jika sudah mencapai batas baris, jeda, lalu bersihkan layar
            if (linesDisplayed >= maxLinesOnScreen)
            {
                yield return new WaitForSeconds(pagePauseDuration); // Jeda agar pemain sempat membaca
                infoText.text = "";
                linesDisplayed = 0;
            }

            // Ketik baris saat ini, karakter per karakter
            foreach (char c in line.ToCharArray())
            {
                infoText.text += c;
                yield return new WaitForSeconds(typingSpeed);
            }

            infoText.text += "\n"; // Tambahkan baris baru setelah selesai mengetik
            linesDisplayed++;
        }
    }
}
