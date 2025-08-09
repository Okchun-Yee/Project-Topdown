using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCross : BaseSkill
{
    [SerializeField] private int slashcount;    // 참격 횟수 (예: 2회 연속 참격)
    [SerializeField] private GameObject skill2_AnimPrefab; // 참격 애니메이션 프리팹
    [SerializeField] private Transform weaponCollider; // 참격 콜라이더

    private Transform skill2_SpawnPoint; // 참격 애니메이션 생성 위치
    private Animator anim; // 무기 프리팹에 붙은 Animator
    private GameObject skill2_Anim; // 현재 활성화된 참격 애니메이션 인스턴스
    private DashMove dashMove; // 대시 이동 컴포넌트
    private Vector2 dashDir; //대시 방향

    private float dashForce = 10f;  // 대시 힘
    private int activationCount = 0;

    readonly int SKILL2_HASH = Animator.StringToHash("Skill_CrossSword");
    private void Awake()
    {
        anim = GetComponent<Animator>();
        dashMove = GetComponentInParent<DashMove>();
    }
    private void Start()
    {
        skill2_SpawnPoint = GameObject.Find("Skill_SpawnPoint").transform;
    }

    protected override void OnSkillActivated()
    {
        // 스킬 활성화 로직
        Debug.Log("SwordCross Activated");
        activationCount++; //호출 횟수 증가

        // 대시 방향 결정
        dashDir = PlayerController.Instance.FacingLeft ? Vector2.left : Vector2.right;
        
        SkillUIManager.Instance.OnSkillUsed(1); // 스킬 사용 UI 업데이트
        StartCoroutine(PerformSkill());
    }
    private IEnumerator PerformSkill()
    {
        BaseSkill.IsCasting = true; // 스킬 사용 중 상태 설정
        // 콜라이더 활성화
        weaponCollider.gameObject.SetActive(true);

        for (int i = 0; i < slashcount; i++)
        {
            anim.SetTrigger(SKILL2_HASH); // 애니메이션 트리거 설정
            
            // 참격 애니메이션 프리팹 생성
            skill2_Anim = Instantiate(skill2_AnimPrefab, skill2_SpawnPoint.position, Quaternion.identity);
            skill2_Anim.transform.parent = this.transform.parent; // 참격 애니메이션을 Player의 자식으로 설정

            dashMove.Dash(dashDir, dashForce, 0.15f);

            // 콜라이더 비활성화 및 애니메이션 제거
            yield return new WaitForSeconds(0.5f); // 애니메이션 지속 시간에 맞춰 조정
        }

        weaponCollider.gameObject.SetActive(false);
        BaseSkill.IsCasting = false; // 스킬 사용 완료 상태 설정
        Destroy(skill2_Anim);
    }

    // 애니메이션 이벤트: 콤보 공격 애니메이션에서 호출
    //상단 휘두를 때 슬래시 방향 회전
    public void SkillUpFlipAnimEvent()
    {
        if (skill2_Anim == null) return;
        skill2_Anim.transform.rotation = Quaternion.Euler(-180, 0, 0);
        if (PlayerController.Instance.FacingLeft)
        {
            var sr = skill2_Anim.GetComponent<SpriteRenderer>();
            if (sr != null) sr.flipX = true;
        }
    }
    // 애니메이션 이벤트: 콤보 공격 애니메이션에서 호출
    //하단 휘두를 때 슬래시 방향 회전
    public void SkillDownFlipAnimEvent()
    {
        if (skill2_Anim == null) return;
        skill2_Anim.transform.rotation = Quaternion.Euler(0, 0, 0);
        if (PlayerController.Instance.FacingLeft)
        {
            var sr = skill2_Anim.GetComponent<SpriteRenderer>();
            if (sr != null) sr.flipX = true;
        }
    }
}
