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
    private bool isCharging = false;
    private float chargeTimer = 0f;

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
        currentActiveWeapon = weapon;
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
        // skillIndex에 따라 타이머/플래그 관리
        isCharging = true;
        currentActiveWeapon?.UseSkill(skillIndex);
    }

    private void OnSkillCanceled(int skillIndex)
    {
        isCharging = false;
    }
}
