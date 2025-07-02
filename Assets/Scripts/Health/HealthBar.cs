using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image healthBarTotal;
    [SerializeField] private Image healthBarCurrent;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        healthBarTotal.fillAmount = 1f;
    }

    // Update is called once per frame
    private void Update()
    {
        healthBarCurrent.fillAmount = playerHealth.currentHealth / playerHealth.maxHealth;
    }
}
