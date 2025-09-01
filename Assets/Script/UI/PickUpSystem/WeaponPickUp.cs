using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private WeaponInfo weaponinfo;
    [SerializeField] private AudioSource pickUpSound;
    
    public void Weapon_Pickup()
    {
        // 1) 무기 장착 요청
        WeaponManager.Instance.EquipWeapon(weaponinfo);

        // 2) 획득 효과 재생 (사운드, 파티클)
        //pickUpSound.Play();

        // 3) 즉시 제거하여 리소스 최소화
        Destroy(gameObject);
    }
}
