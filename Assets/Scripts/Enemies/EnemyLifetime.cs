using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyLifetime : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody2d;
    private Enemy m_EnemyControler;
    private Animator m_Animator;

    private bool m_MarkedToDie = false;

    private void Awake()
    {
        m_Rigidbody2d = GetComponent<Rigidbody2D>();
        m_EnemyControler = GetComponent<Enemy>();
        m_Animator = GetComponent<Animator>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerProjectile"))
        {
            Die(collision);
        }
    }

    private void Die(Collider2D collider)
    {
        if (m_MarkedToDie)
            return;

        m_MarkedToDie = true;

        m_Animator.SetTrigger("Die");
        this.gameObject.layer = LayerMask.NameToLayer("DeadEnemy");
        m_EnemyControler.enabled = false;
        m_Rigidbody2d.bodyType = RigidbodyType2D.Dynamic;
        m_Rigidbody2d.freezeRotation = false;

        var randomPoint = (Vector2)Random.onUnitSphere.normalized;
        randomPoint.y = Mathf.Abs(randomPoint.y);

        m_Rigidbody2d.AddForce(5f * randomPoint, ForceMode2D.Impulse);

        PlayerStatsManager.Instance.ZombiesKilled++;

        Destroy(this.gameObject, .6f);
    }

}
