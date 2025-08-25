using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Projectile : BaseVFX
{
    [SerializeField] private float moveSpeed = 22f;
    [SerializeField] private GameObject particleOnHitPrefabVFX;
    [SerializeField] private bool isEnemyProjectile = false;
    [SerializeField] private float projectileRange = 10f;
    private Vector3 startPosition;
    /// <summary>
    /// 적과 충돌 시 호출될 이벤트
    /// </summary>
    public System.Action<Vector3, EnemyHealth> OnEnemyHit;
    
    /// <summary>
    /// 장애물과 충돌 시 호출될 이벤트  
    /// </summary>
    public System.Action<Vector3> OnObstacleHit;

    protected override void Start()
    {
        startPosition = transform.position;
    }
    protected override void OnVFXInitialized()
    {
        // VFX 초기화 로직
        Debug.Log($"Projectile [{gameObject.name}]: Initialized with damage {assignedDamage}");
    }
    private void Update()
    {
        MoveProjectile();
        DetectFireDistance();
    }
    public void UpdateProjectilRange(float projectileRange)
    {
        this.projectileRange = projectileRange;
    }
    public void UpdateMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        Indestructible indestructible = collision.gameObject.GetComponent<Indestructible>();
        PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();

        if (!collision.isTrigger && (enemyHealth || indestructible || player))
        {
            if ((player && isEnemyProjectile) || (enemyHealth && !isEnemyProjectile))
            {
                if (player && isEnemyProjectile)
                {
                    // 적 발사체 → 플레이어 (기존 방식 유지)
                    player?.TakeDamage(1, transform);
                }
                else if (enemyHealth && !isEnemyProjectile)
                {
                    // 플레이어 발사체 → 적 (DamageSource 사용!)
                    if (isInitialized)
                    {
                        // 스킬용: DamageSource로 계산된 데미지 전달
                        damageSource.DealInstantDamage(assignedDamage, enemyHealth);
                        Debug.Log($"Projectile: DamageSource dealt {assignedDamage} damage to {enemyHealth.name}");
                    }
                    else
                    {
                        // 기본 무기용: DamageSource로 기본 데미지 1 전달
                        damageSource.DealInstantDamage(1f, enemyHealth);
                        Debug.Log($"Projectile: DamageSource dealt 1 damage to {enemyHealth.name} (default)");
                    }
                    // === 스킬별 추가 효과 이벤트 호출 ===
                    OnEnemyHit?.Invoke(transform.position, enemyHealth);
                }
                else if (enemyHealth && !isEnemyProjectile)
                {
                    // DamageSource에서 현재 설정된 데미지 우선 사용
                    float currentDamage = damageSource.DamageAmount;
                    
                    if (currentDamage > 1f) // 무기에서 설정된 데미지가 있으면
                    {
                        damageSource.DealInstantDamage(currentDamage, enemyHealth);
                        Debug.Log($"Projectile: Used weapon damage {currentDamage}");
                    }
                    else if (isInitialized) // 스킬에서 초기화된 데미지
                    {
                        damageSource.DealInstantDamage(assignedDamage, enemyHealth);
                        Debug.Log($"Projectile: Used skill damage {assignedDamage}");
                    }
                    else // 기본값
                    {
                        damageSource.DealInstantDamage(1f, enemyHealth);
                        Debug.Log($"Projectile: Used default damage 1");
                    }
                    
                    // === 스킬별 추가 효과 이벤트 호출 ===
                    OnEnemyHit?.Invoke(transform.position, enemyHealth);
                }
                
                Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
                Destroy(gameObject);
            }
            else if (!collision.isTrigger && indestructible)
            {
                // === 장애물 충돌 이벤트 호출 ===
                OnObstacleHit?.Invoke(transform.position);

                Instantiate(particleOnHitPrefabVFX, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
    private void DetectFireDistance()
    {
        if (Vector3.Distance(transform.position, startPosition) > projectileRange)
        {
            Destroy(gameObject);
        }
    }
    private void MoveProjectile()
    {
        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
    }
    public void DeleteProjectile()
    {
        if(isEnemyProjectile)
        {
            Destroy(gameObject);
        }
    }
}
