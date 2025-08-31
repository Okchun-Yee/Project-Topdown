using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Inventory.UI;
using Inventory.ItemData;

namespace Inventory
{
    public class InventoryController : Singleton<InventoryController>
    {
        [SerializeField] private InventoryPage inventoryPage; // 같은 오브젝트에서 쉽게 참조
        [SerializeField] private InventoryInfo inventoryInfo; // 인벤토리 데이터
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
                PrepareUI();
                //inventoryInfo.Initialized();    // 인벤토리 데이터 초기화
            }
        }

        private void PrepareUI()
        {
            inventoryPage.Initialize_Inventory(inventorySize);  // 인벤토리 페이지 초기화

            // UI 이벤트 구독
            inventoryPage.OnDescriptionRequested += HandleDescriptionRequested;
            inventoryPage.OnSwapItems += HandleSwapItems;
            inventoryPage.OnStartDragging += HandleDragging;
            inventoryPage.OnItemActionRequested += HandleItemActionRequested;
        }

        private void HandleItemActionRequested(int itemIndex)
        {
        }

        private void HandleDragging(int itemIndex)
        {
        }

        private void HandleSwapItems(int itemIndex1, int itemIndex2)
        {
        }

        private void HandleDescriptionRequested(int itemIndex)
        {
            InventorySlot inventorySlot = inventoryInfo.GetItemAt(itemIndex);
            if (inventorySlot.IsEmpty)
            {
                inventoryPage.ResetSelection();
                return;
            }
            ItemInfo item = inventorySlot.item;
            inventoryPage.UpdateDescription(itemIndex, item.Item_Image,
                item.Item_Name, item.Item_Description);
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
                foreach (var slot in inventoryInfo.GetCurrentInventoryState())
                {
                    inventoryPage.UpdateData(slot.Key, // index
                        slot.Value.item.Item_Image, // sprite
                        slot.Value.quantity);   // quantity
                }
            }
            else
            {
                inventoryPage.Hide();
            }
        }
    }
}
