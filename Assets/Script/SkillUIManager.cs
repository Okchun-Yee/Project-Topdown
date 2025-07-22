using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUIManager : MonoBehaviour
{
    public SkillSlotUI[] skillSlots;
    public void Initialized(SkillInfo[] skillInfos)
    {
        for (int i = 0; i < skillInfos.Length; i++)
        {
            skillSlots[i].SetSkill(skillInfos[i].Icon, skillInfos[i].CooldownTime);
        }
    }
    public void OnSkillUsed(int skillIndex)
    {
        if (skillIndex < 0 || skillIndex >= skillSlots.Length) return;
        skillSlots[skillIndex].TriggerCooldown();
    }
}
