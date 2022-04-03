using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FuelBarController : MonoBehaviour
{
    [SerializeField] private Image m_Image;
    [SerializeField] private Image m_FuelBorder;
    private float m_BaseValue;
    private float m_BaseBorderWidth;
    private float m_BaseFillWidth;

    private void Start()
    {
        m_BaseValue = PlayerStatsManager.Instance.MaxFuel; // get starting value;
        m_BaseBorderWidth = m_FuelBorder.rectTransform.rect.width;
        m_BaseFillWidth = m_Image.rectTransform.rect.width;
        PlayerStatsManager.Instance.OnFuelChanged.AddListener(HandleChange);
        PlayerStatsManager.Instance.OnMaxFuelChanged.AddListener(HandleMaxFuelChange);
    }

    private void HandleChange(float val)
    {
        m_Image.fillAmount = val / PlayerStatsManager.Instance.MaxFuel;
    }

    private void HandleMaxFuelChange(float val)
    {
        Debug.Log($"I'm here. New val: {val}");
        var rectX = (val / m_BaseValue);
        m_FuelBorder.rectTransform.SetSize(new Vector2(rectX * m_BaseBorderWidth, m_FuelBorder.rectTransform.rect.size.y));

        m_Image.rectTransform.SetSize(new Vector2(rectX * m_BaseFillWidth, m_Image.rectTransform.rect.size.y));
        m_Image.fillAmount = PlayerStatsManager.Instance.Fuel / PlayerStatsManager.Instance.MaxFuel;

    }

}
