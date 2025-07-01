using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private Weaponinfo weaponinfo;
    private PlayerContorls pc;
    private bool isPlayerInRange = false;

    private void Awake()
    {
        if (weaponinfo == null)
        { Debug.LogError($"[WeaponPickup] WeaponInfo is missing on {name}"); }
        pc = new PlayerContorls();
    }
    private void OnEnable()
    {
        pc.Inventory.Enable();
        pc.Inventory.Pickup.started += PlayerInput;
    }
    private void OnDisable()
    {
        pc.Inventory.Pickup.started -= PlayerInput;
        pc.Inventory.Disable();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) { return; }
        isPlayerInRange = true;
        //UI 이벤트 (EX. "G" 키를 누르시오)
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) { return; }
        isPlayerInRange = false;
        //UI 이벤트 종료
    }
    private void PlayerInput(InputAction.CallbackContext ctx)
    {
        if (!isPlayerInRange)
            return;

        Pickup();
    }
    private void Pickup()
    {
         // 1) 무기 장착 요청
        WeaponManager.Instance.EquipWeapon(weaponinfo);

        // 2) 획득 효과 재생 (사운드, 파티클)

        // 3) 즉시 제거하여 리소스 최소화
        Destroy(gameObject);
    }
}
