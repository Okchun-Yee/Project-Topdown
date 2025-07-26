using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class BaseSkill : MonoBehaviour, ISkill
{
    protected bool isOnCooldown = false;
    public SkillInfo SkillInfo;
    public UnityEvent<int> onSkillUsed;     // 이벤트를 통해 스킬 사용 알림
    // UI 연동용 스킬 인덱스 (예: Skill1 → 0, Skill2 → 1)
    [HideInInspector]
    public int skillIndex;
    public void Initialize(SkillInfo info)
    {
        SkillInfo = info;
        // 추가 초기화 로직이 필요하다면 여기에 작성
    }
    public void ActivateSkill()
    {
        if (!isOnCooldown)
        {
            StartCoroutine(ActivateRoutine());
        }
    }

    private IEnumerator ActivateRoutine()
    {
        isOnCooldown = true;
        onSkillUsed?.Invoke(skillIndex); // 스킬 사용 이벤트 호출
        OnSkillActivated();

        yield return new WaitForSeconds(SkillInfo.CooldownTime);
        isOnCooldown = false;
    }
    protected abstract void OnSkillActivated();
}
