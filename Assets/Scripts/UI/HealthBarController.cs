using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private Image m_Image;

    private void Start()
    {
        PlayerStatsManager.Instance.OnHealthChanged.AddListener(HandleChange);
    }

    private void HandleChange(float val)
    {
        m_Image.fillAmount = val / PlayerStatsManager.Instance.MaxHealth;
    }
}
