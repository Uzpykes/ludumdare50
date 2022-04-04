using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private ProjectileType m_ProjectileType;
    [SerializeField] private Projectile m_ExplosionPrefab;

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

    private void Start()
    {
        GameOverManager.Instance.OnGameOver.AddListener(() =>
        {
            this.gameObject.SetActive(false);
        });
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

        if (m_ProjectileType == ProjectileType.Grenade)
            m_Rigidbody.velocity = m_Direction;
    }

    private void HandleMovement()
    {
        if (m_ProjectileType == ProjectileType.Grenade)
            return;

        m_Rigidbody.velocity = m_Direction;
    }

    private void HandleLifetime()
    {
        if (!m_Prepared)
            return;

        m_TimeSinceSpawn += Time.deltaTime;
        if (m_TimeSinceSpawn > m_Lifetime)
            Die();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_ProjectileType == ProjectileType.Grenade)
            return;

        if (m_CollisionMask == (m_CollisionMask | (1 << collision.gameObject.layer)))
        {
            Die();
        }
    }

    private void Die()
    {
        switch(m_ProjectileType)
        {
            case ProjectileType.Grenade:
            case ProjectileType.Rocket:
                Explode();
                break;
            case ProjectileType.Simple:
                SimpleDestroy();
                break;
            case ProjectileType.Explosion:
                DestroyExplosion();
                break;
        }
    }

    private void SimpleDestroy()
    {
        Destroy(this.gameObject);
    }

    private void Explode()
    {
        AudioManager.Instance.PlayRandomExplossionClip();
        var pr = Instantiate(m_ExplosionPrefab, transform.position, Quaternion.identity);
        pr.Prepare(Vector2.zero, m_CollisionMask, .1f, 0f);
        Destroy(this.gameObject);
        //Spawn explosion
    }

    private void DestroyExplosion()
    {
        var ps = this.gameObject.GetComponentInChildren<ParticleSystem>();
        if (ps != null)
        {
            ps.transform.parent = this.transform.parent;
        }
        Destroy(this.gameObject, 0.1f);
    }

}

public enum ProjectileType
{
    Simple,
    Rocket,
    Explosion,
    Grenade
}
