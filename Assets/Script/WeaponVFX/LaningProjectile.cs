using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaningProjectile : MonoBehaviour
{
    [SerializeField] float totalDuration = 5.2f;
    [SerializeField] float initialDuration = 0.2f; // 첫 0.2초 동안 5의 데미지
    private SpriteFade spriteFader; 
    // private CapsuleCollider2D capsuleCollider;
    private float _spawnTime;
    private void Awake()
    {
        spriteFader = GetComponent<SpriteFade>();
        _spawnTime = Time.time;
    }
    private void Start()
    {
        StartCoroutine(spriteFader.SlowFadeRoutine());
        StartCoroutine(DamageRoutine());
        Invoke("DisableCollider", totalDuration + 0.1f); // 1초 후에 콜라이더 비활성화
    }
    private IEnumerator DamageRoutine()
    {
        // 첫 0.2초 동안 5의 데미지
        float tickDuration = 1f;
        CapsuleCollider2D col = GetComponent<CapsuleCollider2D>();
        col.enabled = true;

        // 첫 0.2초 동안 5의 데미지
        float timer = 0f;
        while (timer < initialDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // 이후 5초 동안 1초마다 1의 데미지
        while (timer < totalDuration)
        {
            yield return new WaitForSeconds(tickDuration);
            timer += tickDuration;
        }
        col.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        EnemyHealth enemyHealth = collider.GetComponent<EnemyHealth>();
        if (enemyHealth == null) return;

        // 첫 0.2초 동안은 5의 데미지, 이후에는 1의 데미지
        float timeSinceStart = Time.time - _spawnTime;
        if (timeSinceStart < initialDuration)
        {
            enemyHealth.TakeDamage(5);
        }
        else if (timeSinceStart < totalDuration)
        {
            enemyHealth.TakeDamage(1);
        }
    }
    private void DisableCollider()
    {
        GetComponent<CapsuleCollider2D>().enabled = false;
    }
}
