using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Timeline;

[RequireComponent(typeof(Animator))]
public class Sword : WeaponBase
{
    [Header("Sword Settings")]
    [SerializeField] private GameObject slashAnimPrefab;

    private Transform slashAnimSpawnPoint;
    private Transform weaponCollider;
    private Animator myAnim;
    private GameObject slashAnim;

    //private bool facingLeft = false;
     // Awake는 지우고 Start만 사용
    public void Initialize(Transform collider, Transform spawnPoint, Animator animator)
    {
        weaponCollider       = collider;
        slashAnimSpawnPoint  = spawnPoint;
        myAnim               = animator;
    }
    private void Update()
    {
        MouseFollowOffset();
    }
    protected override void OnAttack()
    {
        Debug.Log($"{name}: OnAttack() 실행 (Start 호출 후 프레임?)");

        // 최종 null 체크
        if (myAnim == null || weaponCollider == null || slashAnimSpawnPoint == null)
        {
            Debug.LogError($"{name}: OnAttack 진입 시 참조 누락! " +
                $"myAnim={myAnim}, weaponCollider={weaponCollider}, slashAnimSpawnPoint={slashAnimSpawnPoint}");
            return;
        }


        // 실제 공격 동작
        myAnim.SetTrigger("isAttack");
        weaponCollider.gameObject.SetActive(true);

        slashAnim = Instantiate(
            slashAnimPrefab,
            slashAnimSpawnPoint.position,
            Quaternion.identity);
        slashAnim.transform.parent = transform.parent;

        StartCoroutine(EndAttackRoutine());
    }

    private IEnumerator EndAttackRoutine()
    {
        yield return new WaitForSeconds(info.weaponCooldown);
        ActiveWeapon.Instance.ToggleIsAttacking(false);
    }
    // 애니메이션 이벤트: 필요 시 콜라이더 비활성화
    public void DoneAttackingAnimEvent() => weaponCollider.gameObject.SetActive(false);
    public void SwingUpFilpAnimEvent()
    {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(-180, 0, 0);
        if (PlayerController.Instance.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }
    public void SwingDownFilpAnimEvent()
    {
        slashAnim.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        if (PlayerController.Instance.FacingLeft)
        {
            slashAnim.GetComponent<SpriteRenderer>().flipX = true;
        }
    }
    private void MouseFollowOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        if (mousePos.x < playerScreenPoint.x)
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
