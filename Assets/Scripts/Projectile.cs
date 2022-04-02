using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody;
    private LayerMask m_CollisionMask;

    private Vector3 m_Direction;

    private float m_TimeSinceSpawn;
    private float m_Lifetime;
    private bool m_Prepared = false;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleLifetime();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    public void Prepare(Vector2 direction, LayerMask collisionMask, float lifetime, float speed)
    {
        m_Direction = direction.normalized * speed;
        m_Lifetime = lifetime;
        m_CollisionMask = collisionMask;

        m_Prepared = true;
    }

    private void HandleMovement()
    {
        m_Rigidbody.velocity = m_Direction;
    }

    private void HandleLifetime()
    {
        if (!m_Prepared)
            return;

        m_TimeSinceSpawn += Time.deltaTime;
        if (m_TimeSinceSpawn > m_Lifetime)
            Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_CollisionMask == (m_CollisionMask | (1 << collision.gameObject.layer)))
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }

}
