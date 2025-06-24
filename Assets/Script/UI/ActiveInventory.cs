using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveInventory : MonoBehaviour
{
    [SerializeField] private Image curWeaponImage; // UI 이미지
    [SerializeField] private string childObjectName = "Weapon_Sword";
    [SerializeField] private float iconSize;

    private void Start()
    {
        // 무기 오브젝트 찾기
        GameObject weaponObject = GameObject.Find(childObjectName);
        if (weaponObject != null)
        {
            SpriteRenderer spriteRenderer = weaponObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && curWeaponImage != null)
            {
                curWeaponImage.sprite = spriteRenderer.sprite;
                curWeaponImage.SetNativeSize();
                var rt = curWeaponImage.rectTransform;
                rt.sizeDelta = new Vector2(rt.sizeDelta.x * iconSize, rt.sizeDelta.y * iconSize);
            }
            else
            {
                Debug.LogWarning("SpriteRenderer 또는 curWeaponImage가 null입니다.");
            }
        }
        else
        {
            Debug.LogWarning("무기 오브젝트를 찾을 수 없습니다.");
        }
    }
}