using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Manages what gets spawned when
public class BiomeManager : MonoBehaviour
{
    [SerializeField] List<Biome> m_BiomePrefabs;
    [SerializeField] Transform m_EnemyHolder;

    private void Start()
    {
        var firstBiome = Rand(m_BiomePrefabs);

        var instance = Instantiate(firstBiome, Vector3.zero, Quaternion.identity, this.transform);
        instance.OnBiomeVisible.AddListener(SpawnBiome);

        GameOverManager.Instance.OnGameOver.AddListener(() =>
        {
            this.gameObject.SetActive(false);
        });
    }

    private void SpawnBiome(Biome previous)
    {
        var biome = Rand(m_BiomePrefabs);

        var instance = Instantiate(biome, Vector3.zero, Quaternion.identity, this.transform);
        instance.transform.position = new Vector3(previous.EndOffset + instance.StartOffset + previous.transform.position.x -1f, previous.transform.position.y);
        previous.OnBiomeVisible.RemoveListener(SpawnBiome);
        instance.OnBiomeVisible.AddListener(SpawnBiome);
    }


    private void OnDestroy()
    {
    }

    private T Rand<T>(List<T> l)
    {
        return l[Random.Range(0, l.Count)];
    }
}
