using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : Enemy
{
    [SerializeField] private float m_GroundSpeed;
    private Transform m_Player;
    private Rigidbody2D m_Rigidbody2d;

    protected override void Awake()
    {
        base.Awake();

        m_Player = GameObject.FindGameObjectWithTag("Player").transform;
        m_Rigidbody2d = GetComponent<Rigidbody2D>();
    }

    protected override void Update()
    {
        base.Update();

        if (!m_CanAttackPlayer)
            ChacePlayer();
    }


    private void ChacePlayer()
    {
        var dir = Mathf.Sign(transform.position.x - m_Player.position.x);
        m_Rigidbody2d.velocity = new Vector2(m_GroundSpeed * -dir, m_Rigidbody2d.velocity.y);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        //if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        //    Debug.Log("Here!");
    }

}
