using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 할버드(Halberd) 무기 클래스
/// └ BaseWeapon을 상속받아 공통 쿨다운·초기화 로직 자동 적용
/// </summary>

[RequireComponent(typeof(Animator))]
public class Halberd : BaseWeapon
{
    [Header("Combo Settings")]
    [SerializeField] private Transform[] comboColliders;      // 5단계 콤보용 콜라이더
    [SerializeField] private float comboMaxDelay = 0.8f;     // 다음 콤보 입력 허용 최대 시간(초)

    [Header("VFX Settings")]
    [SerializeField] private GameObject[] slashAnimPrefab;    // 슬래시 애니메이션 프리팹 5단계
    private Transform slashAnimSpawnPoint; // 슬래시 애니메이션 생성 위치
    
    private Animator anim;              // 무기 프리팹에 붙은 Animator
    private static readonly string[] comboTriggers = new[] {
        "Attack1", "Attack2", "Attack3"
    };
    private int currentComboIndex = 0;       // 0~4 인덱스
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
        if (isAttacking) return;
        isAttacking = true;

        Debug.Log("Halberd Attack");
        // 1) 애니메이션 트리거
        int idx = currentComboIndex % comboTriggers.Length;
        anim.SetTrigger(comboTriggers[idx]);

        // 2) 해당 콤보 콜라이더만 활성화
        ActivateCollider(idx);
        // slashAnimPrefab[idx] = Instantiate(slashAnimPrefab[idx], slashAnimSpawnPoint.position, Quaternion.identity);
        // slashAnimPrefab[idx].transform.parent = this.transform.parent;

        // 3) 콤보 타이머 시작
        if (comboResetCoroutine != null)
            StopCoroutine(comboResetCoroutine);
        comboResetCoroutine = StartCoroutine(ComboResetTimer());

        // 4) 인덱스 증가
        currentComboIndex++;
    }

    private void ActivateCollider(int index)
    {
        for (int i = 0; i < comboColliders.Length; i++)
        {
            Debug.Log($"Activating collider {i} for combo index {index}");
            comboColliders[i].gameObject.SetActive(i == index);
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
            comboColliders[currentComboIndex].transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            transform.parent.rotation = Quaternion.Euler(0, 0, angle);
            ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, angle);
            comboColliders[currentComboIndex].transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
