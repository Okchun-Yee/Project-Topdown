using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    private IWeapon currentActiveWeapon;
    private PlayerContorls playerContorls;
    private bool attackButtonDown = false;
    private int? currentActiveSkillIndex = null;

    // 현재 무기에 접근할 수 있는 public 프로퍼티
    public IWeapon CurrentWeapon => currentActiveWeapon;

    protected override void Awake()
    {
        base.Awake();
        playerContorls = new PlayerContorls();
    }

    private void OnEnable()
    {
        // 중복된 인스턴스는 초기화를 건너뜁니다. -> Sceen 이동시 ActiveWeapon이 여러개 생기는 것을 방지
        // Singleton 패턴을 사용하고 있으므로, Instance가 null이 아니면 초기화를 건너뜁니다.
        if (this != Instance) return;

        playerContorls.Enable();
        //마우스 클릭 이벤트 구독 (기본 공격)
        playerContorls.Combat.Attack.started += OnAttackStarted;
        playerContorls.Combat.Attack.canceled += OnAttackCanceled;

        //스킬 사용 이벤트 구독 Skill1
        playerContorls.Combat.Skill1.started += ctx => OnSkillStarted(0);
        playerContorls.Combat.Skill1.canceled += ctx => OnSkillCanceled(0);

        //스킬 사용 이벤트 구독 Skill2
        playerContorls.Combat.Skill2.started += ctx => OnSkillStarted(1);
        playerContorls.Combat.Skill2.canceled += ctx => OnSkillCanceled(1);

        //스킬 사용 이벤트 구독 Skill3
        playerContorls.Combat.Skill3.started += ctx => OnSkillStarted(2);
        playerContorls.Combat.Skill3.canceled += ctx => OnSkillCanceled(2);
    }

    private void OnDisable()
    {
        // 중복된 인스턴스는 초기화를 건너뜁니다. -> Sceen 이동시 ActiveWeapon이 여러개 생기는 것을 방지
        // Singleton 패턴을 사용하고 있으므로, Instance가 null이 아니면 초기화를 건너뜁니다.
        if (this != Instance) return;

        playerContorls.Combat.Attack.started -= OnAttackStarted;
        playerContorls.Combat.Attack.canceled -= OnAttackCanceled;
        playerContorls.Disable();
    }
    private void Update()
    {
        if (attackButtonDown && currentActiveWeapon != null)
        {
            currentActiveWeapon.Attack();  // → null일 일도, 잘못된 캐스트도 없음
            attackButtonDown = false;
        }

    }/// <summary>
    /// WeaponManager에서 새 무기를 생성한 직후 호출해주세요.
    /// </summary>
    public void NewWeapon(IWeapon weapon)
    {
        // 이전 무기의 모든 스킬 구독 해제
        if (currentActiveWeapon is BaseWeapon oldWeapon && currentActiveSkillIndex.HasValue)
        {
            UnsubscribeSkill(currentActiveSkillIndex.Value);
        }

        currentActiveWeapon = weapon;
        currentActiveSkillIndex = null;
        attackButtonDown = false;
    }
    /// <summary>
    /// 무기를 해제할 때 호출
    /// </summary>
    public void ClearWeapon()
    {
        currentActiveWeapon = null;
        attackButtonDown = false;
    }
    private void OnAttackStarted(InputAction.CallbackContext ctx)
    {
        attackButtonDown = true;
    }

    private void OnAttackCanceled(InputAction.CallbackContext ctx)
    {
        attackButtonDown = false;
    }

    private void OnSkillStarted(int skillIndex)
    {
        if (currentActiveWeapon == null) return;

        // 이전 활성 스킬 구독 해제
        if (currentActiveSkillIndex.HasValue)
        {
            UnsubscribeSkill(currentActiveSkillIndex.Value);
        }

        // 새 스킬 구독
        SubscribeSkill(skillIndex);
        currentActiveSkillIndex = skillIndex;

        // 스킬 사용
        currentActiveWeapon.UseSkill(skillIndex);
    }

    private void OnSkillCanceled(int skillIndex)
    {
        if (currentActiveWeapon == null) return;

        // 해당 스킬 구독 해제
        UnsubscribeSkill(skillIndex);
        
        if (currentActiveSkillIndex == skillIndex)
        {
            currentActiveSkillIndex = null;
        }

        // 차징/홀딩 종료
        ChargingManager.Instance?.EndCharging();
        HoldingManager.Instance?.EndHolding();
    }

    private void SubscribeSkill(int skillIndex)
    {
        var skills = (currentActiveWeapon as BaseWeapon)?.GetSkills();
        skills?[skillIndex]?.SubscribeSkillEvents();
    }

    private void UnsubscribeSkill(int skillIndex)
    {
        var skills = (currentActiveWeapon as BaseWeapon)?.GetSkills();
        skills?[skillIndex]?.UnsubscribeSkillEvents();
    }
}
