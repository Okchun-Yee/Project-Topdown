using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCross : BaseSkill
{
    [SerializeField] private int slashcount;
    [SerializeField] private GameObject skill2_AnimPrefab; // 참격 애니메이션 프리팹
    [SerializeField] private Transform weaponCollider; // 참격 콜라이더
    private Transform skill2_SpawnPoint; // 참격 애니메이션 생성 위치
    private Animator anim; // 무기 프리팹에 붙은 Animator
    private GameObject skill2_Anim; // 현재 활성화된 참격 애니메이션 인스턴스

    readonly int SKILL2_HASH = Animator.StringToHash("Skill_CrossSword");
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        skill2_SpawnPoint = GameObject.Find("Skill_SpawnPoint").transform;
        if (skill2_SpawnPoint == null)
        {
            Debug.LogError("Skill2 SpawnPoint not found!");
        }
    }

    protected override void OnSkillActivated()
    {
        // 스킬 활성화 로직
        Debug.Log("SwordCross Activated");

        anim.SetTrigger(SKILL2_HASH); // 애니메이션 트리거 설정
        SkillUIManager.Instance.OnSkillUsed(1); // 스킬 사용 UI 업데이트
        StartCoroutine(PerformSkill());
    }
    private IEnumerator PerformSkill()
    {
        // 콜라이더 활성화
        weaponCollider.gameObject.SetActive(true);

        // 참격 애니메이션 프리팹 생성
        skill2_Anim = Instantiate(skill2_AnimPrefab, skill2_SpawnPoint.position, Quaternion.identity);
        skill2_Anim.transform.parent = this.transform.parent; // 참격 애니메이션을 월드 공간에 생성

        // 콜라이더 비활성화 및 애니메이션 제거
        yield return new WaitForSeconds(0.5f); // 애니메이션 지속 시간에 맞춰 조정
        weaponCollider.gameObject.SetActive(false);
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
