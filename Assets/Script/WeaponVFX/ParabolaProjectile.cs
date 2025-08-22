using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaProjectile : BaseVFX
{
    [SerializeField] private float duration = 1f;
    [SerializeField] private float heightY = 3f;
    [SerializeField] private float Range = 5f; // 최대 범위
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private GameObject impactVFXPrefab;

    protected override void Start()
    {
        base.Start();
        SetTargetByMousePosition(Range);
    }
    protected override void OnVFXInitialized()
    {
        Debug.Log($"ParabolaProjectile: Initialized with damage {assignedDamage}");
        // 초기화 시 특별한 동작 없음 (포물선 시작은 Start()에서)
    }

    // 마우스 위치를 기준으로, 스킬 범위 내면 그대로, 범위 밖이면 최대 범위로 타겟 지정
    public void SetTargetByMousePosition(float range)
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;
        Vector3 dir = mouseWorld - transform.position;
        float dist = dir.magnitude;
        Vector3 target;
        if (dist > range)
        {
            dir.Normalize();
            target = transform.position + dir * range;
        }
        else
        {
            target = mouseWorld;
        }
        StartCoroutine(ProjectileCurveRoutine(transform.position, target));
    }


    //포물선 궤적 계산 코루틴
    private IEnumerator ProjectileCurveRoutine(Vector3 startPos, Vector3 endPos)
    {
        float timePassed = 0f;
        float dist = Vector2.Distance(startPos, endPos);
        float t = Mathf.Clamp01(dist / Range);

        float startAngle = Mathf.Lerp(30f, 15f, t);
        float endAngle = Mathf.Lerp(-30f, -15f, t);

        float baseAngle = 0f;
        if (ActiveWeapon.Instance != null)
            baseAngle = ActiveWeapon.Instance.transform.eulerAngles.z;
        baseAngle = (baseAngle + 360f) % 360f;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;
            float heightT = animCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0, heightY, heightT);

            // 포물선 위치 계산
            Vector2 flatPos = Vector2.Lerp(startPos, endPos, linearT);
            Vector3 curvePos = flatPos + new Vector2(0, height);
            transform.position = curvePos;

            // 각도 보정
            float angle;
            if (linearT < 0.5f)
            {
                angle = Mathf.Lerp(startAngle, 0f, linearT * 2f);
            }
            else { angle = Mathf.Lerp(0f, endAngle, (linearT - 0.5f) * 2f); }
            // 무기 회전값을 더해서 적용
            if (baseAngle > 90f && baseAngle < 270f) { angle = 180f - angle; } // 왼쪽 방향일 때 각도 보정
            transform.rotation = Quaternion.Euler(0, 0, angle);

            yield return null;
        }
        Impact();
        Destroy(gameObject);
    }

    private void Impact()
    {
        if (impactVFXPrefab != null)
        {
            GameObject impactVFX = Instantiate(impactVFXPrefab, transform.position, Quaternion.identity);
            impactVFX.GetComponent<LaningProjectile>().Initialize(assignedDamage * 0.2f);
        }
    }
}
