using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.ItemData
{
    [CreateAssetMenu(menuName = "New Item")]
    public class ItemInfo : ScriptableObject
    {
        [field: SerializeField] public bool IsStackable { get; set; }   // 프로퍼티를 인스펙터에서 설정
        public int itemID => GetInstanceID();   // item ID 에 접근
        [field: SerializeField] public int MaxStackSize { get; set; } = 1;

        [field: SerializeField] public string Item_Name { get; set; }

        [field: SerializeField]
        [field: TextArea]
        public string Item_Description { get; set; }
        [field: SerializeField] public Sprite Item_Image { get; set; }

    }
}