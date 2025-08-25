using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordAura : BaseSkill
{
    [SerializeField] private GameObject auraPrefab; // 오라 애니메이션 프리팹
    [SerializeField] private int skillRange; // 스킬 범위 설정
    [SerializeField] private Transform auraSpawnPoint; // 오라 생성 위치

    readonly int AURA_HASH = Animator.StringToHash("Skill_SwordAura");
    private Animator anim; // 무기 프리팹에 붙은 Animator

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    
    protected override void OnSkillActivated()
    {
        Debug.Log("SwordAura Activated");
        anim.SetTrigger(AURA_HASH); // 애니메이션 트리거 설정
        SkillUIManager.Instance.OnSkillUsed(skillIndex); // 스킬 사용 UI 업데이트
        StartCoroutine(PerformAura());

    }

    private IEnumerator PerformAura()
    {
        // 마우스 위치를 월드 좌표로 변환하여 방향 및 회전 계산
        Camera mainCamera = Camera.main;
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 dir = ((Vector2)mouseWorldPos - (Vector2)auraSpawnPoint.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion auraRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // 오라 애니메이션 프리팹 생성 (마우스 방향)
        GameObject auraInstance = Instantiate(auraPrefab, auraSpawnPoint.position, auraRotation);
        auraInstance.GetComponent<Projectile>().UpdateProjectilRange(skillRange);
        auraInstance.GetComponent<Projectile>().Initialize(CalculateFinalDamage()); // 데미지 설정

        yield return null;
    }
}
