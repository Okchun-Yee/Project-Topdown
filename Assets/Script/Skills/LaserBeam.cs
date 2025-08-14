using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : BaseSkill
{
    [SerializeField] private GameObject laserPrefab;   // 레이저 애니메이션 프리팹
    [SerializeField] private Transform laserSpawnPoint; // 레이저 생성 위치
    [SerializeField] private float maxRange = 10f; // 최대 범위
    private GameObject laserInstance; // 생성된 레이저 인스턴스
    private Animator anim;
    private readonly int LASER_HASH = Animator.StringToHash("Skill");
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

            laserInstance = Instantiate(laserPrefab, laserSpawnPoint.position, Quaternion.identity);
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
            Debug.Log("LaserBeam holding ended");
            BaseSkill.IsCasting = isHolding = false;
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

    protected override void OnSkillActivated()
    {
        Debug.Log("LaserBeam Activated");
        SkillUIManager.Instance.OnSkillUsed(1); // 스킬 사용 UI 업데이트
    }

    // 홀딩 상태 확인용 프로퍼티
    public bool IsHolding => isHolding;

    public override void SubscribeSkillEvents()
    {
        if (HoldingManager.Instance != null)
        {
            HoldingManager.Instance.OnHoldingStarted += OnHoldingStarted;
            HoldingManager.Instance.OnHoldingEnded += OnHoldingEnded;
            HoldingManager.Instance.OnHoldingProgress += OnHoldingProgress;
        }
    }

    public override void UnsubscribeSkillEvents()
    {
        if (HoldingManager.Instance != null)
        {
            HoldingManager.Instance.OnHoldingStarted -= OnHoldingStarted;
            HoldingManager.Instance.OnHoldingEnded -= OnHoldingEnded;
            HoldingManager.Instance.OnHoldingProgress -= OnHoldingProgress;
        }
    }
}
