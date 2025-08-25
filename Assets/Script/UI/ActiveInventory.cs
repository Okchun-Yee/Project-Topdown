using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveInventory : MonoBehaviour
{
    [SerializeField] private Image curWeaponImage; // UI 이미지
    [SerializeField] private float iconSize;

    private WeaponManager lastWeaponManager;

    private void Update()
    {
        if (WeaponManager.Instance != lastWeaponManager)
        {
            // 이전 구독 해제
            if (lastWeaponManager != null)
            {
                lastWeaponManager.onCategoryIconChanged -= UpdateIcon;
                Debug.Log("[ActiveInventory] Unsubscribed from old WeaponManager");
            }
            // 새 인스턴스에 구독
            if (WeaponManager.Instance != null)
            {
                WeaponManager.Instance.onCategoryIconChanged += UpdateIcon;
                Debug.Log("[ActiveInventory] Subscribed to new WeaponManager");
            }
            lastWeaponManager = WeaponManager.Instance;
        }
    }
    private void OnEnable()
    {
        StartCoroutine(SubscribeWhenReady());
    }

    private void OnDestroy()
    {
        if (WeaponManager.Instance != null)
        {
            WeaponManager.Instance.onCategoryIconChanged -= UpdateIcon;
        }
    }

    private IEnumerator SubscribeWhenReady()
    {
        while (WeaponManager.Instance == null)
            yield return null;

        WeaponManager.Instance.onCategoryIconChanged += UpdateIcon;
    }
    private void UpdateIcon(Sprite newIcon)
    {
        curWeaponImage.sprite = newIcon;
        curWeaponImage.enabled = newIcon != null;
    }
}