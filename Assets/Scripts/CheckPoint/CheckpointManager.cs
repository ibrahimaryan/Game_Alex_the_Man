using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; }
    private Vector3 lastCheckpointPosition;
    private Transform lastCheckpointRoom;

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "LastScene"|| SceneManager.GetActiveScene().name == "MainMenu")
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCheckPoint(Vector3 position)
    {
        lastCheckpointPosition = position;
    }

    public Vector3 GetLastCheckpointPosition()
    {
        return lastCheckpointPosition;
    }

    public void SetCheckpointRoom(Transform room)
    {
        lastCheckpointRoom = room;
    }

    public Transform GetCheckpointRoom()
    {
        return lastCheckpointRoom;
    }
}
