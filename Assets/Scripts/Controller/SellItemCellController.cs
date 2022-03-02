using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellItemCellController : MonoBehaviour
{
    [SerializeField]
    GameObject defaultIcon;
    [SerializeField]
    Image itemIcon;
    [SerializeField]
    Text itemNameText;
    [SerializeField]
    Text itemExpText;
    [SerializeField]
    Text itemPriceText;
    [SerializeField]
    Button cellButton;
    [SerializeField]
    GameObject itemExpPanel;

    SellItemData sellItemData;
    Func<SellItemData, bool> action;

    public void setOnClickAction(Func<SellItemData, bool> _action)
    {
        action = _action;
    }

    public void setSellItem(SellItemData _sellItemData)
    {
        sellItemData = _sellItemData;

        defaultIcon.SetActive(_sellItemData == null);
        itemIcon.gameObject.SetActive(_sellItemData != null);
        itemExpPanel.SetActive(_sellItemData != null);
        cellButton.interactable = _sellItemData != null;

        if (_sellItemData != null)
        {
            ItemData itemData = ItemManager.instance.getItemData(_sellItemData.id);
            itemIcon.sprite = itemData.imageSpirte;
            itemNameText.text = LanguageManager.instance.getText(itemData.nameKey);
            itemExpText.text = string.Format(LanguageManager.instance.getText("ID_REMAIN_COUNT_N"), _sellItemData.remainCount < 0 ? LanguageManager.instance.getText("ID_INFINITE") : _sellItemData.remainCount.ToString());
            itemPriceText.text = Utils.convertPebToTorStr(_sellItemData.price) + " " + Const.TOR_COIN;
        }
    }

    internal void reset()
    {
        setSellItem(null);
    }

    public void onClickItem()
    {
        action?.Invoke(sellItemData);
    }
}
