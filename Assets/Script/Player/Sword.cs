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
    [SerializeField] private Transform slashAnimSpawnPoint;
    private Transform weaponCollider;
    //무기 종류 체크
    private Animator myAnim;
    private GameObject slashAnim;

    //private bool facingLeft = false;
    private void Awake()
    {
        myAnim = GetComponent<Animator>();
    }
    private void Start()
    {
        weaponCollider = PlayerController.Instance.GetWeaponCollider();
        slashAnimSpawnPoint = GameObject.Find("EffectSpawnPoint").transform;
    }
    private void Update()
    {
        MouseFollowOffset();
    }
    protected override void OnAttack()
    {
        //isAttacking = true;
        myAnim.SetTrigger("isAttack");
        weaponCollider.gameObject.SetActive(true);

        slashAnim = Instantiate(slashAnimPrefab, slashAnimSpawnPoint.position, Quaternion.identity);
        slashAnim.transform.parent = this.transform.parent;

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
            transform.rotation = Quaternion.Euler(0, -180, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, angle);
            weaponCollider.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
