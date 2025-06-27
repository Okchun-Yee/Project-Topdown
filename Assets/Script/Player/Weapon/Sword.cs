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
    private Animator myAnim;
    private PlayerController playerController;
    private GameObject        slashAnim;

    private void Awake()
    {
        myAnim = GetComponent<Animator>();
        playerController = GetComponentInParent<PlayerController>();

        // ① 부모가 WeaponCollider 이므로
        // 부모가 WeaponCollider 오브젝트이므로
        var mount = transform.parent;
        if (mount == null)
        {
            Debug.LogError("[Sword] WeaponCollider(부모)가 없습니다.");
            return;
        }
        weaponColliderGO = mount.gameObject;          // BaseWeapon 필드 사용
        weaponColliderGO.SetActive(false);
        // ② 이펙트 스폰 포인트
        slashSpawnPoint = mount.Find("EffectSpawnPoint");
        if (slashSpawnPoint == null)
            Debug.LogError($"[Sword] EffectSpawnPoint가 없습니다.");
    }
    private void Update()
    {
        MouseFollowOffset();
    }
    /// <summary>
    /// BaseWeapon.Attack() 호출 시 실행되는 실제 공격 로직
    /// </summary>
    protected override void OnAttack()
    {
        myAnim.SetTrigger("isAttack");
        weaponColliderGO.gameObject.SetActive(true);

        Instantiate(
            slashAnimPrefab,
            slashSpawnPoint.position,
            Quaternion.identity,
            slashSpawnPoint
        );
    }

    public void DoneAttackingAnimEvent()
    {
        weaponColliderGO.gameObject.SetActive(false);
    }
    public void SwingUpFilpAnimEvent()
    {
        if (slashAnim == null) return;
        slashAnim.transform.rotation = Quaternion.Euler(-180, 0, 0);
        if (playerController.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }
    public void SwingDownFilpAnimEvent()
    {
        slashAnim.transform.rotation = Quaternion.Euler(0, 0, 0);
        if (playerController.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }
    private void MouseFollowOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(playerController.transform.position);

        float angle = Mathf.Atan2(mousePos.y - playerScreenPoint.y, mousePos.x - playerScreenPoint.x) * Mathf.Rad2Deg;

        if (mousePos.x < playerScreenPoint.x)
        {
            transform.parent.rotation = Quaternion.Euler(0, -180, angle);
            weaponColliderGO.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            transform.parent.rotation = Quaternion.Euler(0, 0, angle);
            weaponColliderGO.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
