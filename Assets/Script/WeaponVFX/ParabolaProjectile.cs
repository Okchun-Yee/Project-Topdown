using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaProjectile : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    [SerializeField] private float heightY = 3f;
    [SerializeField] private float Range = 5f; // 최대 범위
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private GameObject impactVFXPrefab;
    private Vector3 targetPosition;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetTargetByMousePosition(Range);
    }

    // 마우스 위치를 기준으로, 스킬 범위 내면 그대로, 범위 밖이면 원 위의 점으로 타겟 지정
    public void SetTargetByMousePosition(float range)
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;
        Vector3 dir = mouseWorld - transform.position;
        float dist = dir.magnitude;
        // 범위 밖이면 최대 범위 방향으로 발사
        if (dist > range)
        {
            dir.Normalize();
            targetPosition = transform.position + dir * range;
        }
        else
        {
            targetPosition = mouseWorld;
        }
        StartCoroutine(ProjectileCurveRoutine(transform.position, targetPosition));
    }


    private IEnumerator ProjectileCurveRoutine(Vector3 startPos, Vector3 endPos)
    {
        float timePassed = 0f;
        float dist = Vector2.Distance(startPos, endPos);
        float t = Mathf.Clamp01(dist / Range);

        // 각도 계산: t=1(최대) → 45~0~-45, t=0(최소) → 90~0~-90
        float startAngle = Mathf.Lerp(45f, 15f, t);
        float endAngle = Mathf.Lerp(-45f, -15f, t);

        // ActiveWeapon의 현재 회전(Z축) 각도
        float baseAngle = 0f;
        if (ActiveWeapon.Instance != null)
            baseAngle = ActiveWeapon.Instance.transform.eulerAngles.z;
        baseAngle = (baseAngle + 360f) % 360f; // 0~360으로 변환

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
            if (baseAngle > 90f && baseAngle < 270f) { angle += 180f; } // 왼쪽 방향일 때 각도 보정
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // 왼쪽(180도 부근)일 때만 flipX 적용
            if (baseAngle > 90f && baseAngle < 270f) { spriteRenderer.flipX = true; }
            else { spriteRenderer.flipX = false; }

            yield return null;
        }

        if (impactVFXPrefab != null)
            Instantiate(impactVFXPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
