using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

namespace Inventory.ItemData
{
    [CreateAssetMenu(menuName = "New Inventory")]
    public class InventoryInfo : ScriptableObject
    {
        [SerializeField] private List<InventorySlot> inventorySlots;

        [field: SerializeField] public int Size { get; private set; } = 10;
        public event Action<Dictionary<int, InventorySlot>> OnInventoryUpdated; // 인벤토리 업데이트 (딕셔너리 구조 이벤트)
        public void Initialized()
        {
            inventorySlots = new List<InventorySlot>(Size);
            for (int i = 0; i < Size; i++)
            {
                inventorySlots.Add(InventorySlot.GetEmptySlot());
            }
        }

        // 외부 아이템 추가 => 내부에서 자세한 데이터 추가
        private void AddItem(ItemInfo item, int quantity)
        {
            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (inventorySlots[i].IsEmpty)
                {
                    inventorySlots[i] = new InventorySlot
                    {
                        item = item,
                        quantity = quantity
                    };
                    return;
                }
            }
        }

        // 외부에서 아이템 추가
        public void AddItem(InventorySlot item)
        {
            AddItem(item.item, item.quantity);
        }

        // Key(index) : Val(item) 
        // 인벤토리 업데이트용 메서드
        public Dictionary<int, InventorySlot> GetCurrentInventoryState()
        {
            Dictionary<int, InventorySlot> returnVal = new Dictionary<int, InventorySlot>();

            for (int i = 0; i < inventorySlots.Count; i++)
            {
                if (inventorySlots[i].IsEmpty) continue;
                returnVal[i] = inventorySlots[i];
            }
            return returnVal;
        }

        public InventorySlot GetItemAt(int itemIndex)
        {
            return inventorySlots[itemIndex];
        }

        public void SwapItems(int itemIndex1, int itemIndex2)
        {
            InventorySlot temp = inventorySlots[itemIndex1];
            inventorySlots[itemIndex1] = inventorySlots[itemIndex2];
            inventorySlots[itemIndex2] = temp;

            InformAboutChange();
        }

        private void InformAboutChange()
        {
            OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
        }
    }

    [Serializable]
    public struct InventorySlot
    {
        public ItemInfo item;
        public int quantity;
        public bool IsEmpty => item == null;

        public InventorySlot ChangeQuantity(int newQuantity)
        {
            return new InventorySlot
            {
                item = this.item,
                quantity = newQuantity
            };
        }
        public static InventorySlot GetEmptySlot()
            => new InventorySlot
            {
                item = null,
                quantity = 0
            };
    }
}