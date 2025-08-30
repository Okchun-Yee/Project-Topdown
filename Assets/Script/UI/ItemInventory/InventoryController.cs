using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : Singleton<InventoryController>
{
    [SerializeField] private InventoryPage inventoryPage; // 같은 오브젝트에서 쉽게 참조
    private PlayerContorls pc;
    public int inventorySize = 10;

    protected override void Awake()
    {
        base.Awake(); // Singleton Awake 호출
        
        if (this == Instance) // 이 인스턴스가 Singleton 인스턴스라면
        {
            pc = new PlayerContorls();
        }
    }
    
    private void Start()
    {
        if (inventoryPage != null)
        {
            inventoryPage.Initialize_Inventory(inventorySize);
        }
    }

    private void OnEnable()
    {
        if (pc != null)
        {
            pc.Inventory.Enable();
            pc.Inventory.InventoryUI.started += OnInventoryToggle;
        }
    }

    private void OnDisable()
    {
        if (pc != null)
        {
            pc.Inventory.InventoryUI.started -= OnInventoryToggle;
            pc.Inventory.Disable();
        }
    }

    private void OnInventoryToggle(InputAction.CallbackContext context)
    {
        if (inventoryPage == null || BaseSkill.IsCasting ||
        BaseWeapon.IsAttacking || PlayerHealth.Instance.isDead) return;
        
        if (!inventoryPage.isActiveAndEnabled)
        {
            inventoryPage.Show();
        }
        else
        {
            inventoryPage.Hide();
        }
    }
}
