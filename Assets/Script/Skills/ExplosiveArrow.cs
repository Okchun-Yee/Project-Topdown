using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveArrow : BaseSkill
{
    [SerializeField] private GameObject arrowPrefab; // 폭발 화살 애니메이션 프리팹
    [SerializeField] private Transform arrowspawnPoint; // 화살 생성 위치
    [SerializeField] private float projectileRange = 10f; // 화살의 사거리
    private Animator anim; // 무기 프리팹에 붙은 Animator
    readonly int FIRE_HASH = Animator.StringToHash("Fire");

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    protected override void OnSkillActivated()
    {
        Debug.Log("ExplosiveArrow Activated");
        anim.SetTrigger(FIRE_HASH); // 애니메이션 트리거 설정
        SkillUIManager.Instance.OnSkillUsed(0); // 스킬 사용 UI 업데이트
        GameObject newArrow = Instantiate(arrowPrefab, arrowspawnPoint.position, ActiveWeapon.Instance.transform.rotation);
        newArrow.GetComponent<Projectile>().UpdateProjectilRange(projectileRange);
    }
}
