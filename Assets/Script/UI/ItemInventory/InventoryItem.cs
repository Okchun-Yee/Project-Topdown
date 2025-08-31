using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Reflection;
using System.Drawing;

namespace Inventory.UI
{
    public class InventoryItem : MonoBehaviour, IPointerClickHandler,
        IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text quantityText;
        [SerializeField] private Image borderImage;
        public event Action<InventoryItem> OnItemClicked,
            OnItemDroppedOn,
            OnItemBeginDrag,
            OnItemEndDrag,
            OnRightMouseBtnClicked; // Right mouse: 아이템 옵션
        private bool empty = true;

        private void Awake()
        {
            ResetData();
            Deselect();
        }

        public void ResetData()
        {
            this.itemImage.gameObject.SetActive(false);
            empty = true;
        }

        public void Deselect()
        {
            borderImage.enabled = false;
        }
        public void SetData(Sprite sprite, int quantity)
        {
            this.itemImage.gameObject.SetActive(true);
            this.itemImage.sprite = sprite;
            this.quantityText.text = quantity + "";
            empty = false;
        }
        public void Select()
        {
            borderImage.enabled = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndDrag?.Invoke(this);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (empty) return;
            OnItemBeginDrag?.Invoke(this);
        }

        public void OnDrop(PointerEventData eventData)
        {
            OnItemDroppedOn?.Invoke(this);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                OnRightMouseBtnClicked?.Invoke(this);
            }
            else
            {
                OnItemClicked?.Invoke(this);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {

        }
    }
}