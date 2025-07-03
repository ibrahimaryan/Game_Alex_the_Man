using UnityEngine;

public class PauseGame : MonoBehaviour
{
    public GameObject pausePanel;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGameNow();
        }
    }

    public void PauseGameNow()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // public void ExitGame()
    // {
    //     Time.timeScale = 1f;
    //     SceneManager.LoadScene("MainMenu"); // Ganti sesuai nama scene utama
    // }

}
