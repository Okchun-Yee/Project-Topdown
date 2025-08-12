using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChargingManager : Singleton<ChargingManager>
{
    private IEnumerator chargingSkill; // 현재 충전 중인 스킬 코루틴
    private float chargeTime = 0f; // 충전 시간
    private float chargeTimeElapsed = 0f;

    [HideInInspector]
    public bool isChargingSuccessful = false; // 차징 성공 여부

    //차징 완료 or 취소 이벤트
    public event System.Action OnChargingCompleted;
    public event System.Action OnChargingCanceled;
    public event System.Action<float, float> OnChargingProgress; // (elapsed, duration)

    //프로퍼티
    public float ChargeTimeElapsed => chargeTimeElapsed;

    public void StartCharging(float skillChargeTime)
    {
        if (chargingSkill != null)
            EndCharging(); // 중복 차징 방지

        chargeTime = skillChargeTime;
        // Start charging logic
        chargingSkill = ChargingRoutine();
        StartCoroutine(chargingSkill);
    }

    public void EndCharging()
    {
        // Stop charging logic
        if (chargingSkill != null)
        {
            StopCoroutine(chargingSkill);
            chargingSkill = null;

            isChargingSuccessful = false;
            OnChargingCanceled?.Invoke();
        }
    }
    private IEnumerator ChargingRoutine()
    {
        chargeTimeElapsed = 0f;
        isChargingSuccessful = false;

        while (chargeTimeElapsed < chargeTime)
        {
            chargeTimeElapsed += Time.deltaTime;
            OnChargingProgress?.Invoke(chargeTimeElapsed, chargeTime);
            yield return null; // Wait for the next frame
        }
        isChargingSuccessful = true;
        chargingSkill = null;

        OnChargingCompleted?.Invoke();
    }
}
