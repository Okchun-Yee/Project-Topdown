using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "New Skill")]
public class SkillInfo : ScriptableObject
{
    [Header("Common")]
    [SerializeField] private float skillCooldown;
    public int skillDamage;
    [Header("연출")]
    [SerializeField] private Sprite icon;               // UI용 아이콘

    //공격 쿨다운 시간(초)
    public float CooldownTime => skillCooldown;
    public Sprite Icon => icon; 

}
