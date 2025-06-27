using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 활(Bow) 무기 클래스
/// └ BaseWeapon을 상속받아 공통 쿨다운·초기화 로직 자동 적용
/// </summary>
public class Bow : BaseWeapon
{
    protected override void OnAttack()
    {
        Debug.Log("Bow Attack");
    }
}
