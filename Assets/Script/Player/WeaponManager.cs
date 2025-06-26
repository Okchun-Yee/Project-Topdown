using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public event Action<Sprite> onCategoryIconChanged;

    [Header("Weapon Icon")]
    [SerializeField] private Sprite meleeIcon;
    [SerializeField] private Sprite rangedIcon;
    [SerializeField] private Sprite staffIcon;
    private Transform weaponMountPoint;
    private GameObject currentWeaponObj;
    private void Awake()
    {
        var pc = GetComponentInParent<PlayerController>();
        if (pc == null)
        {
            Debug.LogError("WeaponManager: 부모에 PlayerController가 없습니다!");
            return;
        }
        weaponMountPoint = pc.GetWeaponCollider();
        if (weaponMountPoint == null)
            Debug.LogError("PlayerController.WeaponMountPoint가 할당되지 않았습니다!");
    }

    public void EquipWeapon(Weaponinfo info)
    {
        if (currentWeaponObj != null)
        {
            Destroy(currentWeaponObj);
        }
        // 프리팹 Instantiate 해서 마운트 포인트에 붙이기
        currentWeaponObj = Instantiate(info.weaponPrefab, weaponMountPoint.position, weaponMountPoint.rotation,
            weaponMountPoint);

        //UI 갱신
        WeaponCategoryChange(info.category);
        // Sword 컴포넌트 가져오기
        var sword = currentWeaponObj.GetComponent<Sword>();

        // ① 플레이어 콜라이더 참조
        var collider = PlayerController.Instance.GetWeaponCollider();
        // ② 이펙트 스폰 포인트 참조
        var spawnPoint = ActiveWeapon.Instance.transform.Find("EffectSpawnPoint");
        // ③ Animator 컴포넌트 참조
        var animator = currentWeaponObj.GetComponentInChildren<Animator>();
        // 수동으로 주입
        sword.Initialize(collider, spawnPoint, animator);

        // ActiveWeapon 싱글톤에 새 무기 컴포넌트 알리기
        var w = currentWeaponObj.GetComponent<WeaponBase>() as MonoBehaviour;
        ActiveWeapon.Instance.NewWeapon(w);
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
