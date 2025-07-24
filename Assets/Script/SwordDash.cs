using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordDash : BaseSkill
{
    [SerializeField] private float dashForce = 10f;
    private Transform dashCollider;
    private Rigidbody2D rb;
    private Camera mainCamera;
    private Animator anim;
    public bool IsDashing { get; private set; } // 대시 중인지 여부를 나타내는 프로퍼티

    private void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        if (transform.parent != null)
        {
            dashCollider = transform.root.Find("DashCollider");
        }
        anim = GetComponentInParent<Animator>();
        mainCamera = Camera.main;
    }
    protected override void OnSkillActivated()
    {
        if (rb == null || mainCamera == null)
        {
            Debug.LogError("Rigidbody2D or Camera not found!");
            return;
        }
        anim.SetTrigger("SwordDash"); // 대시 애니메이션 트리거 설정
        dashCollider.gameObject.SetActive(true);
        SkillUIManager.Instance.OnSkillUsed(skillIndex); // 스킬 사용 UI 업데이트
        StartCoroutine(PerformDash());
    }

    private IEnumerator PerformDash()
    {
        IsDashing = true;
        PlayerHealth.Instance.DamageRecoveryTime(); // 대시 중에는 피해를 입지 않도록 설정
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 dir = ((Vector2)mouseWorldPos - rb.position).normalized;
        rb.velocity = Vector2.zero; // 기존 속도 초기화
        rb.AddForce(dir * dashForce, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.15f); // 대시 지속 시간

        rb.velocity = Vector2.zero; // 대시 종료 후 속도 초기화
        IsDashing = false;
        dashCollider.gameObject.SetActive(false);
    }
}
