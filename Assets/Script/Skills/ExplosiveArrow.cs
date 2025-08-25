using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveArrow : BaseSkill
{
    [SerializeField] private GameObject arrowPrefab; // 폭발 화살 애니메이션 프리팹
    [SerializeField] private GameObject explosiveVFXPrefab; // 폭발 VFX 프리팹 (추가 필요)
    [SerializeField] private Transform arrowspawnPoint; // 화살 생성 위치
    [SerializeField] private float projectileRange = 10f; // 화살의 사거리
    
    private Animator anim; // 무기 프리팹에 붙은 Animator
    readonly int FIRE_HASH = Animator.StringToHash("Fire");

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    protected override void OnSkillActivated()
    {
        Debug.Log("ExplosiveArrow Activated");
        anim.SetTrigger(FIRE_HASH);
        SkillUIManager.Instance.OnSkillUsed(skillIndex);

        // === 화살 생성 및 설정 ===
        GameObject newArrow = Instantiate(arrowPrefab, arrowspawnPoint.position, ActiveWeapon.Instance.transform.rotation);
        Projectile projectile = newArrow.GetComponent<Projectile>();
        
        // 사거리 설정
        projectile.UpdateProjectilRange(projectileRange);
        
        projectile.Initialize(GetWeaponDamage());   //Initialize로 데미지 설정, 폭발 화살의 화살은 무기 기본 데미지
        
        // === 이벤트 구독 방식 추가 ===
        projectile.OnEnemyHit += OnArrowHitEnemy;
        projectile.OnObstacleHit += OnArrowHitObstacle;
    }

    /// <summary>
    /// 화살이 적과 충돌했을 때 폭발 생성
    /// </summary>
    private void OnArrowHitEnemy(Vector3 hitPosition, EnemyHealth enemy)
    {
        CreateExplosion(hitPosition);
    }

    /// <summary>
    /// 화살이 장애물과 충돌했을 때 폭발 생성
    /// </summary>
    private void OnArrowHitObstacle(Vector3 hitPosition)
    {
        CreateExplosion(hitPosition);
    }

    /// <summary>
    /// 폭발 VFX 생성
    /// </summary>
    private void CreateExplosion(Vector3 position)
    {
        if (explosiveVFXPrefab != null)
        {
            GameObject explosionObj = Instantiate(explosiveVFXPrefab, position, Quaternion.identity);
            ExplosiveVFX explosion = explosionObj.GetComponent<ExplosiveVFX>();

            float explosionDamage = CalculateFinalDamage(); // 폭발 데미지 (무기 공격력 * 스킬 계수)
            explosion?.Initialize(explosionDamage);
        }
    }
}
