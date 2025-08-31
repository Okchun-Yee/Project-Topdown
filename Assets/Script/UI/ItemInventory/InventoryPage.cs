using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class InventoryPage : MonoBehaviour
    {
        [SerializeField] private InventoryItem itemPrefab; // 해당 인벤토리 Item 프리펩
        [SerializeField] private RectTransform contentPanel;  // 인벤토리 아이템을 표시할 패널
        [SerializeField] private InventoryDescription itemDescription; // 아이템 설명 UI
        [SerializeField] private UI_MouseFollower mouseFollower; // 마우스 팔로워 UI

        List<InventoryItem> itemList = new List<InventoryItem>(); // 현재 페이지에 표시된 아이템 리스트
        private int currentDraggedItemIndex = -1;   // 현재 드래그 중인 아이템 인덱스 (초기값 -1)

        public event Action<int> OnDescriptionRequested, // 아이템 우클릭
            OnItemActionRequested,  // 아이템 액션 요청
            OnStartDragging;
        public event Action<int, int> OnSwapItems; // 아이템 교환 요청 (드래그 앤 드롭)

        private void Awake()
        {
            Hide(); // 처음에는 숨김
            mouseFollower.Toggle(false); // 마우스 팔로워 숨김
            itemDescription.ResetDescription(); // 설명 초기화
        }
        public void Initialize_Inventory(int inventorySize)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                InventoryItem newItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                newItem.transform.SetParent(contentPanel);
                itemList.Add(newItem);

                // 이벤트 핸들러 등록
                newItem.OnItemClicked += HandleSelected;
                newItem.OnItemBeginDrag += HandleBeginDrag;
                newItem.OnItemDroppedOn += HandleSwap;
                newItem.OnItemEndDrag += HandleEndDrag;
                newItem.OnRightMouseBtnClicked += HandleShowItemActions;
            }
        }
        // 인벤토리 아이템 업데이트
        public void UpdateData(int itemIndex, Sprite image, int itemQuantity)
        {
            if (itemList.Count > itemIndex)
            {
                itemList[itemIndex].SetData(image, itemQuantity);
            }
        }
        // 이벤트 콜백 함수들
        private void HandleShowItemActions(InventoryItem item)
        {

        }

        private void HandleEndDrag(InventoryItem item)
        {
            ResetDraggedItem();
        }

        private void HandleSwap(InventoryItem item)
        {
            int index = itemList.IndexOf(item);  // 현재 드래그 중인 아이템 인덱스
            
            if (index == -1)
                return;  // 아이템이 리스트에 없으면 무시
            OnSwapItems?.Invoke(currentDraggedItemIndex, index);
        }
        // 아이템 정보 리셋
        private void ResetDraggedItem()
        {
            mouseFollower.Toggle(false);
            currentDraggedItemIndex = -1; // 드래그 중인 아이템 인덱스 초기화
        }

        private void HandleBeginDrag(InventoryItem item)
        {
            int index = itemList.IndexOf(item);  // 현재 드래그 중인 아이템 인덱스
            if (index == -1)
                return;  // 아이템이 리스트에 없으면 무시
            currentDraggedItemIndex = index;  // 현재 드래그 중인 아이템 인덱스 저장
            HandleSelected(item);
            OnStartDragging?.Invoke(index);
        }

        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            mouseFollower.Toggle(true);
            mouseFollower.SetData(sprite, quantity);
        }

        private void HandleSelected(InventoryItem item)
        {
            int index = itemList.IndexOf(item);  // 현재 드래그 중인 아이템 인덱스
            if (index == -1)
                return;  // 아이템이 리스트에 없으면 무시
            OnDescriptionRequested?.Invoke(index);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            itemDescription.ResetDescription(); // 설명 초기화
            ResetSelection();
        }

        public void ResetSelection()
        {
            itemDescription.ResetDescription();
            DeselectAllItems();
        }

        private void DeselectAllItems()
        {
            foreach (InventoryItem item in itemList)
            {
                item.Deselect();
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            ResetDraggedItem();
        }

        public void UpdateDescription(int itemIndex, Sprite item_Image, string item_Name, string item_Description)
        {
            itemDescription.SetDescription(item_Image, item_Name, item_Description);
            DeselectAllItems();
            itemList[itemIndex].Select();
        }
    }
}
