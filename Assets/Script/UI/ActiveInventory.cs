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