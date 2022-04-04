using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private List<WeaponTypeSprites> m_WeaponSprites;
    [SerializeField] private Image m_WeaponSprite;
    [SerializeField] private Image m_WeaponAmmoSprite;
    [SerializeField] private TextMeshProUGUI m_AmmoCountText;

    private void Start()
    {
        PlayerWeaponManager.Instance.OnActiveWeaponChanged.AddListener(HandleWeaponChange);
        PlayerWeaponManager.Instance.OnAmmoCountChanged.AddListener(HandleAmmoCountChange);

        HandleWeaponChange(PlayerWeaponManager.Instance.ActiveWeapon);
    }

    private void HandleWeaponChange(WeaponType newType)
    {
        var sprites = m_WeaponSprites.Find(x => x.Type == newType);
        m_WeaponSprite.sprite = sprites.WeaponIcon;
        m_WeaponAmmoSprite.sprite = sprites.AmmoIcon;

        m_WeaponSprite.preserveAspect = true;
        m_WeaponAmmoSprite.preserveAspect = true;

        // yuck!
        switch (newType)
        {
            case WeaponType.Grenade:
                HandleAmmoCountChange(newType, PlayerWeaponManager.Instance.GrenadeAmmo);
                break;
            case WeaponType.Handgun:
                HandleAmmoCountChange(newType, PlayerWeaponManager.Instance.HandgunAmmo);
                break;
            case WeaponType.Machinegun:
                HandleAmmoCountChange(newType, PlayerWeaponManager.Instance.MachinegunAmmo);
                break;
            case WeaponType.RocketLauncher:
                HandleAmmoCountChange(newType, PlayerWeaponManager.Instance.RocketLauncherAmmo);
                break;
        }
    }
    
    private void HandleAmmoCountChange(WeaponType changedType, int newAmmoCount)
    {
        if (changedType != PlayerWeaponManager.Instance.ActiveWeapon)
            return;

        m_AmmoCountText.text = $"x{newAmmoCount}";
    }


}

[System.Serializable]
public struct WeaponTypeSprites
{
    public WeaponType Type;
    public Sprite WeaponIcon;
    public Sprite AmmoIcon;
}
