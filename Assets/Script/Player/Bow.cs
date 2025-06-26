using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bow : WeaponBase
{
    [Header("Weapon Type")]
    [SerializeField] private WeaponCategory category;
    public WeaponCategory Category => category;
    protected override void OnAttack()
    {
        Debug.Log("Bow");
        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }
}
