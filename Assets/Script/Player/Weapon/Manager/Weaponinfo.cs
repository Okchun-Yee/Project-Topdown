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
    [Header("Prefab")]
    [SerializeField] private GameObject weaponPrefab;
    [Header("Common")]
    [SerializeField] private WeaponCategory category;
    [SerializeField] private float weaponCooldown;
    public int weaponDamge;
    public float weaponRange;

    /// <summary>인스턴스화할 무기 프리팹</summary>
    public GameObject WeaponPrefab => weaponPrefab;

    /// <summary>공격 쿨다운 시간(초)</summary>
    public float CooldownTime => weaponCooldown;

    /// <summary>무기 카테고리 (아이콘 갱신용)</summary>
    public WeaponCategory Category => category;
}
