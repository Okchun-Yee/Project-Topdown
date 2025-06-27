using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponCategory
{
    Melee,
    Range,
    Staff
}

[CreateAssetMenu(menuName = "New Weapon")]
public class Weaponinfo : ScriptableObject
{
    public GameObject weaponPrefab;
    public WeaponCategory category;
    public float weaponCooldown;
}
