using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 스태프(Staff) 무기 클래스
/// └ BaseWeapon을 상속받아 공통 쿨다운·초기화 로직 자동 적용
/// </summary>
public class Staff : BaseWeapon
{
    [SerializeField] private WeaponInfo weaponinfo;
    [SerializeField] private GameObject magicLaser;
    [SerializeField] private Transform magicLaserSpawnPoint;
    private Animator anim;
    readonly int ATTACK_HASH = Animator.StringToHash("Attack");

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        MouseFollowWithOffset();
    }
    // 공통 Attack() 호출 시 실행되는 실제 공격 로직
    protected override void OnAttack()
    {
        Debug.Log("Staff Attack");
        BaseWeapon.IsAttacking = true; // 공격 중 상태 설정
        anim.SetTrigger(ATTACK_HASH);
        // 애니메이션 이벤트를 통해 SpawnStaffProjectileAnimEvent() 호출
    }

    public void SpawnStaffProjectileAnimEvent()
    {
        // 마법 레이저 생성
        GameObject newLaser = Instantiate(magicLaser, magicLaserSpawnPoint.position, Quaternion.identity);
        newLaser.GetComponent<MagicLaser>().UpdateLaserRange(weaponinfo.weaponRange);
    }
    public void ResetAttackState()
    {
        BaseWeapon.IsAttacking = false; // 공격 완료 상태로 변경
    }

    // Animation Event용 함수들
    public void StartCasting()
    {
        BaseSkill.IsCasting = true; // 스킬 사용 중 상태 설정
        Debug.Log("Staff: Casting Started");
    }

    public void EndCasting()
    {
        BaseSkill.IsCasting = false; // 스킬 사용 완료 상태로 변경
        Debug.Log("Staff: Casting Ended");
    }
    public WeaponInfo GetWeaponInfo()
    {
        return weaponinfo;
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
        }
        else
        {
            transform.parent.rotation = Quaternion.Euler(0, 0, angle);
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
