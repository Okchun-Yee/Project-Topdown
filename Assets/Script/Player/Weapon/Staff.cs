using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스태프(Staff) 무기 클래스
/// └ BaseWeapon을 상속받아 공통 쿨다운·초기화 로직 자동 적용
/// </summary>
public class Staff : BaseWeapon
{
    private Transform weaponCollider;
    private Animator anim;
    private void Update()
    {
        MouseFollowWithOffset();
    }
    // 공통 Attack() 호출 시 실행되는 실제 공격 로직
    protected override void OnAttack()
    {
        Debug.Log("Staff Attack");
    }
    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        if (mousePos.x < playerScreenPoint.x)
        {
            transform.parent.rotation = Quaternion.Euler(0, -180, angle);
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            transform.parent.rotation = Quaternion.Euler(0, 0, angle);
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
