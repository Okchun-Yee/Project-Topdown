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

    [Header("Scene References")]
    [SerializeField] private Transform weaponMountPoint;
    [SerializeField] private Transform effectSpawnPoint;
    [SerializeField] private GameObject  weaponCollider;
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
            ActiveWeapon.Instance.WeaponNull();
        }
         // 1) 프리팹 생성
        var go     = Instantiate(info.weaponPrefab, weaponMountPoint.position, Quaternion.identity, weaponMountPoint);
        var weapon = go.GetComponent<BaseWeapon>();
        // 2) Scene 참조 주입
        weapon.InjectSceneReferences(effectSpawnPoint, weaponCollider);
        // 3) SO 초기화
        weapon.Initialize(info);
        currentWeapon = weapon;
        // 4) ActiveWeapon에 IWeapon으로 등록
        ActiveWeapon.Instance.NewWeapon(weapon);

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
