using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Cutscenekelevel1 : MonoBehaviour
{
    public string nextSceneName = "Level1"; // Nama scene tujuan

    // Fungsi ini akan dipanggil ketika Signal Emitter memancarkan signal
    public void OnCutsceneFinished()
    {
        Debug.Log("Cutscene selesai! Memuat scene: " + nextSceneName);
        SceneManager.LoadScene(nextSceneName);
    }
}