using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelBarController : MonoBehaviour
{
    [SerializeField] private Image m_Image;

    private void Start()
    {
        PlayerStatsManager.Instance.OnFuelChanged.AddListener(HandleChange);
    }

    private void HandleChange(float val)
    {
        m_Image.fillAmount = val / PlayerStatsManager.Instance.MaxFuel;
    }
}
