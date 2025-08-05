using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveVFX : MonoBehaviour
{
    [SerializeField] private float explosionRadius = 2f;
    [SerializeField] private int damageAmount = 1;

    private void Start()
    {
        Explode();
    }

    private void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyHealth enemy = hit.GetComponent<EnemyHealth>();
                enemy?.TakeDamage(damageAmount);
            }
            else if (hit.CompareTag("Player"))
            {
                PlayerHealth player = hit.GetComponent<PlayerHealth>();
                player?.TakeDamage(damageAmount, transform);
            }
            // 필요시 다른 Tag도 추가
        }
        // 폭발 이펙트 후 오브젝트 제거
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
