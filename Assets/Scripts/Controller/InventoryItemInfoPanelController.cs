using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemInfoPanelController : MonoBehaviour
{
    [SerializeField]
    GameObject contentsPanel;
    [SerializeField]
    Image itemIconImage;
    [SerializeField]
    Text nameText;
    [SerializeField]
    Text countText;
    [SerializeField]
    Text categoryText;
    [SerializeField]
    Text gradeText;
    [SerializeField]
    Text raceText;
    [SerializeField]
    Text jobText;
    [SerializeField]
    Text optionText;
    [SerializeField]
    Text expText;
    [SerializeField]
    Text usingText;
    [SerializeField]
    GameObject controlPanel;

    ItemData currentItemData;

    Action<ItemData> useItemAction;

    public void resetItem()
    {
        setItem(0);
    }

    public void setItem(int _itemId)
    {
        contentsPanel.SetActive(_itemId > 0);
        if (_itemId <= 0)
        {
            currentItemData = null;
            return;
        }
        ItemData itemData = ItemManager.instance.getItemData(_itemId);

        currentItemData = itemData;

        itemIconImage.sprite = itemData.imageSpirte;
        nameText.text = LanguageManager.instance.getText(itemData.nameKey);
        countText.text = string.Format(LanguageManager.instance.getText("ID_N_EA"), InventoryManager.instance.getMyItemCount(_itemId));
        expText.text = LanguageManager.instance.getText(itemData.expKey);
        usingText.text = LanguageManager.instance.getText(itemData.usingKey);
        gradeText.text = ItemManager.instance.getGradeName(itemData.grade);
        categoryText.text = ItemManager.instance.getCategoryName(itemData.category);

        bool controlPanelActivate;
        switch(_itemId)
        {
            case 1: // Dragon check scroll
                controlPanelActivate = true;
                break;
            default:
                controlPanelActivate = false;
                break;
        }
        controlPanel.SetActive(controlPanelActivate);
    }

    public void setUseAction(Action<ItemData> _action)
    {
        useItemAction = _action;
    }


    public void onUseButtonClicked()
    {
        useItemAction?.Invoke(currentItemData);
    }
}
