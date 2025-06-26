using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Singleton<PlayerController>
{
    public bool FacingLeft { get { return facingLeft; } }

    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private TrailRenderer myTrailRenderer;
    [SerializeField] private Transform weaponCollider;
    
    private PlayerContorls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnim;
    private SpriteRenderer mySprite;

    private WeaponManager weaponManager;
    private WeaponPickUp curPickUp;

    private float startingMoveSpeed;

    private bool facingLeft = false;
    private bool isDashing = false;
    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerContorls();
        rb = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        mySprite = GetComponent<SpriteRenderer>();

        weaponManager = FindObjectOfType<WeaponManager>();
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            curPickUp = collision.GetComponent<WeaponPickUp>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon") && curPickUp != null && collision.GetComponent<WeaponPickUp>() == curPickUp)
        {
            curPickUp = null;
        }
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
    public Transform GetWeaponCollider() {
        return weaponCollider;
    }
    void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        myAnim.SetFloat("moveX", movement.x);
        myAnim.SetFloat("moveY", movement.y);

        //Press "E" key for weapon
        if (curPickUp != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            // UI 갱신 이벤트
            weaponManager.EquipWeapon(curPickUp.info);
            Destroy(curPickUp.gameObject);
            curPickUp = null;
        }
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
