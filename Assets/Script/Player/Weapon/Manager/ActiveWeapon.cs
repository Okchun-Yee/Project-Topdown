using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    private IWeapon currentActiveWeapon;
    private PlayerContorls playerContorls;
    private bool attackButtonDown = false;

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
        playerContorls.Combat.Attack.started += OnAttackStarted;
        playerContorls.Combat.Attack.canceled += OnAttackCanceled;

        //skill 이벤트 바인딩
        playerContorls.Combat.Skill1.performed += ctx => currentActiveWeapon?.UseSkill(0);
        playerContorls.Combat.Skill2.performed += ctx => currentActiveWeapon?.UseSkill(1);
        playerContorls.Combat.Skill3.performed += ctx => currentActiveWeapon?.UseSkill(2);
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
        
    }
    /// <summary>
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
}
