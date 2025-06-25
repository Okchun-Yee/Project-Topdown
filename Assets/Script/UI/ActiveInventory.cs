using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveInventory : MonoBehaviour
{
    [SerializeField] private Image curWeaponImage; // UI 이미지
    [SerializeField] private WeaponManager weaponManager;
    [SerializeField] private float iconSize;

    private void Start()
    {
        if (weaponManager == null)
            weaponManager = FindObjectOfType<WeaponManager>();

        weaponManager.onCategoryIconChanged += UpdateIcon;
        // 시작 시 한번 강제 호출로 현재 무기 아이콘과 동기화
        weaponManager.WeaponCategoryChange(weaponManager.StartingCategory);
    }
    private void UpdateIcon(Sprite newIcon)
    {
        curWeaponImage.sprite = newIcon;
        curWeaponImage.rectTransform.localScale = Vector3.one * iconSize;
    }
    private void OnDestroy()
    {
        weaponManager.onCategoryIconChanged -= UpdateIcon;
    }
}