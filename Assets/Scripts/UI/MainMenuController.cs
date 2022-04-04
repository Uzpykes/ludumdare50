using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    [SerializeField] private Button m_StartGame;
    [SerializeField] private Button m_HowToPlay;
    [SerializeField] private Button m_Quit;

    [SerializeField] private RectTransform m_HowToWindow;

    [SerializeField] private int m_MainGameSceneId;

    private void Start()
    {
        m_StartGame.onClick.AddListener(DoStartGame);
        m_HowToPlay.onClick.AddListener(DoHowToPlay);
        m_Quit.onClick.AddListener(DoQuit);
    }


    private void DoQuit()
    {
        Application.Quit();
    }

    private void DoHowToPlay()
    {
        m_HowToWindow.gameObject.SetActive(true);
    }

    private void DoStartGame()
    {
        SceneManager.LoadScene(m_MainGameSceneId);
    }
}
