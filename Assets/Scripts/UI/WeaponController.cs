using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private List<WeaponTypeSprites> m_WeaponSprites;
    [SerializeField] private Image m_WeaponSprite;
    [SerializeField] private Image m_WeaponAmmoSprite;

    private void Start()
    {
        PlayerWeaponManager.Instance.OnActiveWeaponChanged.AddListener(HandleWeaponChange);
    }

    private void HandleWeaponChange(WeaponType newType)
    {
        var sprites = m_WeaponSprites.Find(x => x.Type == newType);
        m_WeaponSprite.sprite = sprites.WeaponIcon;
        m_WeaponAmmoSprite.sprite = sprites.AmmoIcon;
    }
}

[System.Serializable]
public struct WeaponTypeSprites
{
    public WeaponType Type;
    public Sprite WeaponIcon;
    public Sprite AmmoIcon;
}
