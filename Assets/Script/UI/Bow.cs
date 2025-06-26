using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bow : MonoBehaviour, IWeapon
{
    [Header("Weapon Type")]
    [SerializeField] private WeaponCategory category;
    public WeaponCategory Category => category;
    public void Attack()
    {
        Debug.Log("Bow");
        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }
}
