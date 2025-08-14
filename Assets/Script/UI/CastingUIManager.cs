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
        
        // 홀딩 이벤트 구독 (동일한 UpdateSlider 사용)
        HoldingManager.Instance.OnHoldingStarted += ShowHoldingSlider;
        HoldingManager.Instance.OnHoldingProgress += UpdateSlider; // 통일!
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
            HoldingManager.Instance.OnHoldingProgress -= UpdateSlider; // 통일!
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

    // 홀딩 시작 시 슬라이더 표시 (최대 시간 설정)
    private void ShowHoldingSlider()
    {
        if (castingSlider == null) return;
        castingSlider.gameObject.SetActive(true);
        // HoldingManager에서 maxDuration을 전달받거나, 
        // 홀딩 스킬의 최대 시간을 설정
        castingSlider.maxValue = 5f; // 예: 최대 5초 홀딩
        castingSlider.value = 0f;
    }

    // 차징/홀딩 공통 슬라이더 업데이트
    private void UpdateSlider(float elapsed, float duration)
    {
        if (castingSlider == null) return;
        castingSlider.maxValue = duration;
        castingSlider.value = Mathf.Clamp(elapsed, 0, duration);
    }

    private void HideSlider()
    {
        if (castingSlider == null) return;
        castingSlider.gameObject.SetActive(false);
    }
}