using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : MonoBehaviour, IWeapon
{
    [Header("Weapon Type")]
    [SerializeField] private WeaponCategory category;
    public WeaponCategory Category => category;
    public void Attack()
    {
        Debug.Log("Staff");
        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }
}
