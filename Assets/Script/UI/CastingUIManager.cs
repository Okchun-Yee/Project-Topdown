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
        Debug.Log("CastingUIManager: Subscribing to ChargingManager events");
        ChargingManager.Instance.OnChargingProgress += UpdateSlider;
        ChargingManager.Instance.OnChargingCompleted += HideSlider;
        ChargingManager.Instance.OnChargingCanceled += HideSlider;
    }

    private void OnDisable()
    {
        Debug.Log("CastingUIManager: Unsubscribing from ChargingManager events");
        ChargingManager.Instance.OnChargingProgress -= UpdateSlider;
        ChargingManager.Instance.OnChargingCompleted -= HideSlider;
        ChargingManager.Instance.OnChargingCanceled -= HideSlider;
    }   

    public void ShowSlider(float duration)
    {
        if (castingSlider == null) return;
        castingSlider.gameObject.SetActive(true);

        castingSlider.maxValue = duration;
        castingSlider.value = 0;

    }

    private void UpdateSlider(float elapsed, float duration)
    {
        if (castingSlider == null) return;
        castingSlider.value = Mathf.Clamp(elapsed, 0, duration);
    }

    private void HideSlider()
    {
        if (castingSlider == null) return;
        castingSlider.gameObject.SetActive(false);
    }
}