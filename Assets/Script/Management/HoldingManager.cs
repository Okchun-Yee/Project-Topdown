using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingManager : Singleton<HoldingManager>
{
    private bool isHolding = false; // 홀딩 상태
    private float holdDuration = 0f; // 홀딩 지속 시간

    // 홀딩 이벤트
    public event System.Action OnHoldingStarted;
    public event System.Action OnHoldingEnded;
    public event System.Action<float> OnHoldingProgress; // (duration)

    // 프로퍼티
    public bool IsHolding => isHolding;
    public float HoldDuration => holdDuration;

    public void StartHolding()
    {
        if (isHolding) return; // 이미 홀딩 중이면 무시

        isHolding = true;
        holdDuration = 0f;
        Debug.Log("[HoldingManager] 홀딩 시작");
        
        OnHoldingStarted?.Invoke();
        StartCoroutine(HoldingRoutine());
    }

    public void EndHolding()
    {
        if (!isHolding) return; // 홀딩 중이 아니면 무시

        isHolding = false;
        Debug.Log($"[HoldingManager] 홀딩 종료 (지속시간: {holdDuration:F2}초)");
        
        OnHoldingEnded?.Invoke();
        holdDuration = 0f;
    }

    private IEnumerator HoldingRoutine()
    {
        while (isHolding)
        {
            holdDuration += Time.deltaTime;
            OnHoldingProgress?.Invoke(holdDuration);
            yield return null; // Wait for the next frame
        }
    }
}
