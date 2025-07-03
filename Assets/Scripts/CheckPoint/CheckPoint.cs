using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Transform roomTransform; 
    private bool activated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activated && collision.CompareTag("Player"))
        {
            CheckpointManager.Instance.SetCheckPoint(transform.position);
            CheckpointManager.Instance.SetCheckpointRoom(roomTransform);
            Debug.Log("Checkpoint set at: " + transform.position);
            activated = true;
        }
    }
}
