using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingBehaviour : MonoBehaviour
{
    [SerializeField] private Transform m_ShootingPoint;
    [SerializeField] private Projectile m_ProjectilePrefab;
    [SerializeField] private float m_ProjectileSpeed;

    [SerializeField] private float m_ShootingCooldown;
    private float m_TimeSinceLastShot;

    private void Update()
    {
        m_TimeSinceLastShot += Time.deltaTime;
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButton(0))
        {
            SpawnProjectile();
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

    private void SpawnProjectile()
    {
        if (m_TimeSinceLastShot < m_ShootingCooldown)
            return;

        m_TimeSinceLastShot = 0f;

        var worldPoint = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        var projectile = Instantiate(m_ProjectilePrefab, m_ShootingPoint.transform.position, Quaternion.identity);
        projectile.Prepare(worldPoint - (Vector2)m_ShootingPoint.position, LayerMask.GetMask("Enemy", "Ground"), 2f, m_ProjectileSpeed);
    }

}
