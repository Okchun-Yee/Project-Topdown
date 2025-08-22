using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveVFX : BaseVFX
{
    [Header("Explosion Radius Settings")]
    [SerializeField] private float explosionRadius = 1f;

    protected override void OnVFXInitialized()
    {
        // DamageSource의 범위 데미지 메서드 직접 호출
        damageSource.DealAreaDamage(assignedDamage, explosionRadius);
        Debug.Log($"ExplosiveVFX [{gameObject.name}]: Dealt area damage {assignedDamage} in radius {explosionRadius}");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
