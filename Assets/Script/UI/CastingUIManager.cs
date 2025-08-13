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

    private void OnEnable()
    {
        Debug.Log("CastingUIManager: Subscribing to ChargingManager and HoldingManager events");
        
        // 차징 이벤트 구독
        ChargingManager.Instance.OnChargingProgress += UpdateSlider;
        ChargingManager.Instance.OnChargingCompleted += HideSlider;
        ChargingManager.Instance.OnChargingCanceled += HideSlider;
        
        // 홀딩 이벤트 구독
        HoldingManager.Instance.OnHoldingStarted += ShowHoldingSlider;
        HoldingManager.Instance.OnHoldingProgress += UpdateHoldingSlider;
        HoldingManager.Instance.OnHoldingEnded += HideSlider;
    }

    private void OnDisable()
    {
        Debug.Log("CastingUIManager: Unsubscribing from ChargingManager and HoldingManager events");
        
        // 차징 이벤트 해제
        if (ChargingManager.Instance != null)
        {
            ChargingManager.Instance.OnChargingProgress -= UpdateSlider;
            ChargingManager.Instance.OnChargingCompleted -= HideSlider;
            ChargingManager.Instance.OnChargingCanceled -= HideSlider;
        }
        
        // 홀딩 이벤트 해제
        if (HoldingManager.Instance != null)
        {
            HoldingManager.Instance.OnHoldingStarted -= ShowHoldingSlider;
            HoldingManager.Instance.OnHoldingProgress -= UpdateHoldingSlider;
            HoldingManager.Instance.OnHoldingEnded -= HideSlider;
        }
    }   

    public void ShowSlider(float duration)
    {
        if (castingSlider == null) return;
        castingSlider.gameObject.SetActive(true);

        castingSlider.maxValue = duration;
        castingSlider.value = 0;
    }

    // 홀딩 시작 시 슬라이더 표시 (무한 모드)
    private void ShowHoldingSlider()
    {
        if (castingSlider == null) return;
        castingSlider.gameObject.SetActive(true);
        castingSlider.maxValue = 100f; // 홀딩은 무한이므로 임의의 큰 값
        castingSlider.value = 0f;
    }

    // 차징용 슬라이더 업데이트
    private void UpdateSlider(float elapsed, float duration)
    {
        if (castingSlider == null) return;
        castingSlider.value = Mathf.Clamp(elapsed, 0, duration);
    }

    // 홀딩용 슬라이더 업데이트 (지속시간 표시)
    private void UpdateHoldingSlider(float duration)
    {
        if (castingSlider == null) return;
        // 홀딩 시간이 길어질수록 슬라이더 값 증가 (최대값은 유동적)
        castingSlider.maxValue = Mathf.Max(castingSlider.maxValue, duration + 10f);
        castingSlider.value = duration;
    }

    private void HideSlider()
    {
        if (castingSlider == null) return;
        castingSlider.gameObject.SetActive(false);
    }
}