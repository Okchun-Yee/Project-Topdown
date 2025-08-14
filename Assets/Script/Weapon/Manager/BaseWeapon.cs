using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 모든 무기의 공통 기능(초기화·쿨다운 관리)을 담당하는 추상 클래스
/// </summary>
public abstract class BaseWeapon : MonoBehaviour, IWeapon
{
    public static bool IsAttacking = false; // 공격 중 상태를 전역으로 관리
    private Coroutine CooldownCoroutine; //무기 공격 쿨다운 코루틴
    protected ISkill[] skills; // 무기에 적용된 스킬들

    /// <summary>
    /// 무기 및 스킬 사용 변수
    /// </summary>
    private float weaponCooldown;    // SO에서 주입받는 쿨다운 시간
    private bool isCooldown;     //무기 쿨타임 검사
    private float[] skillCastingTime; // 스킬 시전 시간

    /// <summary>
    /// ScriptableObject(Weaponinfo)로부터 설정을 주입합니다.
    /// </summary>
    public virtual void Initialize(Weaponinfo info)
    {
        if (info == null)
        {
            Debug.LogError($"[BaseWeapon] Weaponinfo is null on {name}");
            return;
        }

        weaponCooldown = info.CooldownTime;
        skills = GetComponents<ISkill>();
        // 배열 크기 초기화 추가
        skillCastingTime = new float[skills.Length];

        for (int i = 0; i < skills.Length; i++)
        {
            if (skills.Length != info.Skills.Length)
            {
                Debug.LogWarning($"[BaseWeapon] SkillInfo length ({info.Skills.Length}) != ISkill components ({skills.Length}) on {name}");
            }
            skills[i].Initialize(info.Skills[i]);
            skillCastingTime[i] = info.Skills[i].CastingTime;
        }
        SkillUIManager.Instance.Initialized(info.Skills); // 스킬 UI 초기화
        //추가 사항, 공격피해, 프리펩 등
        var ds = GetComponentInChildren<DamageSource>();
        if (ds != null)
            ds.DamageAmount = info.weaponDamge;
    }

    /// <summary>
    /// 외부에서 호출하는 공격 진입점.
    /// 쿨다운 중이면 무시하고, 아니면 OnAttack() 및 쿨다운 시작.
    /// </summary>
    public void Attack()
    {
        if (isCooldown) { return; }
        OnAttack();
        CooldownCoroutine = StartCoroutine(CooldownRoutine());
    }
    // <summary>
    /// 구체 무기에서 실제 공격 로직을 구현할 메서드.
    /// </summary>
    protected abstract void OnAttack();

    private IEnumerator CooldownRoutine()
    {
        isCooldown = true;
        yield return new WaitForSeconds(weaponCooldown);
        isCooldown = false;
        // 쿨다운 끝나면 ActiveWeapon에 알려주기
    }
    /// <summary>
    /// 객체가 비활성화될 때 코루틴을 정리해 안전하게 멈춥니다.
    /// </summary>
    protected virtual void OnDisable()
    {
        // 쿨다운 코루틴 정리만 남김
        if (CooldownCoroutine != null)
            StopCoroutine(CooldownCoroutine);
    }
    public void UseSkill(int index)
    {
        if (index < 0 || index >= skills.Length) return;

        // 스킬에게 책임 위임
        skills[index]?.ActivateSkill();
    }

    public ISkill[] GetSkills()
    {
        return skills;
    }
}
