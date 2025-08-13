using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PillarOfFlame : BaseSkill
{
    [SerializeField] private GameObject pillarPrefab;   //불기둥 애니메이션 프리팹
    [SerializeField] private float maxRange = 7f; // 최대 범위
    [SerializeField] private float minRange = 3f; // 최소 범위
    private GameObject pillarInstance; // 생성된 불기둥 인스턴스
    private Animator anim;
    private readonly int PILLAR_HASH = Animator.StringToHash("Fire");
    private bool sliderShown = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        ChargingManager.Instance.OnChargingProgress += OnChargingProgress;
        ChargingManager.Instance.OnChargingCompleted += OnChargingCompleted;
        ChargingManager.Instance.OnChargingCanceled += OnChargingCanceled;
    }
    private void OnDisable()
    {
        ChargingManager.Instance.OnChargingProgress -= OnChargingProgress;
        ChargingManager.Instance.OnChargingCompleted -= OnChargingCompleted;
        ChargingManager.Instance.OnChargingCanceled -= OnChargingCanceled;
    }
    protected override void OnChargingCompleted()
    {
        anim.SetBool(PILLAR_HASH, false); // 애니메이션 트리거 설정
        sliderShown = false;
        // 차징 취소 시 프리팹 삭제
        if (pillarInstance != null)
        {
            Destroy(pillarInstance);
            pillarInstance = null;
        }
    }
    protected override void OnChargingCanceled()
    {
        BaseSkill.IsCasting = false; // 스킬 사용 완료 상태로 변경
        anim.SetBool(PILLAR_HASH, false); // 애니메이션 트리거 설정
        sliderShown = false;
        // 차징 취소 시 프리팹 삭제
        if (pillarInstance != null)
        {
            Destroy(pillarInstance);
            pillarInstance = null;
        }
    }
    // 차징 중(프로그레스) 이벤트에서 프리팹 최초 1회 생성
    protected override void OnChargingProgress(float elapsed, float duration)
    {
        if (isOnCooldown) return;

        anim.SetBool(PILLAR_HASH, true);

        if (!sliderShown)
        {
            CastingUIManager.Instance.ShowSlider(SkillInfo.CastingTime);
            sliderShown = true;
        }

        if (pillarInstance == null)
        {
            Vector3 spawnPos = GetMouseWorldPosition();
            pillarInstance = Instantiate(pillarPrefab, spawnPos, Quaternion.identity);
        }
    }
    protected override void OnSkillActivated()
    {
        Debug.Log("PillarOfFlame Activated");
        // 스킬 사용 UI 업데이트
        SkillUIManager.Instance.OnSkillUsed(0); // 예시로 0번 스킬로 업데이트
    }

    // 마우스 위치를 기준으로 프리팹 생성 위치를 반환하는 함수
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;
        Vector3 playerPos = PlayerController.Instance.transform.position;
        Vector3 dir = (mouseWorldPos - playerPos).normalized;
        float dist = Vector3.Distance(playerPos, mouseWorldPos);
        float clampedDist = Mathf.Clamp(dist, minRange, maxRange);

        return playerPos + dir * clampedDist;
    }
}
