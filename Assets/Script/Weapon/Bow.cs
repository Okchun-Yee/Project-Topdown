using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 활(Bow) 무기 클래스
/// └ BaseWeapon을 상속받아 공통 쿨다운·초기화 로직 자동 적용
/// </summary>
public class Bow : BaseWeapon
{
    [SerializeField] WeaponInfo weaponinfo;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowspawnPoint;
    readonly int FIRE_HASH = Animator.StringToHash("Fire");
    private Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    protected override void OnAttack()
    {
        Debug.Log("Bow Attack");
        BaseWeapon.IsAttacking = true;
        anim.SetTrigger(FIRE_HASH);
        GameObject newArrow = Instantiate(arrowPrefab, arrowspawnPoint.position, ActiveWeapon.Instance.transform.rotation);
        newArrow.GetComponent<Projectile>().UpdateProjectilRange(weaponinfo.weaponRange);

        newArrow.GetComponent<Projectile>().Initialize(weaponinfo.weaponDamage); // Initialize로 데미지 설정

        Debug.Log($"[Bow] weaponinfo.weaponDamage: {weaponinfo.weaponDamage}");

        BaseWeapon.IsAttacking = false; // 공격 완료 상태 설정
    }
    public WeaponInfo GetWeaponInfo()
    {
        return weaponinfo;
    }
}
