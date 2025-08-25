using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillCategory
{
    Charging,
    Performed,
    Holding
}

[CreateAssetMenu(menuName = "New Skill")]
public class SkillInfo : ScriptableObject
{
    [Header("Common")]
    [SerializeField] private float skillCooldown;
    public float CastingTime; // 스킬 시전 시간
    public SkillCategory skillCategory; // 스킬 카테고리
    [Header("Damage ( %)")]
    public float skillDamage;
    [Header("Icon")]
    [SerializeField] private Sprite icon;               // UI용 아이콘

    //공격 쿨다운 시간(초)
    public float CooldownTime => skillCooldown;
    public Sprite Icon => icon; 

}
