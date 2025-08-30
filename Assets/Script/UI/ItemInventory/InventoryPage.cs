using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPage : MonoBehaviour
{
    [SerializeField] private InventoryItem itemPrefab; // 해당 인벤토리 Item 프리펩
    [SerializeField] private RectTransform contentPanel;  // 인벤토리 아이템을 표시할 패널
    [SerializeField] private InventoryDescription itemDescription; // 아이템 설명 UI
    [SerializeField] private UI_MouseFollower mouseFollower; // 마우스 팔로워 UI

    List<InventoryItem> itemList = new List<InventoryItem>(); // 현재 페이지에 표시된 아이템 리스트

    public Sprite image;
    public int index;
    public string pageName, description;
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

            // 우클릭 이벤트 핸들러 등록
            newItem.OnItemClicked += HandleSelected;
            newItem.OnItemBeginDrag += HandleBeginDrag;
            newItem.OnItemDroppedOn += HandleSwap;
            newItem.OnItemEndDrag += HandleEndDrag;
            newItem.OnRightMouseBtnClicked += HandleShowItemActions;
        }
    }
    // 이벤트 콜백 함수들
    private void HandleShowItemActions(InventoryItem item)
    {
        
    }

    private void HandleEndDrag(InventoryItem item)
    {
        mouseFollower.Toggle(false);
    }

    private void HandleSwap(InventoryItem item)
    {
        
    }

    private void HandleBeginDrag(InventoryItem item)
    {
        mouseFollower.Toggle(true);
        mouseFollower.SetData(image, index);
    }

    private void HandleSelected(InventoryItem item)
    {
        itemDescription.SetDescription(image, pageName, description);
        itemList[0].Select();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        itemDescription.ResetDescription(); // 설명 초기화

        itemList[0].SetData(image, index);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
