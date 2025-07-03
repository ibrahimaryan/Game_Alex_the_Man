using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlayerScenePositioner : MonoBehaviour
{
    [System.Serializable]
    public class SceneSpawn
    {
        public string sceneName;
        public Vector2 spawnPosition;
    }

    [SerializeField] private List<SceneSpawn> sceneSpawns;

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
        foreach (var spawn in sceneSpawns)
        {
            if (spawn.sceneName == scene.name)
            {
                transform.position = spawn.spawnPosition;
                break;
            }
        }
    }
}