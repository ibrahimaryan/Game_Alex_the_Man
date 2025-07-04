using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BacksoundManager : MonoBehaviour
{
    public static BacksoundManager instance;
    private AudioSource audioSource;

    private readonly HashSet<string> allowedScenes = new HashSet<string> { "Level1", "Level2", "Level3" };

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "LastScene"|| SceneManager.GetActiveScene().name == "MainMenu")
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            SceneManager.sceneLoaded += OnSceneLoaded;
            CheckSceneAndPlay(SceneManager.GetActiveScene().name);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckSceneAndPlay(scene.name);
    }

    private void CheckSceneAndPlay(string sceneName)
    {
        if (allowedScenes.Contains(sceneName))
        {
            if (audioSource != null && !audioSource.isPlaying && audioSource.clip != null)
                audioSource.Play();
        }
        else
        {
            if (audioSource != null && audioSource.isPlaying)
                audioSource.Stop();
        }
    }

    public void ChangeMusic(AudioClip newMusic)
    {
        if (audioSource.clip == newMusic)
        {
            return;
        }

        audioSource.Stop();
        audioSource.clip = newMusic;
        // Hanya play jika di scene yang diizinkan
        if (allowedScenes.Contains(SceneManager.GetActiveScene().name))
            audioSource.Play();
    }
}