using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordDash : BaseSkill
{
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private GameObject dashAnimPrefab; // 대시 애니메이션 프리팹

    private Transform dashCollider;
    private Transform dashSpawnPoint; // 대시 애니메이션 생성 위치

    private Rigidbody2D rb;
    private Camera mainCamera;
    private GameObject dashAnim; // 현재 활성화된 대시 애니메이션 인스턴스

    public bool IsDashing { get; private set; } // 대시 중인지 여부를 나타내는 프로퍼티

    private void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        dashCollider = transform.root.Find("DashCollider");
        if (dashCollider == null)
        {
            Debug.LogError("DashCollider not found!");
        }
        mainCamera = Camera.main;
    }
    private void Start()
    {
        dashSpawnPoint = GameObject.Find("DashSpawnPoint").transform;
    }
    protected override void OnSkillActivated()
    {
        if (rb == null || mainCamera == null)
        {
            Debug.LogError("Rigidbody2D or Camera not found!");
            return;
        }

        // SwordDash 충돌 및 UI 시동 로직
        dashCollider.gameObject.SetActive(true);
        SkillUIManager.Instance.OnSkillUsed(0); // 스킬 사용 UI 업데이트
        StartCoroutine(PerformDash());
    }

    private IEnumerator PerformDash()
    {
        IsDashing = true;
        PlayerHealth.Instance.DamageRecoveryTime();

        //마우스 위치를 월드 좌표로 변환 => 참격 애니메이션 프리팹 생성, 대쉬 애니메이션 생성 방향 결정에 사용
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 dir = ((Vector2)mouseWorldPos - rb.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion dashRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //참격 애니메이션 프리펩 생성
        dashAnim = Instantiate(dashAnimPrefab, dashSpawnPoint.position, dashRotation);
        dashAnim.transform.parent = null; // 대시 애니메이션을 월드 공간에 생성

        // 대시 위치 설정
        rb.velocity = Vector2.zero;
        rb.AddForce(dir * dashForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.15f);

        rb.velocity = Vector2.zero;
        IsDashing = false;
        dashCollider.gameObject.SetActive(false);
    }
}
