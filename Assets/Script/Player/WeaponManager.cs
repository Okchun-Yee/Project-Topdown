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

    //시작 시 현재 무기 종류가 무엇인지 파악하는 설정 << 이후에 제거 가능
    [SerializeField] private WeaponCategory startingCategory;
    public WeaponCategory StartingCategory => startingCategory;
    
    private void Start()
    {
        // 구독된 UI가 있으면 초기에 한 번 호출
        WeaponCategoryChange(startingCategory);
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
