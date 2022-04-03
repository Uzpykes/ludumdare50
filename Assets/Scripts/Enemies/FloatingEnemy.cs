using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingEnemy : Enemy
{
    [SerializeField] private float m_Floatiness; //How much it wobles up and down
    [SerializeField] private float m_FloatinessRate;
    private float m_InitialHeight;
    private float m_TimeSinceSpawn;

    [SerializeField] private float m_FloatSpeed;

    protected override void Awake()
    {
        m_InitialHeight = transform.position.y;
        base.Awake();
        m_TimeSinceSpawn = Random.Range(0f, m_FloatinessRate);
    }

    protected override void Update()
    {
        m_TimeSinceSpawn += Time.deltaTime;

        Float();
        base.Update();
    }

    //Wobbles height to make it look like balloon is floating
    private void Float()
    {
        var newY = Mathf.Sin(m_TimeSinceSpawn * m_FloatinessRate);

        newY = m_InitialHeight + (newY * m_Floatiness);

        transform.position = new Vector3(transform.position.x - (m_FloatSpeed * Time.deltaTime), newY);
    }
}
