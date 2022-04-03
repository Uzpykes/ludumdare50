using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Biome : MonoBehaviour
{
    [SerializeField] private Transform m_GroundSpawnPoint; // stays just outside the camera view while the biome is moving
    [SerializeField] private Transform m_AirSpawnPoint;

    public List<FloatingEnemy> m_BiomeFloatingEnemies;
    public List<GroundEnemy> m_BiomeGroundEnemies;

    [NonSerialized] public UnityEvent<Biome> OnBiomeDestroyed;
    [NonSerialized] public UnityEvent<Biome> OnBiomeVisible;

    [SerializeField] private float m_SpawnCooldown = 2f;
    private float m_TimeSinceLastSpawn = 0f;

    [SerializeField] private Collider2D m_Collider;
    public float StartOffset => transform.position.x - m_Collider.bounds.min.x;
    public float EndOffset => m_Collider.bounds.max.x - transform.position.x; // returns where the current biome ends

    private void Awake()
    {
        OnBiomeDestroyed = new UnityEvent<Biome>();
        OnBiomeVisible = new UnityEvent<Biome>();

        OnBiomeVisible.AddListener(EnableSpawnPointsWhenVisible);
        m_AirSpawnPoint.gameObject.SetActive(false);
        m_GroundSpawnPoint.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (IsVisible())
            OnBiomeVisible.Invoke(this);

        m_TimeSinceLastSpawn += Time.deltaTime;

        UpdateSpawnPoints();

        if (m_TimeSinceLastSpawn > m_SpawnCooldown)
        {
            SpawnGroundEnemy();
            SpawnFloatingEnemy();
        }
    }


    private bool IsVisible()
    {
        if (m_Collider.bounds.min.x > VisibilityBounds.Bounds.max.y)
            return false;

        if (m_Collider.bounds.max.x < VisibilityBounds.Bounds.min.y)
            return false;

        return true;
    }

    private void UpdateSpawnPoints()
    {
        if (m_GroundSpawnPoint.gameObject.activeSelf)
        {
            var pos = m_GroundSpawnPoint.position;
            pos.x = VisibilityBounds.Bounds.max.x;
            if (pos.x >= m_Collider.bounds.max.x)
                m_GroundSpawnPoint.gameObject.SetActive(false);
            else
            {
                pos.x += 2f;
                m_GroundSpawnPoint.position = pos;
            }
        }

        if (m_AirSpawnPoint.gameObject.activeSelf)
        {
            var pos = m_AirSpawnPoint.position;
            pos.x = VisibilityBounds.Bounds.max.x;
            if (pos.x >= m_Collider.bounds.max.x)
                m_AirSpawnPoint.gameObject.SetActive(false);
            else
            {
                pos.x += 2f;
                m_AirSpawnPoint.position = pos;
            }
            
        }

    }

    private void SpawnGroundEnemy()
    {
        if (!m_GroundSpawnPoint.gameObject.activeSelf)
            return;

        var enemy = Instantiate(m_BiomeGroundEnemies[0], m_GroundSpawnPoint.position + Vector3.up, Quaternion.identity, this.transform);
            OnBiomeVisible.AddListener(enemy.HandleVisible);

        m_TimeSinceLastSpawn = 0f;
    }

    private void SpawnFloatingEnemy()
    {
        if (!m_AirSpawnPoint.gameObject.activeSelf)
            return;

        var enemy = Instantiate(m_BiomeFloatingEnemies[0], m_AirSpawnPoint.position + (Vector3.up * UnityEngine.Random.Range(-3f, 3f)), Quaternion.identity, this.transform);

        OnBiomeVisible.AddListener(enemy.HandleVisible);
        m_TimeSinceLastSpawn = 0f;

    }

    private void EnableSpawnPointsWhenVisible(Biome biome)
    {
        m_AirSpawnPoint.gameObject.SetActive(true);
        m_GroundSpawnPoint.gameObject.SetActive(true);

        OnBiomeVisible.RemoveListener(EnableSpawnPointsWhenVisible);
    }

    private void OnDestroy()
    {
        OnBiomeDestroyed.Invoke(this);
    }
}
