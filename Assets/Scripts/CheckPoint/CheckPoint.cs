using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private bool activated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CheckpointManager.Instance.SetCheckPoint(transform.position);
            Debug.Log("Checkpoint set at: " + transform.position);
            activated = true;
        }
    }
}
