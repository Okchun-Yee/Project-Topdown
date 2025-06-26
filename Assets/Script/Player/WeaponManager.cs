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
