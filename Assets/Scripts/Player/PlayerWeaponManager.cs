using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerWeaponManager : MonoBehaviour
{
    public static PlayerWeaponManager Instance { get; private set; }

    public WeaponType ActiveWeapon { get => m_ActiveWeapon; set { m_ActiveWeapon = value; OnActiveWeaponChanged.Invoke(value); } }
    public bool CanUseActiveWeapon { get => m_CanUseActiveWeapon; set => m_CanUseActiveWeapon = value; }

    private WeaponType m_ActiveWeapon;
    private bool m_CanUseActiveWeapon;

    [SerializeField] private int m_HandgunAmmo;
    [SerializeField] private int m_MachinegunAmmo;
    [SerializeField] private int m_RocketLauncherAmmo;
    [SerializeField] private int m_GrenadeAmmo;

    public int HandgunAmmo { get => m_HandgunAmmo; set { m_HandgunAmmo = value; OnAmmoCountChanged.Invoke(WeaponType.Handgun, value); } }
    public int MachinegunAmmo { get => m_MachinegunAmmo; set { m_MachinegunAmmo = value; OnAmmoCountChanged.Invoke(WeaponType.Machinegun, value); } }
    public int RocketLauncherAmmo { get => m_RocketLauncherAmmo; set { m_RocketLauncherAmmo = value; OnAmmoCountChanged.Invoke(WeaponType.RocketLauncher, value); } }
    public int GrenadeAmmo { get => m_GrenadeAmmo; set { m_GrenadeAmmo = value; OnAmmoCountChanged.Invoke(WeaponType.Grenade, value); } }

    [NonSerialized] public UnityEvent<WeaponType> OnActiveWeaponChanged;
    [NonSerialized] public UnityEvent<WeaponType, int> OnAmmoCountChanged;

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
        OnActiveWeaponChanged = new UnityEvent<WeaponType>();
        OnAmmoCountChanged = new UnityEvent<WeaponType, int>();
    }
}

public enum WeaponType
{
    Handgun,
    Machinegun,
    RocketLauncher,
    Grenade
}

public static class WeaponTypeExtensions
{
    public static WeaponType Next(this WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.Handgun:
                return WeaponType.Machinegun;
            case WeaponType.Machinegun:
                return WeaponType.RocketLauncher;
            case WeaponType.RocketLauncher:
                return WeaponType.Grenade;
            case WeaponType.Grenade:
                return WeaponType.Handgun;
            default:
                return default;
        }
    }

    public static WeaponType Previous(this WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponType.Handgun:
                return WeaponType.Grenade;
            case WeaponType.Grenade:
                return WeaponType.RocketLauncher;
            case WeaponType.RocketLauncher:
                return WeaponType.Machinegun;
            case WeaponType.Machinegun:
                return WeaponType.Handgun;
            default:
                return default;
        }
    }
}
