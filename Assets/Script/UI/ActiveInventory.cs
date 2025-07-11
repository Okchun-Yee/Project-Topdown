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
        Debug.Log($"[ActiveInventory] OnEnable called (InstanceID: {GetInstanceID()})");
        StartCoroutine(SubscribeWhenReady());
    }

    private void OnDestroy()
    {
        Debug.Log($"[ActiveInventory] OnDestroy called (InstanceID: {GetInstanceID()})");
        if (WeaponManager.Instance != null)
        {
            WeaponManager.Instance.onCategoryIconChanged -= UpdateIcon;
            Debug.Log("[ActiveInventory] Unsubscribed from WeaponManager icon change event (OnDestroy)");
        }
    }

    private IEnumerator SubscribeWhenReady()
    {
        while (WeaponManager.Instance == null)
            yield return null;

        Debug.Log($"[ActiveInventory] Subscribing to WeaponManager (WeaponManager.InstanceID: {WeaponManager.Instance.GetInstanceID()})");
        WeaponManager.Instance.onCategoryIconChanged += UpdateIcon;
        Debug.Log("[ActiveInventory] Subscribed to WeaponManager icon change event");
    }
    private void UpdateIcon(Sprite newIcon)
    {
        Debug.Log($"[ActiveInventory] UpdateIcon called (sprite: {(newIcon != null ? newIcon.name : "null")})");
        curWeaponImage.sprite = newIcon;
        curWeaponImage.enabled = newIcon != null;
    }
}