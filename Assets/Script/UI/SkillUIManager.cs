using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUIManager : Singleton<SkillUIManager>
{
    protected override void Awake()
    {
        base.Awake();
        // Debug.Log($"[SkillUIManager] Awake called (InstanceID: {GetInstanceID()})");
    }
    public SkillSlotUI[] skillSlots;
    public void Initialized(SkillInfo[] skillInfos)
    {
        for (int i = 0; i < skillInfos.Length; i++)
        {
            skillSlots[i].SetSkill(skillInfos[i]);
        }
    }
    public void OnSkillUsed(int skillIndex)
    {
        if (skillIndex < 0 || skillIndex >= skillSlots.Length) return;
        skillSlots[skillIndex].TriggerCooldown();
    }
}
