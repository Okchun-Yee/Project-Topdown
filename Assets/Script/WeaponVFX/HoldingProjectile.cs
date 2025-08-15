using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingProjectile : MonoBehaviour
{
    private bool isHolding = false;
    private float laserRange;
    private SpriteRenderer spriteRenderer;
    private CapsuleCollider2D capsuleCollider2D;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }
    
    private void Start()
    {
        LaserFaceMouse();
    }
    
    // Update - 홀딩 중 지속적으로 마우스 방향 추적
    private void Update()
    {
        if (isHolding)
        {
            LaserFaceMouse();
        }
    }
    
    public void UpdateLaserRange(float range)
    {
        this.laserRange = range;
        SetLaserLength(range); // 즉시 지정된 길이로 설정
    }
    
    // 홀딩 상태 설정
    public void SetHoldingState(bool holding)
    {
        isHolding = holding;
        
        if (!holding)
        {
            // 홀딩 종료 시 레이저 페이드 아웃
            StartCoroutine(GetComponent<SpriteFade>().SlowFadeRoutine());
        }
    }

    // 레이저 길이를 즉시 설정
    private void SetLaserLength(float length)
    {
        // 스프라이트 크기 즉시 설정
        spriteRenderer.size = new Vector2(length, 1f);

        // 콜라이더 크기와 위치 즉시 설정
        capsuleCollider2D.size = new Vector2(length, capsuleCollider2D.size.y);
        capsuleCollider2D.offset = new Vector2(length / 2, capsuleCollider2D.offset.y);
    }
    
    private void LaserFaceMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 direction = transform.position - mousePos;
        transform.right = -direction;
    }
}
