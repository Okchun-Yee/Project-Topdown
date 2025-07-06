using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveInventory : MonoBehaviour
{
    [SerializeField] private Image curWeaponImage; // UI 이미지
    [SerializeField] private float iconSize;

    private void OnEnable()
    {
        // WeaponManager 이벤트 구독
        if (WeaponManager.Instance != null){ WeaponManager.Instance.onCategoryIconChanged += UpdateIcon; }
        
    }
    private void OnDisable()
    {
        if (WeaponManager.Instance != null){ WeaponManager.Instance.onCategoryIconChanged -= UpdateIcon; }
        
    }
    private void UpdateIcon(Sprite newIcon)
    {
        // 아이콘 변경 또는 숨김
        curWeaponImage.sprite = newIcon;
        curWeaponImage.enabled = newIcon != null;
    }
}