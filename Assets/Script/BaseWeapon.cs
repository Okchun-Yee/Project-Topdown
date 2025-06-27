using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 모든 무기의 공통 기능(초기화·쿨다운 관리)을 담당하는 추상 클래스
/// </summary>
public abstract class BaseWeapon : MonoBehaviour
{
    private float cooldownTime;
    public bool IsOnCooldown { get; private set; }
    private Coroutine cooldownCoroutine;

    /// <summary>
    /// Weaponinfo(SO)로부터 설정을 주입합니다.
    /// </summary>
    public virtual void Initialize(Weaponinfo info)
    {
        if (info == null)
        {
            Debug.LogError($"[BaseWeapon] Weaponinfo is null on {name}");
            return;
        }

        cooldownTime = info.weaponCooldown;
        //추가 사항, 공격피해, 프리펩 등
    }
    /// 외부(WeaponManager 등)에서 호출하는 공격 진입점
    public void Attack()
    {
        if (IsOnCooldown) return;
        OnAttack(); //개별 무기의 공격 로직
        cooldownCoroutine = StartCoroutine(CooldownRoutine());
    }
    protected abstract void OnAttack();

    private IEnumerator CooldownRoutine()
    {
        IsOnCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        IsOnCooldown = false;
    }
    // ——— 안전 장치: 코루틴 정리 ———
    protected virtual void OnDestroy()
    {
        if (cooldownCoroutine != null)
            StopCoroutine(CooldownRoutine());
    }
}
