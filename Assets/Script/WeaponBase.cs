using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
//abstract : 추상 클래스
public abstract class WeaponBase : MonoBehaviour, IWeapon
{
    [Header("Data")]
    [SerializeField] protected Weaponinfo info;
    public float Cooldown => info.weaponCooldown;
    public float lastAttackTime;

    public void Attack()
    {
        if (Time.time < lastAttackTime + Cooldown) return;
        lastAttackTime = Time.time;
        OnAttack();  // 실제 공격은 서브클래스에 위임
    }
    protected abstract void OnAttack();
}
