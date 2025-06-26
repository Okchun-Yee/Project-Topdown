using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : WeaponBase
{
    [Header("Weapon Type")]
    [SerializeField] private WeaponCategory category;
    public WeaponCategory Category => category;
    protected override void OnAttack()
    {
        Debug.Log("Staff");
        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }
}
