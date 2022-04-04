using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Biome : MonoBehaviour
{
    [SerializeField] private Transform m_GroundSpawnPoint; // stays just outside the camera view while the biome is moving
    [SerializeField] private Transform m_AirSpawnPoint;
    [SerializeField] private Transform m_PointOfInterest;

    public List<FloatingEnemy> m_BiomeFloatingEnemies;
    public List<GroundEnemy> m_BiomeGroundEnemies;

    [NonSerialized] public UnityEvent<Biome> OnBiomeDestroyed;
    [NonSerialized] public UnityEvent<Biome> OnBiomeVisible;

    [SerializeField] private float m_GroundSpawnCooldown = 2f;
    [SerializeField] private float m_AirSpawnCooldown = 5f;

    private float m_AdjustedGroundSpawnCooldown;
    private float m_AdjustedAirSpawnCooldown;
    private float m_TimeSinceLastGroundSpawn = 0f;
    private float m_TimeSinceLastAirSpawn = 0f;

    [SerializeField] private List<MinigameTrigger> m_MinigamePrefabs;
    private bool m_MinigameSpawned;

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

    private void Start()
    {
        GameOverManager.Instance.OnGameOver.AddListener(() =>
        {
            this.gameObject.SetActive(false);
        });
    }

    private void Update()
    {
        if (IsVisible())
            OnBiomeVisible.Invoke(this);

        m_TimeSinceLastGroundSpawn += Time.deltaTime;
        m_TimeSinceLastAirSpawn += Time.deltaTime;

        UpdateSpawnPoints();
        CalculateAdjustedSpawnCooldown();

        if (m_TimeSinceLastGroundSpawn > m_AdjustedGroundSpawnCooldown)
        {
            SpawnGroundEnemy();
        }

        if (m_TimeSinceLastAirSpawn > m_AdjustedAirSpawnCooldown)
        {
            SpawnFloatingEnemy();
        }

        if (m_MinigameSpawned == false)
        {
            m_MinigameSpawned = true;
            SpawnMinigame();
        }
    }

    private void CalculateAdjustedSpawnCooldown()
    {
        m_AdjustedGroundSpawnCooldown = Mathf.Max(1f, m_GroundSpawnCooldown - PlayerStatsManager.Instance.GlobalDifficultyMultiplier);

        m_AdjustedAirSpawnCooldown = Mathf.Max(3f, m_AirSpawnCooldown - PlayerStatsManager.Instance.GlobalDifficultyMultiplier);
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

        var enemy = Instantiate(m_BiomeGroundEnemies[UnityEngine.Random.Range(0, m_BiomeGroundEnemies.Count)], m_GroundSpawnPoint.position + Vector3.up, Quaternion.identity, this.transform.parent);
            OnBiomeVisible.AddListener(enemy.HandleVisible);

        m_TimeSinceLastGroundSpawn = UnityEngine.Random.Range(-1f, 1f); //hacky way to randomize spawn time
    }

    private void SpawnFloatingEnemy()
    {
        if (!m_AirSpawnPoint.gameObject.activeSelf)
            return;

        var enemy = Instantiate(m_BiomeFloatingEnemies[UnityEngine.Random.Range(0, m_BiomeFloatingEnemies.Count)], m_AirSpawnPoint.position + (Vector3.up * UnityEngine.Random.Range(-3f, 3f)), Quaternion.identity, this.transform.parent);

        OnBiomeVisible.AddListener(enemy.HandleVisible);
        m_TimeSinceLastAirSpawn = UnityEngine.Random.Range(-1f, 1f); //hacky way to randomize spawn time
    }

    private void SpawnMinigame()
    {
        var randomMinigame = m_MinigamePrefabs[UnityEngine.Random.Range(0, m_MinigamePrefabs.Count)];

        Instantiate(randomMinigame, m_PointOfInterest.position, Quaternion.identity, this.transform.parent);
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
