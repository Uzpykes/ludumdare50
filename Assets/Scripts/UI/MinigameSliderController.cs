using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameSliderController : MonoBehaviour
{
    [SerializeField] private Slider m_SliderRoot;
    [SerializeField] private PrizeUIController m_PrizePortrait;
    [SerializeField] private RectTransform m_PortraitSpawnObject;
    [SerializeField] private Image m_ResultFeedbackImage;
    [SerializeField] private float m_ResultFadeoutSpeed;
    [SerializeField] private Color m_SuccessColour;
    [SerializeField] private Color m_FailureColour;

    private List<PrizeUIController> m_PortraitInstances;

    private void Start()
    {
        m_PortraitInstances = new List<PrizeUIController>();
        MinigameManager.Instance.OnMinigameEnteredNextStage.AddListener(HandleOnNextStage);
        MinigameManager.Instance.OnMinigameStarted.AddListener(HandleMinigameStart);
        MinigameManager.Instance.OnMinigameEnded.AddListener(HandleMinigameEnd);
        MinigameManager.Instance.OnProgressChanged.AddListener(HandleProgressChange);

        MinigameManager.Instance.OnPrizeSuccess.AddListener(HandlePrizeSuccess);
        MinigameManager.Instance.OnPrizeFail.AddListener(HandlePrizeFailure);
    }

    private void HandleOnNextStage(MinigameStage stage)
    {
        foreach (var instance in m_PortraitInstances)
        {
            Destroy(instance.gameObject);
        }
        m_PortraitInstances.Clear();

        foreach (var prize in stage.Prizes)
        {
            SpawnPortrait(prize);
        }
    }

    private void HandleMinigameStart()
    {
        m_SliderRoot.gameObject.SetActive(true);
    }

    private void HandleMinigameEnd()
    {
        m_SliderRoot.gameObject.SetActive(false);
        foreach (var instance in m_PortraitInstances)
        {
            Destroy(instance.gameObject);
        }
        m_PortraitInstances.Clear();
    }
    
    private void HandleProgressChange(float val)
    {
        m_SliderRoot.value = val;
    }

    private void HandlePrizeSuccess(MinigamePrize prize)
    {
        m_ResultFeedbackImage.color = m_SuccessColour;
    }

    private void HandlePrizeFailure()
    {
        m_ResultFeedbackImage.color = m_FailureColour;
    }

    private void Update()
    {
        var colour = m_ResultFeedbackImage.color;
        colour.a = Mathf.Max(0, m_ResultFeedbackImage.color.a - m_ResultFadeoutSpeed * Time.deltaTime);
        m_ResultFeedbackImage.color = colour;
    }

    private void SpawnPortrait(MinigamePrizeEntry prize)
    {
        var pos = m_PortraitSpawnObject.position + new Vector3(prize.Location * m_PortraitSpawnObject.rect.size.x, 0, 0);
        var instance = Instantiate(m_PrizePortrait, pos, Quaternion.identity, m_PortraitSpawnObject);

        instance.SetPortrait(prize.Prize.PrizeSprite);
        instance.SetWidth(prize.Prize.BaseWidth * m_PortraitSpawnObject.rect.size.x);

        m_PortraitInstances.Add(instance);
    }
}
