using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : Singleton<WeaponManager>
{
    /// <summary>
    /// 무기 분류가 변경될 때 UI 아이콘(Sprite)을 전달
    /// </summary>
    public event Action<Sprite> onCategoryIconChanged;

    [Header("Weapon Icon")]
    [SerializeField] private Sprite meleeIcon;
    [SerializeField] private Sprite rangedIcon;
    [SerializeField] private Sprite staffIcon;

    [Header("Weapon Mount")]
    [SerializeField] private Transform weaponMountPoint;  // 무기 인스턴스가 붙을 위치
    private BaseWeapon currentWeapon;

    /// <summary>
    /// Weaponinfo를 받아 기존 무기를 해제하고 새 무기를 장착합니다.
    /// </summary>
    public void EquipWeapon(Weaponinfo info)
    {
        if (info == null)
        {
            Debug.LogError("[WeaponManager] Weaponinfo is null");
            return;
        }

        // 1) 기존 무기 제거
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
            currentWeapon = null;
        }

        // 2) 새 무기 생성 및 초기화
        GameObject gameObject = Instantiate(info.weaponPrefab, weaponMountPoint);
        var weapon = gameObject.GetComponent<BaseWeapon>();
        if (weapon == null)
        {
            Debug.LogError("[WeaponManager] WeaponPrefab에 BaseWeapon 컴포넌트가 없습니다.");
            Destroy(gameObject);
            return;
        }
        weapon.Initialize(info);
        currentWeapon = weapon;

        WeaponCategoryChange(info.category);
    }

    public void UnequipWeapon()
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
            currentWeapon = null;
        }
        onCategoryIconChanged.Invoke(null);
    }

    public void WeaponCategoryChange(WeaponCategory category)
    {
        Sprite icon = category switch
        {
            WeaponCategory.Melee => meleeIcon,
            WeaponCategory.Range => rangedIcon,
            WeaponCategory.Staff => staffIcon,
            _ => null
        };
        onCategoryIconChanged?.Invoke(icon);
    }
}
