using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleAttackCollider : MonoBehaviour
{
    private DamageSource damageSource;
    private HashSet<EnemyHealth> hitEnemies = new HashSet<EnemyHealth>(); // 중복 방지

    private void Awake()
    {
        damageSource = GetComponent<DamageSource>();
        if (damageSource == null)
        {
            damageSource = gameObject.AddComponent<DamageSource>();
        }
    }

    private void OnEnable()
    {
        // 콜라이더 활성화 시 히트 목록 초기화
        hitEnemies.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
        if (enemyHealth == null) return;

        // 이미 맞은 적은 다시 맞지 않도록 방지
        if (hitEnemies.Contains(enemyHealth)) return;

        hitEnemies.Add(enemyHealth);

        // 데미지 처리
        if (damageSource != null)
        {
            damageSource.DealInstantDamage(damageSource.DamageAmount, enemyHealth);
        }
    }
}
