using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HeatBarController : MonoBehaviour
{
    [SerializeField] private Image m_HeatImage;
    [SerializeField] private Slider m_WeightSlider;
    [SerializeField] private TextMeshProUGUI m_WeightText;

    [SerializeField] private Gradient m_SpeedGradient;

    private void Start()
    {
        PlayerStatsManager.Instance.OnHeatChanged.AddListener(HandleHeatChange);
        PlayerStatsManager.Instance.OnWeightChanged.AddListener(HandleWeightChange);
        PlayerStatsManager.Instance.OnVerticalSpeedChanged.AddListener(HandleVerticalSpeedChange);

        HandleWeightChange(PlayerStatsManager.Instance.Weight);
    }

    private void HandleHeatChange(float val)
    {
        m_HeatImage.fillAmount = val / PlayerStatsManager.Instance.MaxHeat;
    }

    private void HandleWeightChange(float val)
    {
        m_WeightSlider.value = val / (PlayerStatsManager.Instance.MaxHeat * PlayerStatsManager.Instance.HeatEfficency);
        m_WeightText.text = Mathf.RoundToInt(val).ToString();
    }

    private void HandleVerticalSpeedChange(float val)
    {
        var normalized = (val / 2f) + .5f; // -1; +1 are biggest extremes

        m_HeatImage.color = m_SpeedGradient.Evaluate(normalized);
    }

}
