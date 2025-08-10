using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCasting : MonoBehaviour
{
    private bool isCasted = false;  
    private float castingTimer = 0f;
    private void Update()
    {
        if (isCasted)
        {
            // Implement your skill casting logic here
            castingTimer -= Time.deltaTime;
            if (castingTimer <= 0f)
            {
                Debug.Log("Skill ON!");
                isCasted = false; // 시전 완료 후 상태 초기화
                // 실제 스킬 발동 코드
            }
        }
    }
    public void SkillCasting(float castingTime)
    {
        if (isCasted) { return; } // 이미 시전 중이면 무시
        castingTimer = castingTime;
        isCasted = true;
        Debug.Log("Skill Casting Start!");
    }
}
