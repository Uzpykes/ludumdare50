using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Biome : MonoBehaviour
{
    public List<Transform> m_SpawnPoints;
    public List<Transform> m_FloatingEnemySpawnPoints;

    public List<FloatingEnemy> m_BiomeFloatingEnemies;
    public List<GroundEnemy> m_BiomeGroundEnemies;

    [NonSerialized] public UnityEvent<Biome> OnBiomeDestroyed;
    [NonSerialized] public UnityEvent<Biome> OnBiomeVisible;

    [SerializeField] private Collider2D m_Collider;
    public float StartOffset => transform.position.x - m_Collider.bounds.min.x;
    public float EndOffset => m_Collider.bounds.max.x - transform.position.x; // returns where the current biome ends

    private void Awake()
    {
        OnBiomeDestroyed = new UnityEvent<Biome>();
        OnBiomeVisible = new UnityEvent<Biome>();
    }

    private void Start()
    {
        SpawnGroundEnemies();
        SpawnFloatingEnemies();
    }

    private void Update()
    {
        if (IsVisible())
            OnBiomeVisible.Invoke(this);
    }


    private bool IsVisible()
    {
        if (m_Collider.bounds.min.x > VisibilityBounds.Bounds.max.y)
            return false;

        if (m_Collider.bounds.max.x < VisibilityBounds.Bounds.min.y)
            return false;

        return true;
    }

    private void SpawnGroundEnemies()
    {
        foreach (var pos in m_SpawnPoints)
        {
            var enemy = Instantiate(m_BiomeGroundEnemies[0], pos.position + Vector3.up, Quaternion.identity, this.transform);
            OnBiomeVisible.AddListener(enemy.HandleVisible);
        }
    }

    private void SpawnFloatingEnemies()
    {
        foreach (var pos in m_FloatingEnemySpawnPoints)
        {
            var enemy = Instantiate(m_BiomeFloatingEnemies[0], pos.position + Vector3.up, Quaternion.identity, this.transform);

            OnBiomeVisible.AddListener(enemy.HandleVisible);
        }
    }

    private void OnDestroy()
    {
        OnBiomeDestroyed.Invoke(this);
    }
}
