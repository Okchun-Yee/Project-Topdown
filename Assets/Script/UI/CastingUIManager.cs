using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CastingUIManager : Singleton<CastingUIManager>   
{
    [SerializeField] private Slider castingSlider;
    private Coroutine subscribeCoroutine;

    protected override void Awake()
    {
        base.Awake();
        if (castingSlider != null)
            castingSlider.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        subscribeCoroutine = StartCoroutine(SubscribeWhenReady());
    }

    private void OnDisable()
    {
        if (subscribeCoroutine != null)
        {
            StopCoroutine(subscribeCoroutine);
            subscribeCoroutine = null;
        }

        UnsubscribeEvents();
    }

    private IEnumerator SubscribeWhenReady()
    {
        // Manager들이 생성될 때까지 대기
        while (ChargingManager.Instance == null || HoldingManager.Instance == null)
        {
            yield return null;
        }

        Debug.Log("CastingUIManager: Subscribing to events");
        
        // 차징 이벤트 구독
        ChargingManager.Instance.OnChargingProgress += UpdateSlider;
        ChargingManager.Instance.OnChargingCompleted += HideSlider;
        ChargingManager.Instance.OnChargingCanceled += HideSlider;
        
        // 홀딩 이벤트 구독
        HoldingManager.Instance.OnHoldingStarted += ShowHoldingSlider;
        HoldingManager.Instance.OnHoldingProgress += UpdateSlider;
        HoldingManager.Instance.OnHoldingEnded += HideSlider;
    }

    private void UnsubscribeEvents()
    {
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
            HoldingManager.Instance.OnHoldingProgress -= UpdateSlider;
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

    public void ShowHoldingSlider(float maxDuration)
    {
        if (castingSlider == null) return;
        castingSlider.gameObject.SetActive(true);
        castingSlider.maxValue = maxDuration; // 고정값이 아닌 전달받은 값 사용
        castingSlider.value = 0f;
        Debug.Log($"ShowHoldingSlider - maxDuration: {maxDuration}");
    }

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