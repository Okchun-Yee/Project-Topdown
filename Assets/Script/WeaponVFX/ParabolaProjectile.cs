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

    private void Start()
    {
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
        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;
            float heightT = animCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0, heightY, heightT);

            transform.position = Vector2.Lerp(startPos, endPos, linearT) + new Vector2(0, height);
            yield return null;
        }

        if (impactVFXPrefab != null)
            Instantiate(impactVFXPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
