using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveInventory : MonoBehaviour
{
    [SerializeField] private Image curWeaponImage;
    [SerializeField] private ActiveWeapon activeWeapon;

    private void Start()
    {
        // WeaponManager를 할당하거나 Find로 가져올 수도 있음
        if (activeWeapon == null)
        {
            activeWeapon = FindObjectOfType<ActiveWeapon>();
        }

        if (activeWeapon != null)
        {
            activeWeapon.onWeaponChanged += UpdateWeaponIcon;
        }
    }

    private void UpdateWeaponIcon(Sprite newSprite)
    {
        if (curWeaponImage != null && newSprite != null)
        {
            curWeaponImage.sprite = newSprite;
        }
    }

    private void OnDestroy()
    {
        if (activeWeapon != null)
            activeWeapon.onWeaponChanged -= UpdateWeaponIcon;
    }
}
