using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUIController : MonoBehaviour
{
    [SerializeField] private RectTransform m_GameOverScreen;
    [SerializeField] private TextMeshProUGUI m_DistanceField;
    [SerializeField] private TextMeshProUGUI m_KillsField;

    [SerializeField] private Button m_RestartButton;
    [SerializeField] private Button m_QuitButton;
    [SerializeField] private Button m_MainMenuButton;

    private void Start()
    {
        GameOverManager.Instance.OnGameOver.AddListener(HandleGameOver);
        m_RestartButton.onClick.AddListener(Restart);
        m_QuitButton.onClick.AddListener(Quit);
        m_MainMenuButton.onClick.AddListener(MainMenu);
    }

    void HandleGameOver()
    {
        m_GameOverScreen.gameObject.SetActive(true);
        m_DistanceField.text = $"{Mathf.RoundToInt(PlayerStatsManager.Instance.DistanceTraveled)} m";
        m_KillsField.text = $"{PlayerStatsManager.Instance.ZombiesKilled} x";
    }

    void Restart()
    {
        SceneManager.LoadScene(1);
    }

    void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    
    void Quit()
    {
        Application.Quit();
    }
}
