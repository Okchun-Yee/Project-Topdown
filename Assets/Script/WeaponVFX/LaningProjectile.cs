using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaningProjectile : BaseVFX
{
    [Header("지속 데미지 설정")]
    [SerializeField] private float tickInterval = 1f; // 데미지 간격 (1초마다)
    [SerializeField] private float totalDuration = 5.2f; // 전체 지속 시간
    [SerializeField] private float damageRadius = 1f; // 데미지 범위
    [SerializeField] private LayerMask enemyLayers = -1; // 적 레이어

    private SpriteFade spriteFader; 
    private CapsuleCollider2D capsuleCollider;

    protected override void Awake()
    {
        base.Awake();
        spriteFader = GetComponent<SpriteFade>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    protected override void Start()
    {
        base.Start();
        
        // 시각적 효과 시작
        if (spriteFader != null)
        {
            StartCoroutine(spriteFader.SlowFadeRoutine());
        }
        
        // 충돌체 활성화 및 정리 예약
        capsuleCollider.enabled = true;
        Invoke(nameof(DisableCollider), totalDuration + 0.1f);
    }

    protected override void OnVFXInitialized()
    {
        // === 지속 데미지 시작 ===
        if (damageSource != null)
        {
            damageSource.StartAreaContinuousDamage(
                damagePerTick: assignedDamage,
                interval: tickInterval,
                duration: totalDuration,
                radius: damageRadius,
                targetLayers: enemyLayers
            );    
        }
    }

    private void DisableCollider()
    {
        if (capsuleCollider != null)
        {
            capsuleCollider.enabled = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // 데미지 범위 시각화
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }

    private void OnDestroy()
    {
        // 오브젝트 파괴 시 지속 데미지 정리
        if (damageSource != null)
        {
            damageSource.StopContinuousDamage();
        }
    }
}
