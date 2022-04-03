using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance { get; private set; }

    [SerializeField] private float m_MaxHealth;
    [SerializeField] private float m_Health;
    [SerializeField] private float m_MaxHeat;
    [SerializeField] private float m_Heat;
    [SerializeField] private float m_Weight;
    [SerializeField] private float m_MaxFuel;
    [SerializeField] private float m_Fuel;
    [SerializeField] private float m_FuelUseRate;
    private float m_VerticalSpeed;
    [SerializeField] private float m_HeatEfficency; // How much weight one unit of heat can carry

    public float MaxHealth { get => m_MaxHealth; set => m_MaxHealth = value; }
    public float Health { get => m_Health; set { m_Health = value; OnHealthChanged.Invoke(value); } }
    public float MaxHeat { get => m_MaxHeat; set => m_MaxHeat = value; }
    public float Heat { get => m_Heat; set { m_Heat = value; OnHeatChanged.Invoke(value); } }
    public float Weight { get => m_Weight; set { m_Weight = value; OnWeightChanged.Invoke(value); } }
    public float MaxFuel { get => m_MaxFuel; set => m_MaxFuel = value; }
    public float Fuel { get => m_Fuel; set { m_Fuel = value; OnFuelChanged.Invoke(value); } }
    public float HeatEfficency { get => m_HeatEfficency; set => m_HeatEfficency = value; }
    public float FuelUseRate { get => m_FuelUseRate; set => m_FuelUseRate = value; }

    public float VerticalSpeed { get => m_VerticalSpeed; set { m_VerticalSpeed = value; OnVerticalSpeedChanged.Invoke(value); } }

    [NonSerialized] public UnityEvent<float> OnHealthChanged;
    [NonSerialized] public UnityEvent<float> OnHeatChanged;
    [NonSerialized] public UnityEvent<float> OnWeightChanged;
    [NonSerialized] public UnityEvent<float> OnFuelChanged;
    [NonSerialized] public UnityEvent<float> OnVerticalSpeedChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            CreateEvents();
        }
    }

    private void CreateEvents()
    {
        OnHealthChanged = new UnityEvent<float>();
        OnHeatChanged = new UnityEvent<float>();
        OnWeightChanged = new UnityEvent<float>();
        OnFuelChanged = new UnityEvent<float>();
        OnVerticalSpeedChanged = new UnityEvent<float>();
    }

}

public class PlayerInventory
{
    public InventoryItem SandBag;
    public InventoryItem SmallPackage;
    public InventoryItem MediumPackage;
    public InventoryItem LargePackage;
}

[System.Serializable]
public struct InventoryItem
{
    public string Name;
    public Sprite Sprite;
    public float ItemWeight;
    public float ItemCount;   
}
