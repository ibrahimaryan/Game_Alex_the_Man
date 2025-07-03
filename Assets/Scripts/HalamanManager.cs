using UnityEngine;
using UnityEngine.SceneManagement;

public class HalamanManager : MonoBehaviour
{
    // Start is called before the first frame update
    public string escapeScene;
    public bool isEscapeToExit;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {         
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (isEscapeToExit)
            {
                Application.Quit();
            }
            else
            {
                Debug.Log("Name Scene: " + escapeScene);
                SceneManager.LoadScene(escapeScene);
            }
        }
    }

    public void MulaiPermainan() {
        SceneManager.LoadScene("CutScene");
    }
    
    public void MulaiPermainan1() {
        SceneManager.LoadScene("Level1");
    }

    public void MulaiPermainan2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void MulaiPermainan3() {
        SceneManager.LoadScene("Level3");
    }

    public void MulaikeMenuUtama() {
        SceneManager.LoadScene("MainMenu");
    }

    public void Credits() {
        SceneManager.LoadScene("Credits");
    }
    
    public void Escape() {
        SceneManager.LoadScene("Keluar");
    }

    public void Keluar()
    {
        Application.Quit();
    }
}