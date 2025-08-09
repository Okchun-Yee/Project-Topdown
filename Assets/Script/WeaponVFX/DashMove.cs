using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashMove : MonoBehaviour
{
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public void Dash(Vector2 direction, float force, float duration)
    {
        StartCoroutine(DashRoutine(direction, force, duration));
    }
    private IEnumerator DashRoutine(Vector2 direction, float force, float duration)
    {
        rb.velocity = Vector2.zero; // 초기 속도 초기화
        rb.AddForce(direction * force, ForceMode2D.Impulse); // 대시 방향으로 힘 적용
        yield return new WaitForSeconds(duration);
        rb.velocity = Vector2.zero; // 대시 후 속도 초기화
    }
}
