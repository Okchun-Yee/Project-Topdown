using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Inventory.UI
{
    public class InventoryDescription : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text itemTitle;
        [SerializeField] private TMP_Text itemDescription;

        public void Awake()
        {
            // 초기화 작업  
            ResetDescription();
        }

        public void ResetDescription()
        {
            itemImage.gameObject.SetActive(false);
            itemTitle.text = "";
            itemDescription.text = "";
        }
        public void SetDescription(Sprite sprite, string title, string description)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = sprite;
            itemTitle.text = title;
            itemDescription.text = description;
        }
    }
}