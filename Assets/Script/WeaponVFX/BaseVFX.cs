using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseVFX : MonoBehaviour
{
    [Header("Base VFX Settings")]
    [SerializeField] protected float lifetime = 2f; // VFX 지속 시간
    [SerializeField] protected bool autoDestroy = true; // VFX 종료 시 자동 파괴 여부

    protected DamageSource damageSource;    // VFX의 DamageSource 컴포넌트
    protected float assignedDamage;    // VFX에 할당된 피해량 = 최종 데미지
    protected bool isInitialized = false;   // VFX 초기화 여부

    /// <summary>
    /// 프로퍼티
    /// </summary>
    public bool IsInitialized => isInitialized; // 초기화 여부 검사
    public float GetAssignedDamage() => assignedDamage; // 현재 설정한 데미지 반환

    protected virtual void Awake()
    {
        damageSource = GetComponent<DamageSource>();    // VFX의 DamageSource 컴포넌트
    }
    /// <summary>
    /// 생명주기 관리
    /// </summary>
    protected virtual void Start()
    {
        // Initialize가 호출되지 않은 경우 기본값으로 초기화
        if (!isInitialized)
        {
            Debug.LogWarning($"BaseVFX [{gameObject.name}]: Not initialized! Using default damage 1.");
            Initialize(1f); // 기본 데미지 1로 초기화
        }
        if (autoDestroy && lifetime > 0f)
        {
            Destroy(gameObject, lifetime); // VFX가 자동으로 파괴되도록 설정
        }
    }
    /// <summary>
    /// 스킬에서 계산된 데미지로 VFX 초기화
    /// </summary>
    public virtual void Initialize(float damage)
    {
        assignedDamage = damage;
        isInitialized = true;

        damageSource?.SetDamage(assignedDamage); // DamageSource에 데미지 설정 (즉시 데미지 용도)

        Debug.Log($"BaseVFX [{gameObject.name}]: Initialized with damage {damage}");

        // 각 VFX별 초기화 로직 실행
        OnVFXInitialized();
    }
    /// <summary>
    /// 각 VFX에서 구현할 초기화 로직 - DamageSource 설정
    /// </summary>
    protected abstract void OnVFXInitialized();

    /// <summary>
    /// VFX 즉시 종료
    /// </summary>
    public virtual void OnVFXDestroyed()
    {
        if (autoDestroy)
        {
            Destroy(gameObject); // VFX 즉시 파괴
        }
        else
        {
            gameObject.SetActive(false); // VFX 비활성화
        }
    }
}
