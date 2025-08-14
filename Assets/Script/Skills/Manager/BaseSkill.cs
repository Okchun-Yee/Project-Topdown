using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseSkill : MonoBehaviour, ISkill
{
    public static bool IsCasting = false; // 대시 중 상태를 전역으로 관리
    protected bool isOnCooldown = false;
    public SkillInfo SkillInfo { get; private set; }
    // UI 연동용 스킬 인덱스 (예: Skill1 → 0, Skill2 → 1)
    [HideInInspector]
    public int skillIndex;
    public void Initialize(SkillInfo info)
    {
        SkillInfo = info;
        // 추가 초기화 로직이 필요하다면 여기에 작성
    }
    public virtual void ActivateSkill()
    {
        if (isOnCooldown) return; // 쿨타임 중이면 아예 홀딩 시작 안 함
        
        if (SkillInfo.skillCategory == SkillCategory.Charging)
        {
            SubscribeSkillEvents();
            ChargingManager.Instance.StartCharging(SkillInfo.CastingTime);
        }
        else if (SkillInfo.skillCategory == SkillCategory.Holding)
        {
            SubscribeSkillEvents();
            HoldingManager.Instance.StartHolding(SkillInfo.CastingTime);
        }
        else
        {
            OnSkill();
        }
    }
    public void OnSkill()
    {
        if (!isOnCooldown)
        {
            StartCoroutine(ActivateRoutine());
        }
    }

    private IEnumerator ActivateRoutine()
    {
        isOnCooldown = true;
        OnSkillActivated();

        yield return new WaitForSeconds(SkillInfo.CooldownTime);
        isOnCooldown = false;
    }
    public void StartCasting()
    {
        BaseSkill.IsCasting = true; // 스킬 사용 중 상태 설정
    }
    public void EndCasting()
    {
        BaseSkill.IsCasting = false; // 스킬 사용 완료 상태로 변경
    }
    protected abstract void OnSkillActivated();

    //<차징용>
    // 차징 완료 시 호출
    protected virtual void OnChargingCompleted() { }
    // 차징 취소 시 호출
    protected virtual void OnChargingCanceled() { }
    // 차징 중
    protected virtual void OnChargingProgress(float elapsed, float duration) { }

    // <홀딩용>
    // 홀딩 시작 시 호출
    protected virtual void OnHoldingStarted(float maxDuration) { }
    // 홀딩 종료 시 호출
    protected virtual void OnHoldingEnded() { }
    // 홀딩 중
    protected virtual void OnHoldingProgress(float elapsed, float duration) { }

    // 스킬 이벤트 구독
    public virtual void SubscribeSkillEvents()
    {
        // 각 스킬 타입별로 오버라이드에서 구현
    }

    // 스킬 이벤트 해제
    public virtual void UnsubscribeSkillEvents()
    {
        // 각 스킬 타입별로 오버라이드에서 구현
    }

    // OnEnable/OnDisable에서 자동 구독하지 않도록 제거
}
