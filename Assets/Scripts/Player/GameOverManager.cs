using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameOverManager : MonoBehaviour
{
    public static GameOverManager Instance { get; private set; }
    [NonSerialized] public UnityEvent OnGameOver;

    private bool m_IsGameOver;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            CreateEvents();
        }
    }

    private void CreateEvents()
    {
        OnGameOver = new UnityEvent();
    }

    private void Start()
    {
        PlayerStatsManager.Instance.OnHealthChanged.AddListener(HandleHealth);
    }

    private void HandleHealth(float newVal)
    {
        if (m_IsGameOver)
            return;

        if (newVal <= 0f)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        OnGameOver.Invoke();
    }

}
