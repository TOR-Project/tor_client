using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemCellController : MonoBehaviour
{
    [SerializeField]
    GameObject defaultIcon;
    [SerializeField]
    Image itemIcon;
    [SerializeField]
    Text itemCountText;
    [SerializeField]
    Button cellButton;

    ItemData itemData;
    Func<ItemData, bool> action;

    public void setOnClickAction(Func<ItemData, bool> _action)
    {
        action = _action;
    }

    public void setItem(ItemData _itemData)
    {
        if (_itemData != null && InventoryManager.instance.getMyItemCount(_itemData.id) <= 0)
        {
            itemData = null;
        } else
        {
            itemData = _itemData;
        }

        defaultIcon.SetActive(itemData == null);
        itemIcon.gameObject.SetActive(itemData != null);
        itemCountText.gameObject.SetActive(itemData != null);
        cellButton.interactable = itemData != null;

        if (itemData != null)
        {
            itemIcon.sprite = itemData.imageSpirte;
            itemCountText.text = "x" + InventoryManager.instance.getMyItemCount(itemData.id).ToString();
        }

    }

    public void onClickItem()
    {
        action?.Invoke(itemData);
    }

    internal void reset()
    {
        setItem(null);
    }
}
