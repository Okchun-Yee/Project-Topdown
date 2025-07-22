using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlotUI : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private Image cooldownOverlay;

    private float cooldownTime;
    private float remainingCooldown;
    private bool isOnCooldown;

    public void SetSkill(Sprite sprite, float cooldown)
    {
        iconImage.sprite = sprite;
        cooldownTime = cooldown;
        remainingCooldown = 0f;
        Debug.Log($"[SkillSlotUI] SetSkill: sprite={sprite?.name}, cooldown={cooldown}");
    }

    public void TriggerCooldown()
    {
        isOnCooldown = true;
        remainingCooldown = cooldownTime;

        cooldownOverlay.gameObject.SetActive(true); // 쿨다운 시작 → 활성화
        cooldownOverlay.fillAmount = 1f; // 쿨다운 오버레이 초기화
        Debug.Log($"[SkillSlotUI] TriggerCooldown: cooldownTime={cooldownTime}");
    }
    private void Update()
    {
        if (!isOnCooldown) { return; }

        remainingCooldown -= Time.deltaTime;
        float ratio = remainingCooldown / cooldownTime;
        cooldownOverlay.fillAmount = ratio;

        if (remainingCooldown <= 0f)
        {
            isOnCooldown = false;
            cooldownOverlay.gameObject.SetActive(false); // 쿨다운 끝 → 숨김
            Debug.Log("[SkillSlotUI] Cooldown Ended");
        }
    }
}
