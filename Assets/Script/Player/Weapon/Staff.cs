using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스태프(Staff) 무기 클래스
/// └ BaseWeapon을 상속받아 공통 쿨다운·초기화 로직 자동 적용
/// </summary>
public class Staff : BaseWeapon
{
    // 공통 Attack() 호출 시 실행되는 실제 공격 로직
    protected override void OnAttack()
    {
        Debug.Log("Staff Attack");
    }
}
