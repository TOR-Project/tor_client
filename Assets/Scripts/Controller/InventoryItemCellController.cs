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
        itemData = _itemData;

        defaultIcon.SetActive(_itemData == null);
        itemIcon.gameObject.SetActive(_itemData != null);
        cellButton.interactable = _itemData != null;

        if (_itemData != null)
        {
            itemIcon.sprite = _itemData.imageSpirte;
            itemCountText.text = InventoryManager.instance.getMyItemCount(_itemData.id).ToString();
        } else
        {
            itemCountText.text = "";
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
