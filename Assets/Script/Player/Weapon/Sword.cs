using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 검 무기 클래스
/// └ BaseWeapon을 상속받아 공통 쿨다운·초기화 로직 적용
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider2D))]
public class Sword : BaseWeapon
{
    protected override void OnAttack()
    {
        Debug.Log("Sword Attack");
    }
}
