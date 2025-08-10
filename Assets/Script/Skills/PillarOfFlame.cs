using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PillarOfFlame : BaseSkill
{
    [SerializeField] private GameObject pillarPrefab;   //불기둥 애니메이션 프리팹
    [SerializeField] private float maxRange = 7f; // 최대 범위
    [SerializeField] private float minRange = 3f; // 최소 범위
    [SerializeField] private float castingTime = 2f; // 시전 시간
    private Animator anim;
    private readonly int PILLAR_HASH = Animator.StringToHash("Skill_PillarOfFlame");

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    protected override void OnSkillActivated()
    {
        Debug.Log("PillarOfFlame Activated");
        IsCasting = true; // 스킬 사용 중 상태 설정
        anim.SetTrigger(PILLAR_HASH); // 애니메이션 트리거 설정

        // 마우스 위치를 월드 좌표로 변환
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        Vector3 playerPos = PlayerController.Instance.transform.position;
        Vector3 dir = (mouseWorldPos - playerPos).normalized;
        float dist = Vector3.Distance(playerPos, mouseWorldPos);

        float clampedDist = Mathf.Clamp(dist, minRange, maxRange);
        Vector3 spawnPos = playerPos + dir * clampedDist;

        // 프리팹 생성
        Instantiate(pillarPrefab, spawnPos, Quaternion.identity);

        // 스킬 사용 UI 업데이트
        SkillUIManager.Instance.OnSkillUsed(0); // 예시로 0번 스킬로 업데이트

        IsCasting = false; // 스킬 사용 완료 상태로 변경 
    }
}
