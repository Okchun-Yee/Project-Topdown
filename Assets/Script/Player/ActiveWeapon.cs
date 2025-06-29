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
        playerContorls.Enable();
        playerContorls.Combat.Attack.started +=  OnAttackStarted;
        playerContorls.Combat.Attack.canceled += OnAttackCanceled;
    }

    private void OnDisable()
    {
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
