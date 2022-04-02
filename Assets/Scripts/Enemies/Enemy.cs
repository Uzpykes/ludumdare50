using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D), typeof(Animator))]
public class Enemy : MonoBehaviour
{
    private Animator m_Animator;
    [SerializeField] protected float m_AttackCooldown;
    protected float m_TimeSinceLastAttack = 100f;
    protected bool m_CanAttackPlayer;

    protected virtual void Awake()
    {
        m_Animator = GetComponent<Animator>();
        this.gameObject.SetActive(false);
    }

    protected virtual void Update()
    {
        if (m_CanAttackPlayer)
            Attack();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            m_CanAttackPlayer = true;
        }
    }


    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            m_CanAttackPlayer = false;
        }
    }

    private void Attack()
    {
        if (m_TimeSinceLastAttack > m_AttackCooldown)
        {
            m_Animator.SetTrigger("Attack");
            PlayerHealthManager.OnAttacked.Invoke(4f);

            m_TimeSinceLastAttack = 0f;
        }

    }

    public void HandleVisible(Biome biome)
    {
        biome.OnBiomeVisible.RemoveListener(HandleVisible);
        this.gameObject.SetActive(true);
    }

}
