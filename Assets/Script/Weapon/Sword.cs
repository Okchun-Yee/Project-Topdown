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
    [SerializeField] private GameObject slashAnimPrefab;
    [SerializeField] private Transform weaponCollider;
    private Transform slashAnimSpawnPoint;

    private Animator anim;
    private GameObject slashAnim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        slashAnimSpawnPoint = GameObject.Find("Slash SpawnPoint").transform;
    }
    private void Update()
    {
        MouseFollowWithOffset();
    }
    protected override void OnAttack()
    {
        Debug.Log("Sword Attack");
        BaseWeapon.IsAttacking = true; // 공격 중 상태 설정
        anim.SetTrigger("isAttack");
        weaponCollider.gameObject.SetActive(true);

        slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
        slashAnim.transform.parent = this.transform.parent;
    }
    public void DoneAttackingAnimEvent()
    {
        weaponCollider.gameObject.SetActive(false);
        BaseWeapon.IsAttacking = false; // 공격 완료 상태로 변경
    }
    /// <summary>
    /// 애니메이션 이벤트: 위로 휘두를 때 슬래시 방향 회전
    /// </summary>
    public void SwingUpFlipAnimEvent()
    {
        if (slashAnim == null) return;
        slashAnim.transform.rotation = Quaternion.Euler(-180, 0, 0);
        if (PlayerController.Instance.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }
    public void SwingDownFlipAnimEvent()
    {
        if (slashAnim == null) { return; }
        slashAnim.transform.rotation = Quaternion.Euler(0, 0, 0);
        if (PlayerController.Instance.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }
    private void MouseFollowWithOffset()
    {
        if (BaseSkill.IsCasting || BaseWeapon.IsAttacking) { return; }

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
