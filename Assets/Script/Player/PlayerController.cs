using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool FacingLeft { get { return facingLeft; } }
    public static PlayerController Instance;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private TrailRenderer myTrailRenderer;
    private PlayerContorls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnim;
    private SpriteRenderer mySprite;
    private float startingMoveSpeed;

    private bool facingLeft = false;
    private bool isDashing = false;
    void Awake()
    {
        Instance = this;
        playerControls = new PlayerContorls();
        rb = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        mySprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        playerControls.Combat.Dash.performed += _ => Dash();
        startingMoveSpeed = moveSpeed;
    }
    void OnEnable()
    {
        playerControls.Enable();
    }
    void Update()
    {
        PlayerInput();
    }
    void FixedUpdate()
    {
        AdjustPlayerDirection();
        Move();
    }
    void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        myAnim.SetFloat("moveX", movement.x);
        myAnim.SetFloat("moveY", movement.y);
    }
    void Move()
    {
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }
    void AdjustPlayerDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);
        if (mousePos.x < playerScreenPoint.x)
        {
            mySprite.flipX = true;
            facingLeft = true;
        }
        else
        {
            mySprite.flipX = false;
            facingLeft = false;
        }
    }
    private void Dash()
    {
        if (!isDashing)
        {
            isDashing = true;
            moveSpeed *= dashSpeed;
            myTrailRenderer.emitting = true;
            StartCoroutine(EndDashRoutine());
        }
    }

    private IEnumerator EndDashRoutine()
    {
        float dashTime = .2f;
        float dashCD = .25f;
        yield return new WaitForSeconds(dashTime);
        moveSpeed = startingMoveSpeed;
        myTrailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }
}
