using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrizeUIController : MonoBehaviour
{
    [SerializeField] private Image m_PrizePortrait;
    [SerializeField] private Image m_PrizeSuccessArea;

    public void SetPortrait(Sprite sprite)
    {
        m_PrizePortrait.sprite = sprite;
    }

    public void SetWidth(float width)
    {
        m_PrizeSuccessArea.rectTransform.SetSize(new Vector2(width, m_PrizeSuccessArea.rectTransform.rect.size.y));
    }
}
