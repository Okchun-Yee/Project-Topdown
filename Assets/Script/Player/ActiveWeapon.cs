using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>
{
    public MonoBehaviour currentActiveWeapon { get; private set; }
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
    }
    private void Start()
    {
        playerContorls.Combat.Attack.started += _ => StartAttacking();
        playerContorls.Combat.Attack.canceled += _ => StopAttacking();
    }
    private void Update()
    {
        Attack();
    }
    public void NewWeapon(MonoBehaviour newWeapon)
    {
        currentActiveWeapon = newWeapon;
    }
    public void WeaponNull()
    {
        currentActiveWeapon = null;
    }
    public void ToggleIsAttacking(bool value)
    {
        isAttacking = value;
    }
    private void StartAttacking()
    {
        attackButtonDown = true;
    }
    private void StopAttacking()
    {
        attackButtonDown = false;
    }
    private void Attack()
    {
        if (attackButtonDown && !isAttacking)
        {
            isAttacking = true;
            (currentActiveWeapon as IWeapon).Attack();
        }
    }
}
