using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveVFX : BaseVFX
{
    [Header("폭발 설정")]
    [SerializeField] private float explosionRadius = 1f;

    protected override void OnVFXInitialized()
    {
        // DamageSource의 범위 데미지 메서드 직접 호출
        damageSource.DealAreaDamage(assignedDamage, explosionRadius);
        
        Debug.Log($"ExplosiveVFX: Exploded with damage {assignedDamage} in radius {explosionRadius}");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
