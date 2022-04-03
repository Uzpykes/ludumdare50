using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//There will always be only one minigame
public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance { get; private set; }
    private MinigameTrigger m_Trigger;
    private List<MinigameStage> m_MinigameStages;
    private int m_CurrentStage;
    private float m_MinigameProgress;
    private bool m_IsInProgress;
    [SerializeField] private float m_StageStartCooldown;
    private float m_TimeSinceStageStart;

    [SerializeField] private float m_TimeToPauseAfterInput;
    private float m_PauseFor;

    private bool m_ShouldChangeStage = false;

    [NonSerialized] public UnityEvent OnMinigameStarted;
    [NonSerialized] public UnityEvent<float> OnProgressChanged;
    [NonSerialized] public UnityEvent<MinigameStage> OnMinigameEnteredNextStage;
    [NonSerialized] public UnityEvent OnMinigameEnded;

    [NonSerialized] public UnityEvent<MinigamePrize> OnPrizeSuccess;
    [NonSerialized] public UnityEvent OnPrizeFail;

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
            m_CurrentStage = -1;
        }
    }

    private void CreateEvents()
    {
        OnMinigameStarted = new UnityEvent();
        OnProgressChanged = new UnityEvent<float>();
        OnMinigameEnteredNextStage = new UnityEvent<MinigameStage>();
        OnMinigameEnded = new UnityEvent();

        OnPrizeSuccess = new UnityEvent<MinigamePrize>();
        OnPrizeFail = new UnityEvent();
    }

    public void StartMinigame(List<MinigameStage> stages, MinigameTrigger trigger)
    {
        if (m_IsInProgress)
            return;

        m_IsInProgress = true;
        m_Trigger = trigger;
        m_MinigameStages = stages;
        m_TimeSinceStageStart = 0f;
        OnMinigameStarted.Invoke();
        NextStage();
    }

    public void EndMinigame(MinigameTrigger trigger)
    {
        //Only end if caller is the same that started the minigame
        if (m_Trigger != trigger)
            return;

        m_IsInProgress = false;
        m_MinigameStages = null;
        m_TimeSinceStageStart = 0f;
        m_CurrentStage = -1;
        OnMinigameEnded.Invoke();
    }

    private void NextStage()
    {
        m_CurrentStage++;
        m_TimeSinceStageStart = 0f;
        m_MinigameProgress = 0f;
        OnProgressChanged.Invoke(m_MinigameProgress);
        if (m_CurrentStage >= m_MinigameStages.Count) //if next stage does not exist then end minigame
            EndMinigame(m_Trigger);
        else
            OnMinigameEnteredNextStage.Invoke(m_MinigameStages[m_CurrentStage]);
    }


    private void Update()
    {
        if (m_PauseFor > 0f)
            m_PauseFor -= Time.deltaTime;

        if (m_PauseFor <= 0f)
        {
            UpdateMinigameStage();
            HandleInput();
        }
    }

    void UpdateMinigameStage()
    {
        if (m_IsInProgress == false)
            return;

        if (m_CurrentStage < 0)
            return;

        if (m_ShouldChangeStage)
        {
            m_ShouldChangeStage = false;
            NextStage();
        }

        m_TimeSinceStageStart += Time.deltaTime;
        if (m_TimeSinceStageStart < m_StageStartCooldown)
            return;

        var stage = m_MinigameStages[m_CurrentStage];

        m_MinigameProgress += Time.deltaTime / stage.TimeToEnd;
        OnProgressChanged.Invoke(m_MinigameProgress);

        if (m_MinigameProgress >= 1f)
            NextStage();
    }

    private void HandleInput()
    {
        if (!m_IsInProgress)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var prize = EvaluatePrize();
            if (prize == null)
            {
                OnPrizeFail.Invoke();
            }
            else
            {
                OnPrizeSuccess.Invoke(prize);
            }

            m_PauseFor = m_TimeToPauseAfterInput;
            m_ShouldChangeStage = true;
        }
    }

    //Return prize witch is at current progress
    private MinigamePrize EvaluatePrize()
    {
        foreach(var prize in m_MinigameStages[m_CurrentStage].Prizes)
        {
            if (m_MinigameProgress > prize.Location - (prize.Prize.BaseWidth / 2f) &&
                m_MinigameProgress < prize.Location + (prize.Prize.BaseWidth / 2f))
            {
                return prize.Prize;
            }
        }
        return null;
    }
}

[System.Serializable]
public struct MinigameStage
{
    public float TimeToEnd;
    [SerializeField] public List<MinigamePrizeEntry> Prizes;
}
