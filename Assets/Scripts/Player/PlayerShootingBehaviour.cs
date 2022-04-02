using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootingBehaviour : MonoBehaviour
{
    [SerializeField] private Transform m_ShootingPoint;
    [SerializeField] private Projectile m_ProjectilePrefab;
    [SerializeField] private float m_ProjectileSpeed; // should be upgradeable

    [SerializeField] private float m_ShootingCooldown; // should be upgradeable
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
