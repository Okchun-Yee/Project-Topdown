using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainArrow : BaseSkill
{
    [SerializeField] private GameObject arrowPrefab;
    private Animator anim;
    private readonly int RAIN_ARROW_HASH = Animator.StringToHash("Fire");
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    protected override void OnSkillActivated()
    {
        Debug.Log("RainArrow Activated");
        anim.SetTrigger(RAIN_ARROW_HASH); // 애니메이션 트리거 설정
        SkillUIManager.Instance.OnSkillUsed(2); // 스킬 사용 UI 업데이트
        Instantiate(arrowPrefab, transform.position, ActiveWeapon.Instance.transform.rotation);
    }
}
