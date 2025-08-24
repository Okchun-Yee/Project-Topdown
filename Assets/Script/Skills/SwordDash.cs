using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordDash : BaseSkill
{
    [SerializeField] private float dashForce;
    [SerializeField] private GameObject dashAnimPrefab; // 대시 애니메이션 프리팹

    private Transform dashCollider;
    private Transform dashSpawnPoint; // 대시 애니메이션 생성 위치

    private Rigidbody2D rb;
    private Camera mainCamera;
    private GameObject dashAnim; // 현재 활성화된 대시 애니메이션 인스턴스
    private DashMove dashMove; // 대시 이동 컴포넌트

    private void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        dashCollider = transform.root.Find("DashCollider");
        mainCamera = Camera.main;
        dashMove = GetComponentInParent<DashMove>();
    }
    private void Start()
    {
        dashSpawnPoint = GameObject.Find("DashSpawnPoint").transform;
    }
    protected override void OnSkillActivated()
    {
        Debug.Log("SwordDash Activated");

        // SwordDash 충돌 및 UI 시동 로직
        dashCollider.gameObject.SetActive(true);
        //Dash 데미지 지정
        dashCollider.GetComponent<DamageSource>()?.SetDamage(CalculateFinalDamage());

        SkillUIManager.Instance.OnSkillUsed(skillIndex); // 스킬 사용 UI 업데이트
        StartCoroutine(PerformDash());
    }

    private IEnumerator PerformDash()
    {
        BaseSkill.IsCasting = true; // 스킬 사용 중 상태 설정
        PlayerHealth.Instance.DamageRecoveryTime();

        //마우스 위치를 월드 좌표로 변환 => 참격 애니메이션 프리팹 생성, 대쉬 애니메이션 생성 방향 결정에 사용
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 dir = ((Vector2)mouseWorldPos - rb.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion dashRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //참격 애니메이션 프리펩 생성
        dashAnim = Instantiate(dashAnimPrefab, dashSpawnPoint.position, dashRotation);
        dashAnim.transform.parent = null; // 대시 애니메이션을 월드 공간에 생성

        // 대시 설정
        dashMove.Dash(dir, dashForce, 0.15f);

        yield return new WaitForSeconds(0.15f);
        dashCollider.gameObject.SetActive(false);
        BaseSkill.IsCasting = false; // 스킬 사용 완료 상태 설정
    }
}
