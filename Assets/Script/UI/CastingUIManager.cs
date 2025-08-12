using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastingUIManager : Singleton<CastingUIManager>
{
    [SerializeField] private Slider castingSlider;

    protected override void Awake()
    {
        base.Awake();
        if (castingSlider != null)
            castingSlider.gameObject.SetActive(false);
    }

    private void Start()
    {
        TrySubscribeEvents();
    }

    private void TrySubscribeEvents()
    {
        if (ChargingManager.Instance != null)
        {
            ChargingManager.Instance.OnChargingProgress += UpdateSlider;
            ChargingManager.Instance.OnChargingCompleted += HideSlider;
            ChargingManager.Instance.OnChargingCanceled += HideSlider;
        }
        else
        {
            Debug.LogWarning("CastingUIManager: ChargingManager.Instance가 null입니다! 구독 재시도 필요.");
        }
    }
    public void ShowSlider(float duration)
    {
        if (castingSlider == null) return;
        castingSlider.maxValue = duration;
        castingSlider.value = 0;
        castingSlider.gameObject.SetActive(true);
    }

    private void UpdateSlider(float elapsed, float duration)
    {
        Debug.Log($"[CastingUIManager] UpdateSlider: {elapsed}/{duration}");
        if (castingSlider == null) return;
        castingSlider.value = Mathf.Clamp(elapsed, 0, duration);
    }

    private void HideSlider()
    {
        if (castingSlider == null) return;
        castingSlider.gameObject.SetActive(false);
    }
}