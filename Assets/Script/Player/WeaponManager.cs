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

    [Header("Weapon Instance")]
    [SerializeField] private Transform weaponMountPoint;
    private GameObject currentWeaponObj;

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
        
        // ActiveWeapon 싱글톤에 새 무기 컴포넌트 알리기
        var w = currentWeaponObj.GetComponent<MonoBehaviour>();
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
