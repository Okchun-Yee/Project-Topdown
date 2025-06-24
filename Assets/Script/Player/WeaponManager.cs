using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public delegate void OnWeaponChanged(Sprite newWeaponSprite);
    public  event OnWeaponChanged onWeaponChanged;

    private GameObject currentWeapon;

    public void EquipWeapon(GameObject newWeapon)
    {
        currentWeapon = newWeapon;

        // 무기 Sprite를 가져와 이벤트 발생
        SpriteRenderer sr = newWeapon.GetComponent<SpriteRenderer>();
        if (sr != null && onWeaponChanged != null)
        {
            onWeaponChanged.Invoke(sr.sprite);
        }
    }
}
