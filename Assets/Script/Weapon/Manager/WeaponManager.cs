using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : Singleton<WeaponManager>
{
    [Header("Weapon Icons")]
    [SerializeField] private Sprite meleeIcon;
    [SerializeField] private Sprite rangedIcon;
    [SerializeField] private Sprite staffIcon;

    [Header("Weapon")]
    [SerializeField] private Transform weaponMountPoint;
    private BaseWeapon currentWeapon;
    public event Action<Sprite> onCategoryIconChanged;
    protected override void Awake()
    {
        base.Awake();
        //Debug.Log($"[WeaponManager] Awake called (InstanceID: {GetInstanceID()})");
    }
    public void EquipWeapon(WeaponInfo info)
    {
        //Debug.Log($"[WeaponManager] EquipWeapon called (InstanceID: {GetInstanceID()})");
        if (info == null)
        {
            Debug.LogError("[WeaponManager] WeaponInfo is null");
            return;
        }
        // 1) 기존 장착 무기 제거
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
            currentWeapon = null;
            ActiveWeapon.Instance.ClearWeapon();
            // Debug.Log("[WeaponManager] Step 1: Removed existing weapon");
        }
        else
        {
            // Debug.Log("[WeaponManager] Step 1: No existing weapon to remove");
        }

        // 2) 새 무기 인스턴스화 및 마운트
        var go = Instantiate(info.WeaponPrefab, weaponMountPoint.position, Quaternion.identity, weaponMountPoint);
        // Debug.Log($"[WeaponManager] Step 2: Instantiated new weapon prefab '{info.WeaponPrefab.name}'");

        // 3) BaseWeapon 컴포넌트 가져와 초기화
        var bw = go.GetComponent<BaseWeapon>();
        if (bw == null)
        {
            Debug.LogError("[WeaponManager] Step 3: WeaponPrefab missing BaseWeapon component");
            Destroy(go);
            return;
        }
        bw.Initialize(info);
        currentWeapon = bw;
        // Debug.Log($"[WeaponManager] Step 3: Initialized BaseWeapon with cooldown {info.CooldownTime}");

        // 4) ActiveWeapon에 장착 통보
        ActiveWeapon.Instance.NewWeapon(bw);
        // Debug.Log("[WeaponManager] Step 4: Notified ActiveWeapon of new weapon");

        // 5) UI 아이콘 갱신 이벤트 발행
        Sprite icon = info.Category switch
        {
            WeaponCategory.Melee => meleeIcon,
            WeaponCategory.Range => rangedIcon,
            WeaponCategory.Staff => staffIcon,
            _ => null
        };
        onCategoryIconChanged?.Invoke(icon);
        // Debug.Log($"[WeaponManager] Step 5: Fired onCategoryIconChanged with icon for {info.Category}");
    }

    public void UnequipWeapon()
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
            currentWeapon = null;
            ActiveWeapon.Instance.ClearWeapon();
            // Debug.Log("[WeaponManager] UnequipWeapon: Removed and cleared current weapon");
        }
        else
        {
            // Debug.Log("[WeaponManager] UnequipWeapon: No weapon to remove");
        }
        onCategoryIconChanged?.Invoke(null);
        // Debug.Log("[WeaponManager] UnequipWeapon: Cleared UI icon");
    }
}
