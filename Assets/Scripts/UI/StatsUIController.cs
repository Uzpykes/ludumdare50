using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_ZombieKillsText;
    [SerializeField] private TextMeshProUGUI m_MetersTraveledText;

    private void Start()
    {
        PlayerStatsManager.Instance.OnZombiesKilled.AddListener(HandleKillsChange);
        PlayerStatsManager.Instance.OnDistanceTraveledChanged.AddListener(HandleDistanceChange);

        HandleKillsChange(PlayerStatsManager.Instance.ZombiesKilled);
        HandleDistanceChange(PlayerStatsManager.Instance.DistanceTraveled);
    }

    void HandleKillsChange(int kills)
    {
        m_ZombieKillsText.text = $"{kills} x";
    }

    void HandleDistanceChange(float dist)
    {
        m_MetersTraveledText.text = $"{Mathf.RoundToInt(dist)} m";
    }
}
