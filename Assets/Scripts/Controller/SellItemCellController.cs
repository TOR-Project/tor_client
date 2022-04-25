using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellItemCellController : MonoBehaviour, BlockNumberObserever
{
    [SerializeField]
    GameObject defaultIcon;
    [SerializeField]
    Image itemIcon;
    [SerializeField]
    Text itemNameText;
    [SerializeField]
    Text itemExpText1;
    [SerializeField]
    Text itemExpText2;
    [SerializeField]
    Text itemPriceText;
    [SerializeField]
    Button cellButton;
    [SerializeField]
    GameObject itemExpPanel;

    SellItemData sellItemData;
    Func<SellItemData, bool> action;

    private void OnEnable()
    {
        SystemInfoManager.instance.addBlockNumberObserver(this);
    }

    private void OnDisable()
    {
        SystemInfoManager.instance.removeBlockNumberObserver(this);
    }

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
            updateItemExp();
            itemPriceText.text = Utils.convertPebToTorStr(_sellItemData.price) + " " + Const.TOR_COIN;
        }
    }

    private void updateItemExp()
    {
        if (sellItemData == null)
        {
            return;
        }

        string remainCountStr;
        string remainTimeStr;

        if (sellItemData.deadline <= 0) // infinity
        {
            remainTimeStr = LanguageManager.instance.getText("ID_INFINITE");
        }
        else if (sellItemData.deadline <= SystemInfoManager.instance.blockNumber)
        {
            remainTimeStr = LanguageManager.instance.getText("ID_BUY_DISABLED");
        }
        else
        {
            long remainTime = sellItemData.deadline - SystemInfoManager.instance.blockNumber;
            if (remainTime < 0)
            {
                remainTime = 0;
            }

            remainTimeStr = string.Format("{0:#,##0} block", remainTime);
        }

        if (sellItemData.remainCount < 0) // infinity
        {
            remainCountStr = LanguageManager.instance.getText("ID_INFINITE");
        }
        else if (sellItemData.remainCount == 0)
        {
            remainCountStr = LanguageManager.instance.getText("ID_SOLD_OUT");
        }
        else
        {
            remainCountStr = string.Format(LanguageManager.instance.getText("ID_N_EA"), sellItemData.remainCount);
        }

        itemExpText1.text = string.Format(LanguageManager.instance.getText("ID_REMAIN_COUNT_N"), remainCountStr);
        itemExpText2.text = string.Format(LanguageManager.instance.getText("ID_REMAIN_BLOCK_N"), remainTimeStr);

    }

    internal void reset()
    {
        setSellItem(null);
    }

    public void onClickItem()
    {
        action?.Invoke(sellItemData);
    }

    public void onBlockNumberChanged(long _num)
    {
        updateItemExp();
    }
}
