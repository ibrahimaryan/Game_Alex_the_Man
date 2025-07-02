using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScenePositioner : MonoBehaviour
{
    [SerializeField] private string targetScene = "Level2";
    [SerializeField] private Vector2 spawnPosition;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == targetScene)
        {
            transform.position = spawnPosition;
        }
    }
}