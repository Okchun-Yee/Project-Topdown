using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 할버드(Halberd) 무기 클래스
/// └ BaseWeapon을 상속받아 공통 쿨다운·초기화 로직 자동 적용
/// </summary>

[RequireComponent(typeof(Animator))]
public class Sword_02 : BaseWeapon
{
    [Header("Combo Settings")]
    [SerializeField] private Transform[] comboColliders;      // 콤보용 콜라이더
    [SerializeField] private float comboMaxDelay;    // 다음 콤보 입력 허용 최대 시간(초)

    [Header("VFX Settings")]
    [SerializeField] private GameObject[] slashAnimPrefab;    // 콤보 슬래시 애니메이션 프리팹
    private Transform slashAnimSpawnPoint; // 슬래시 애니메이션 생성 위치
    
    private Animator anim;              // 무기 프리팹에 붙은 Animator
    private static readonly string[] comboTriggers = new[] {
        "Attack1", "Attack2", "Attack3"
    };
    private int currentComboIndex = 0;       // 0~4 인덱스
    private int activeColliderIndex = 0;   // 현재 활성화된 콜라이더 인덱스
    private bool isAttacking = false;        // 공격 중 플래그
    private Coroutine comboResetCoroutine;   // 콤보 초기화 코루틴 참조

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        ResetCombo();
        slashAnimSpawnPoint = GameObject.Find("Slash SpawnPoint").transform;
    }

    private void Update()
    {
        MouseFollowWithOffset();
    }

    protected override void OnAttack()
    {
        // 공격 중이면 입력 무시
        if (isAttacking) return;
        isAttacking = true;

        Debug.Log("Halberd Attack");
        int idx = currentComboIndex % comboTriggers.Length;
        Debug.Log($"animation {comboTriggers[idx]} index {idx} Cur Index {currentComboIndex}");
        anim.SetTrigger(comboTriggers[idx]);

        ActivateCollider(idx);

        // 콤보별 슬래시 애니메이션 프리팹 생성
    if (slashAnimPrefab != null && idx < slashAnimPrefab.Length && slashAnimSpawnPoint != null)
    {
        /*콤보별 슬래시 애니메이션 생성 위치는 slashAnimSpawnPoint로 설정*/
        Instantiate(slashAnimPrefab[idx], slashAnimSpawnPoint.position, slashAnimSpawnPoint.rotation);
        // Debug.Log($"Slash VFX spawned for combo {idx}");
    }

        // 기존 콤보 리셋 코루틴이 있으면 중단
        if (comboResetCoroutine != null)
            StopCoroutine(comboResetCoroutine);
        // 새 콤보 리셋 코루틴 시작
        comboResetCoroutine = StartCoroutine(ComboResetTimer());

        currentComboIndex++; // 콤보 인덱스 증가

        isAttacking = false; // 실제 게임에서는 애니메이션 이벤트에서 해제
    }

    private void ActivateCollider(int index)
    {
        for (int i = 0; i < comboColliders.Length; i++)
        {
            // 콜라이더 활성화/비활성화
            comboColliders[i].gameObject.SetActive(i == index);
            activeColliderIndex = index;
        }
    }
    private IEnumerator ComboResetTimer()
    {
        float timer = 0f;
        while (timer < comboMaxDelay)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        ResetCombo();
    }
    private void ResetCombo()
    {
        // 인덱스 초기화
        currentComboIndex = 0;
        // 모든 콜라이더 비활성화
        foreach (var col in comboColliders)
            col.gameObject.SetActive(false);
        isAttacking = false;
    }

    // 애니메이션 이벤트에서 호출 예시
    // Animation Clip에 이벤트를 걸고 해당 프레임에 이 함수가 실행되도록 합니다.
    public void OnComboHit()
    {
        // 타이밍에 맞춰 데미지 처리, 이펙트 재생 등
        isAttacking = false;       // 공격 완료 상태로 변경
        Debug.Log("Combo hit registered");
    }
    /// <summary>
    /// 마우스 위치에 따라 무기 방향 조정
    /// </summary>
    private void MouseFollowWithOffset()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position);

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;

        if (mousePos.x < playerScreenPoint.x)
        {
            transform.parent.rotation = Quaternion.Euler(0, -180, angle);
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, -180, angle);
            comboColliders[activeColliderIndex].transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            transform.parent.rotation = Quaternion.Euler(0, 0, angle);
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
            comboColliders[activeColliderIndex].transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
