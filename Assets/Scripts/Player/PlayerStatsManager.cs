using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance { get; private set; }
    
    [SerializeField] private float m_Health;
    [SerializeField] private float m_Heat;
    [SerializeField] private float m_Weight;
    [SerializeField] private float m_Fuel;

    public float Health { get => m_Health; set => m_Health = value; }
    public float Heat { get => m_Heat; set => m_Heat = value; }
    public float Weight { get => m_Weight; set => m_Weight = value; }
    public float Fuel { get => m_Fuel; set => m_Fuel = value; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

}
