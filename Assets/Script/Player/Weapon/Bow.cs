using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 활(Bow) 무기 클래스
/// └ BaseWeapon을 상속받아 공통 쿨다운·초기화 로직 자동 적용
/// </summary>
public class Bow : BaseWeapon
{
    [SerializeField] Weaponinfo weaponinfo;
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
        anim.SetTrigger(FIRE_HASH);
        GameObject newArrow = Instantiate(arrowPrefab, arrowspawnPoint.position, ActiveWeapon.Instance.transform.rotation);
        newArrow.GetComponent<Projectile>().UpdateProjectilRange(weaponinfo.weaponRange);
    }
    public Weaponinfo GetWeaponInfo()
    {
        return weaponinfo;
    }
}
