using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : BaseSkill
{
    [SerializeField] private GameObject laserPrefab;   // 레이저 애니메이션 프리팹
    [SerializeField] private Transform laserSpawnPoint; // 레이저 생성 위치
    [SerializeField] private float maxRange = 4f; // 최대 범위
    [SerializeField] private float damageInterval = 0.2f;
    private GameObject laserInstance; // 생성된 레이저 인스턴스
    private Animator anim;
    private readonly int LASER_HASH = Animator.StringToHash("Fire_Holding");
    private bool isHolding = false; // 홀딩 상태 플래그

    // 레이캐스트 관련
    private float lastDamageTime = 0f;
   
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // 홀딩 시작
    protected override void OnHoldingStarted(float maxDuration)
    {
        if (!isHolding)
        {
            Debug.Log("LaserBeam holding started");
            isHolding = true;
            anim.SetBool(LASER_HASH, true);

            // 레이저 생성
            laserInstance = Instantiate(laserPrefab, laserSpawnPoint.position, Quaternion.identity);
            laserInstance.transform.parent = laserSpawnPoint; // 부모 설정으로 위치 추적
            laserInstance.transform.localRotation = Quaternion.identity; // 로컬 회전 초기화

            // HoldingProjectile의 마우스 추적 비활성화
            HoldingProjectile holdingComp = laserInstance.GetComponent<HoldingProjectile>();
            if (holdingComp != null)
            {
                holdingComp.SetMouseTracking(false); // 마우스 추적 끄기
            }

            // 데미지 및 범위 설정
            float finalDamage = CalculateFinalDamage();
            laserInstance.GetComponent<DamageSource>().SetDamage(finalDamage);
            laserInstance.GetComponent<HoldingProjectile>().UpdateLaserRange(maxRange);

            OnSkill();
        }
    }

    // 홀딩 진행 중
    protected override void OnHoldingProgress(float elapsed, float duration)
    {
        // 홀딩 중 지속적으로 레이캐스트 발사
        if (Time.time >= lastDamageTime + damageInterval)
        {
            CastLaserRay();
            lastDamageTime = Time.time;
        }
    }

    // 홀딩 종료
    protected override void OnHoldingEnded()
    {
        if (isHolding)
        {
            BaseSkill.IsCasting = false;
            isHolding = false;

            anim.SetBool(LASER_HASH, false);

            laserInstance.GetComponent<HoldingProjectile>().SetHoldingState(false); // 레이저 종료 처리

            // 레이저 프리팹 삭제
            if (laserInstance != null)
            {
                Destroy(laserInstance);
                laserInstance = null;
            }
        }
    }

    private void CastLaserRay()
    {
        if (laserInstance != null)
        {
            Vector2 origin = laserSpawnPoint.position;
            Vector2 direction = laserSpawnPoint.right; // 또는 마우스 방향
            float distance = maxRange;
            
            DamageSource damageSource = laserInstance.GetComponent<DamageSource>();
            RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, distance);
            
            // Debug.DrawRay로 시각화 (Game 뷰에서도 보임)
            Debug.DrawRay(origin, direction * distance, Color.red, damageInterval);
            
            foreach (var hit in hits)
            {
                EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    float finalDamage = CalculateFinalDamage();
                    damageSource.DealInstantDamage(finalDamage, enemy);
                    
                    // 히트 지점 시각화
                    Debug.DrawLine(origin, hit.point, Color.yellow, damageInterval);
                }
            }
        }
    }

    // 기즈모로 레이캐스트 시각화
    private void OnDrawGizmos()
    {
        if (laserSpawnPoint != null && isHolding && Application.isPlaying)
        {
            Vector3 origin = laserSpawnPoint.position;
            Vector3 direction = laserSpawnPoint.right;
            
            // 레이캐스트 라인 (빨간색)
            Gizmos.color = Color.red;
            Gizmos.DrawRay(origin, direction * maxRange);
        }
    }

    protected override void OnSkillActivated()
    {
        Debug.Log("LaserBeam Activated");
        SkillUIManager.Instance.OnSkillUsed(skillIndex); // 스킬 사용 UI 업데이트
    }

    public override void SubscribeSkillEvents()
    {
        if (HoldingManager.Instance != null)
        {
            HoldingManager.Instance.OnHoldingStarted += OnHoldingStarted;
            HoldingManager.Instance.OnHoldingEnded += OnHoldingEnded;
            HoldingManager.Instance.OnHoldingProgress += OnHoldingProgress;
            HoldingManager.Instance.OnHoldingCanceled += OnHoldingEnded;
        }
    }

    public override void UnsubscribeSkillEvents()
    {
        if (HoldingManager.Instance != null)
        {
            HoldingManager.Instance.OnHoldingStarted -= OnHoldingStarted;
            HoldingManager.Instance.OnHoldingEnded -= OnHoldingEnded;
            HoldingManager.Instance.OnHoldingProgress -= OnHoldingProgress;
            HoldingManager.Instance.OnHoldingCanceled -= OnHoldingEnded;
        }
    }

    private void OnDestroy()
    {
        UnsubscribeSkillEvents();
        
        if (isHolding)
        {
            isHolding = false;
            if (laserInstance != null)
            {
                Destroy(laserInstance);
            }
        }
    }
}
