using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FireBall : BaseSkill
{
    [SerializeField] private GameObject fireBallPrefab; // 화염구 프리팹
    [SerializeField] private Transform fireBallSpawnPoint; // 화염구 생성 위치
    [SerializeField] private float projectileRange = 10f; // 화염구의 사거리
    readonly int FIRE_HASH = Animator.StringToHash("Fire_Perform");
    private Animator anim; // 무기 프리팹에 붙은 Animator

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    protected override void OnSkillActivated()
    {
        Debug.Log("FireBall Activated");
        
        anim.SetTrigger(FIRE_HASH); // 애니메이션 트리거 설정
        SkillUIManager.Instance.OnSkillUsed(skillIndex); // 스킬 사용 UI 업데이트
        StartCoroutine(PerformFireBall());
    }

    private IEnumerator PerformFireBall()
    {
        // 마우스 위치를 월드 좌표로 변환하여 방향 및 회전 계산
        Camera mainCamera = Camera.main;
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 dir = ((Vector2)mouseWorldPos - (Vector2)fireBallSpawnPoint.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion fireBallRotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // 화염구 프리팹 생성 (마우스 방향)
        GameObject fireBallInstance = Instantiate(fireBallPrefab, fireBallSpawnPoint.position, fireBallRotation);
        fireBallInstance.GetComponent<Projectile>().UpdateProjectilRange(projectileRange);
        fireBallInstance.GetComponent<Projectile>().Initialize(CalculateFinalDamage()); // 데미지 설정

        yield return null;
    }
}
