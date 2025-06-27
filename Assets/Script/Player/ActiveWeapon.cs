using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    private IWeapon currentActiveWeapon;
    private PlayerContorls playerContorls;
    private bool attackButtonDown, isAttacking = false;

    protected override void Awake()
    {
        base.Awake();
        playerContorls = new PlayerContorls();
    }

    private void OnEnable()
    {
        playerContorls.Enable();
        playerContorls.Combat.Attack.started  += _ => attackButtonDown = true;
        playerContorls.Combat.Attack.canceled += _ => attackButtonDown = false;
    }

    private void OnDisable()
    {
        playerContorls.Combat.Attack.started  -= _ => attackButtonDown = true;
        playerContorls.Combat.Attack.canceled -= _ => attackButtonDown = false;
        playerContorls.Disable();
    }
    private void Update()
    {
        if (!attackButtonDown || isAttacking || currentActiveWeapon == null)
            return;
        isAttacking = true;
        currentActiveWeapon.Attack();  // → null일 일도, 잘못된 캐스트도 없음
    }
    /// <summary>WeaponManager에서 호출</summary>
    public void NewWeapon(IWeapon weapon)
    {
        currentActiveWeapon = weapon;
        isAttacking = false;
        attackButtonDown = false;
    }

    public void WeaponNull()
    {
        currentActiveWeapon = null;
        isAttacking = false;
        attackButtonDown = false;
    }
    public void ToggleIsAttacking(bool value)
    {
        isAttacking = value;
    }
}
