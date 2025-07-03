using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BacksoundManager : MonoBehaviour
{
    public static BacksoundManager instance;
    private AudioSource audioSource;
    // Start is called before the first frame update
    private void Awake()
    {
        // --- Pola Singleton ---
        // Cek apakah sudah ada instance MusicManager lain di scene.
        if (instance == null)
        {
            // Jika belum ada, jadikan objek ini sebagai instance utama.
            instance = this;
            
            // Perintah KUNCI: Jangan hancurkan objek ini saat scene baru dimuat.
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Jika sudah ada instance lain (misalnya saat kembali ke menu utama),
            // hancurkan objek duplikat ini agar tidak ada dua musik yang berjalan bersamaan.
            Destroy(gameObject);
            return;
        }

        // Dapatkan komponen AudioSource yang terpasang pada objek ini.
        audioSource = GetComponent<AudioSource>();
    }

    // Fungsi ini bisa dipanggil dari skrip lain untuk mengganti musik
    // Misalnya, untuk musik boss yang lebih tegang.
    public void ChangeMusic(AudioClip newMusic)
    {
        // Cek agar tidak mengganti musik dengan klip yang sama.
        if (audioSource.clip == newMusic)
        {
            return;
        }

        audioSource.Stop();
        audioSource.clip = newMusic;
        audioSource.Play();
    }
}
