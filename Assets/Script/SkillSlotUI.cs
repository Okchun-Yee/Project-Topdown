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
    }

    public void TriggerCooldown()
    {
        isOnCooldown = true;
        remainingCooldown = cooldownTime;
    }
    private void Update()
    {
        if (!isOnCooldown) { return; }
        remainingCooldown -= Time.deltaTime;
        cooldownOverlay.fillAmount = remainingCooldown / cooldownTime;
        if (remainingCooldown <= 0f)
        {
            isOnCooldown = false;
            cooldownOverlay.fillAmount = 0f;
        }
    }
}
