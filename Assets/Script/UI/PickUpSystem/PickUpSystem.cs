using System.Collections;
using System.Collections.Generic;
using Inventory.ItemData;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpSystem : MonoBehaviour
{
    [SerializeField] private InventoryInfo inventoryInfo;
    private PlayerContorls pc;
    private bool isPlayerInRange = false;

    // 현재 아이템 판별
    private ItemPickUp itemPickup;
    private WeaponPickup weaponPickup;

    private void Awake()
    {
        pc = new PlayerContorls();
        // 자신이 어떤 타입인지 확인
        itemPickup = GetComponent<ItemPickUp>();
        weaponPickup = GetComponent<WeaponPickup>();
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
        Debug.Log($"Player in range of {GetPickupType()}");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) { return; }
        isPlayerInRange = false;
        //UI 이벤트 종료
    }
    // 입력 처리
    private void PlayerInput(InputAction.CallbackContext ctx)
    { 
        if (!isPlayerInRange)
            return;

        // 아이템 또는 무기 획득
        if (weaponPickup != null)
        {
            Debug.Log("Picking up weapon");
            weaponPickup.Weapon_Pickup();
        }
        else if (itemPickup != null)
        {
            Debug.Log("Picking up item");
            itemPickup.Item_Pickup(inventoryInfo);
        }
    }
    // 디버깅용: 현재 픽업 타입 확인
    private string GetPickupType()
    {
        if (weaponPickup != null) return "Weapon";
        if (itemPickup != null) return "Item";
        return "Unknown";
    }
}
