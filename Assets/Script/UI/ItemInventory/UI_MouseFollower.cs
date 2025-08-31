using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory.UI;

public class UI_MouseFollower : MonoBehaviour
{
    private Canvas canvas;
    private InventoryItem item;
    public void Awake()
    {
        canvas = transform.root.GetComponent<Canvas>();
        item = GetComponentInChildren<InventoryItem>();
    }
    public void SetData(Sprite sprite, int quantity)
    {
        item.SetData(sprite, quantity);
    }
    private void Update()
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.worldCamera,
            out position);
        transform.position = canvas.transform.TransformPoint(position);
    }
    public void Toggle(bool val)
    {
        gameObject.SetActive(val);
    }
}
