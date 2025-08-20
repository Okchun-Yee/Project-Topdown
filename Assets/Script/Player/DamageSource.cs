using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSource : MonoBehaviour
{
    [SerializeField] private float damageAmount = 1f;

    public float DamageAmount
    {
        get => damageAmount;
        set => damageAmount = value;
    }
    // 초기화 메서드
    //스킬에서 VFX 생성 직후 데미지 설정
    public void Initialize(float damage)
    {
        SetDamage(damage);
    }

    // 외부에서 데미지 설정하는 메서드들
    public void SetDamage(float damage)
    {
        damageAmount = damage;
    }

    // 즉시 데미지 (특정 대상)
    // 단순히 적에게 데미지를 주는 방식
    public void DealInstantDamage(float damage, EnemyHealth target)
    {
        target?.TakeDamage(damage);
    }

    // 지속 데미지 (DoT)
    // 코루틴을 사용하여 지속적으로 데미지를 주는 방식
    public void StartContinuousDamage(float damagePerTick, float interval, float duration, EnemyHealth target)
    {
        StartCoroutine(ContinuousDamageCoroutine(damagePerTick, interval, duration, target));
    }

    // 범위 데미지 (AoE)
    // 특정 범위 내의 적들에게 데미지를 주는 방식
    public void DealAreaDamage(float damage, float radius, Vector3? center = null)
    {
        Vector3 damageCenter = center ?? transform.position;
        Collider2D[] hits = Physics2D.OverlapCircleAll(damageCenter, radius);

        foreach (var hit in hits)
        {
            EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                DealInstantDamage(damage, enemy);
            }
        }
    }

    // 지속 데미지 코루틴
    private IEnumerator ContinuousDamageCoroutine(float damagePerTick, float interval, float duration, EnemyHealth target)
    {
        float elapsed = 0f;
        while (elapsed < duration && target != null)
        {
            DealInstantDamage(damagePerTick, target);
            yield return new WaitForSeconds(interval);
            elapsed += interval;
        }
    }

    // 기존 트리거 시스템 유지 (하위 호환성)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            DealInstantDamage(damageAmount, enemyHealth);
        }
        
        if (collision.gameObject.GetComponent<Projectile>())
        {
            collision.gameObject.GetComponent<Projectile>().DeleteProjectile();
        }
    }
}
