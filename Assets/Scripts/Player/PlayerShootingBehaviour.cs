using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingBehaviour : MonoBehaviour
{
    [SerializeField] private Transform m_ShootingPoint;
    [SerializeField] private Projectile m_ProjectilePrefab;
    [SerializeField] private Projectile m_RocketPrefab;
    [SerializeField] private Projectile m_GrenadePrefab;

    [SerializeField] private float m_HandgunCooldown;
    [SerializeField] private float m_MachinegunCooldown;
    [SerializeField] private float m_RocketlauncherCooldown;
    [SerializeField] private float m_GrenadeCooldown;

    private float m_TimeSinceLastShot;

    private void Start()
    {
        GameOverManager.Instance.OnGameOver.AddListener(() =>
        {
            this.gameObject.SetActive(false);
        });
    }

    private void Update()
    {
        m_TimeSinceLastShot += Time.deltaTime;
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButton(0))
        {
            Shoot();
        }

        if (Input.mouseScrollDelta.y > 0f)
        {
            PreviousWeapon();
        }
        else if (Input.mouseScrollDelta.y < 0f)
        {
            NextWeapon();
        }
    }

    private void NextWeapon()
    {
        PlayerWeaponManager.Instance.ActiveWeapon = PlayerWeaponManager.Instance.ActiveWeapon.Next();
    }

    private void PreviousWeapon()
    {
        PlayerWeaponManager.Instance.ActiveWeapon = PlayerWeaponManager.Instance.ActiveWeapon.Previous();
    }

    private void Shoot()
    {
        switch(PlayerWeaponManager.Instance.ActiveWeapon)
        {
            case WeaponType.Grenade:
                SpawnGrenade(5f, m_GrenadeCooldown);
                break;
            case WeaponType.Handgun:
                SpawnProjectile(12f, m_HandgunCooldown);
                break;
            case WeaponType.Machinegun:
                SpawnProjectile(17f, m_MachinegunCooldown);
                break;
            case WeaponType.RocketLauncher:
                SpawnRocket(20f, m_RocketlauncherCooldown);
                break;
        }

    }

    private void SpawnProjectile(float projectileSpeed, float shotCooldown)
    {
        if (m_TimeSinceLastShot < shotCooldown)
            return;

        if (PlayerWeaponManager.Instance.ActiveWeapon == WeaponType.Handgun)
        {
            if (PlayerWeaponManager.Instance.HandgunAmmo <= 0)
                return;

            PlayerWeaponManager.Instance.HandgunAmmo--;
            AudioManager.Instance.PlayRandomHandgunClip();
        }
        else if (PlayerWeaponManager.Instance.ActiveWeapon == WeaponType.Machinegun)
        {
            if (PlayerWeaponManager.Instance.MachinegunAmmo <= 0)
                return;

            PlayerWeaponManager.Instance.MachinegunAmmo--;
            AudioManager.Instance.PlayRandomMachinegunClip();
        }

        m_TimeSinceLastShot = 0f;

        var worldPoint = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var projectile = Instantiate(m_ProjectilePrefab, m_ShootingPoint.transform.position, Quaternion.identity);
        projectile.Prepare(worldPoint - (Vector2)m_ShootingPoint.position, LayerMask.GetMask("Enemy", "Ground"), 2f, projectileSpeed);
    }

    private void SpawnRocket(float projectileSpeed, float shotCooldown)
    {
        if (m_TimeSinceLastShot < shotCooldown)
            return;

        if (PlayerWeaponManager.Instance.RocketLauncherAmmo <= 0)
            return;

        PlayerWeaponManager.Instance.RocketLauncherAmmo--;
        AudioManager.Instance.PlayRandomRocketClip();

        m_TimeSinceLastShot = 0f;

        var worldPoint = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var projectile = Instantiate(m_RocketPrefab, m_ShootingPoint.transform.position, Quaternion.identity);
        projectile.Prepare(worldPoint - (Vector2)m_ShootingPoint.position, LayerMask.GetMask("Enemy", "Ground"), 2f, projectileSpeed);
    }

    private void SpawnGrenade(float projectileSpeed, float shotCooldown)
    {
        if (m_TimeSinceLastShot < shotCooldown)
            return;

        if (PlayerWeaponManager.Instance.GrenadeAmmo <= 0)
            return;

        PlayerWeaponManager.Instance.GrenadeAmmo--;

        m_TimeSinceLastShot = 0f;

        var worldPoint = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var projectile = Instantiate(m_GrenadePrefab, m_ShootingPoint.transform.position, Quaternion.identity);
        projectile.Prepare(worldPoint - (Vector2)m_ShootingPoint.position, LayerMask.GetMask("Enemy", "Ground"), 2f, projectileSpeed);
    }

}
