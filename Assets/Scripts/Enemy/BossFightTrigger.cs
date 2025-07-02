using UnityEngine;

public class BossFightTrigger : MonoBehaviour
{
    [SerializeField] private GameObject timerCanvas;

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered && collision.CompareTag("Player"))
        {
            triggered = true;

            // Aktifkan canvas timer
            if (timerCanvas != null)
                timerCanvas.SetActive(true);

            // Mulai pertarungan boss
            FindObjectOfType<BossFightManager>().StartBossFight();
        }
    }
}