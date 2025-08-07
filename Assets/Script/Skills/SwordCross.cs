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
}
