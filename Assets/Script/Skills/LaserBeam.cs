using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : BaseSkill
{
    [SerializeField] private GameObject laserPrefab;   // 레이저 애니메이션 프리팹
    [SerializeField] private Transform laserSpawnPoint; // 레이저 생성 위치
    [SerializeField] private float maxRange = 4f; // 최대 범위
    private GameObject laserInstance; // 생성된 레이저 인스턴스
    private Animator anim;
    private readonly int LASER_HASH = Animator.StringToHash("Fire_Holding");
    private bool isHolding = false; // 홀딩 상태 플래그

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

            // 마우스 방향으로 회전값 계산
            laserInstance = Instantiate(laserPrefab, laserSpawnPoint.position, Quaternion.identity);
            laserInstance.transform.parent = this.transform; // 부모 설정으로 위치 추적
    
            laserInstance.GetComponent<HoldingProjectile>().UpdateLaserRange(maxRange);
            OnSkill();
        }
    }

    // 홀딩 진행 중
    protected override void OnHoldingProgress(float elapsed, float duration)
    {
        // 홀딩 중 지속적인 처리가 필요하면 여기에 작성

        // 예: 데미지 증가, 이펙트 강화 등
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

    // 홀딩 시간이 최대치에 도달했을 때 (강제 종료)
    protected override void OnHoldingCanceled()
    {
        if (isHolding)
        {
            Debug.Log("LaserBeam holding time reached maximum - forced end");
            
            BaseSkill.IsCasting = false;
            isHolding = false;

            anim.SetBool(LASER_HASH, false);

            // 최대 홀딩 시간 도달 시 특별한 효과 추가 가능
            // 예: 더 강한 데미지, 특별한 이펙트 등
            if (laserInstance != null)
            {
                laserInstance.GetComponent<HoldingProjectile>().SetHoldingState(false);
                
                // 여기에 최대 홀딩 시간 도달 시의 특별한 로직 추가 가능
                // 예: 폭발 효과, 추가 데미지 등
                
                Destroy(laserInstance);
                laserInstance = null;
            }
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
            HoldingManager.Instance.OnHoldingCanceled += OnHoldingCanceled;
        }
    }

    public override void UnsubscribeSkillEvents()
    {
        if (HoldingManager.Instance != null)
        {
            HoldingManager.Instance.OnHoldingStarted -= OnHoldingStarted;
            HoldingManager.Instance.OnHoldingEnded -= OnHoldingEnded;
            HoldingManager.Instance.OnHoldingProgress -= OnHoldingProgress;
            HoldingManager.Instance.OnHoldingCanceled -= OnHoldingCanceled;
        }
    }
}
