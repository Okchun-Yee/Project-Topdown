using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingProjectile : MonoBehaviour
{
    [SerializeField] private float laserGrowTime = 2f;
    private bool isGrowing = true;
    private bool isHolding = false;
    private bool shouldFollowMouse = false; // 마우스 추적 여부 추가
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
    
    // Update 추가 - 실시간 마우스 추적
    private void Update()
    {
        if (isHolding && shouldFollowMouse)
        {
            LaserFaceMouse();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Indestructible>() && !collision.isTrigger)
        {
            isGrowing = false;
        }
    }
    
    public void UpdateLaserRange(float range)
    {
        this.laserRange = range;
        StartCoroutine(IncreaseLaserLengthRoutine());
    }
    
    // 홀딩 상태 설정
    public void SetHoldingState(bool holding)
    {
        isHolding = holding;
        shouldFollowMouse = holding; // 홀딩 상태와 마우스 추적 연동
        
        if (!holding)
        {
            // 홀딩 종료 시 레이저 페이드 아웃
            StartCoroutine(GetComponent<SpriteFade>().SlowFadeRoutine());
        }
    }

    private IEnumerator IncreaseLaserLengthRoutine()
    {
        float timePassed = 0f;
        
        // 레이저 성장
        while (spriteRenderer.size.x < laserRange && isGrowing)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / laserGrowTime;

            //sprite
            spriteRenderer.size = new Vector2(Mathf.Lerp(1f, laserRange, linearT), 1f);

            //collider
            capsuleCollider2D.size = new Vector2(Mathf.Lerp(1f, laserRange, linearT), capsuleCollider2D.size.y);
            capsuleCollider2D.offset = new Vector2(Mathf.Lerp(1f, laserRange, linearT) / 2, capsuleCollider2D.offset.y);

            yield return null;
        }
        
        // 홀딩 중에는 레이저 유지, 홀딩 종료까지 대기
        while (isHolding)
        {
            yield return null;
        }
        
        // 홀딩이 끝나면 자동으로 페이드 아웃은 SetHoldingState에서 처리됨
    }
    
    private void LaserFaceMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 direction = transform.position - mousePos;
        transform.right = -direction;
    }
}
