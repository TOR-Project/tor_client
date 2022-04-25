using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecretShopWindowController : MonoBehaviour, DataObserver<Dictionary<int, SellItemData>>, DataObserver<Dictionary<int, int>>
{
    [SerializeField]
    RectTransform kioskGridRT;
    [SerializeField]
    GameObject kioskLoading;
    [SerializeField]
    RectTransform inventoryGridRT;
    [SerializeField]
    GameObject inventoryLoading;
    [SerializeField]
    ShoppingCartPopupController shoppingCardPopupController;
    [SerializeField]
    SellItemCellController[] sellItemCellControllerArr;
    [SerializeField]
    InventoryItemCellController[] inventoryCellControllerArr;
    [SerializeField]
    Animator shoppingCardPopupAnimator;

    private string dismissingTrigger = "dismissing";

    private void OnEnable()
    {
        resetKioskGrid();
        kioskLoading.SetActive(true);
        SecretShopManager.instance.addObserver(this);
        SecretShopManager.instance.requestSellItemList();

        resetInventoryGrid();
        inventoryLoading.SetActive(true);
        InventoryManager.instance.addObserver(this);
        InventoryManager.instance.requestInventoryItemList();
    }

    private void resetKioskGrid()
    {
        foreach (SellItemCellController cellController in sellItemCellControllerArr)
        {
            cellController.reset();
            cellController.setOnClickAction(onSellItemCellClicked);
        }
    }

    private void resetInventoryGrid()
    {
        foreach (InventoryItemCellController cellController in inventoryCellControllerArr)
        {
            cellController.reset();
            cellController.setOnClickAction(onInventoryItemCellClicked);
        }
    }

    private void OnDisable()
    {
        SecretShopManager.instance.removeObserver(this);
        InventoryManager.instance.removeObserver(this);
    }

    // responceSellItemList
    public void onDataReceived(Dictionary<int, SellItemData> _data)
    {
        resetKioskGrid();

        List<int> keyList = new List<int>(_data.Keys);
        keyList.Sort();
        for (int idx = 0; idx < keyList.Count; idx++)
        {
            if (idx >= sellItemCellControllerArr.Length)
            {
                break;
            }
            int key = keyList[idx];
            SellItemData sellItemData = _data[key];

            sellItemCellControllerArr[idx].setSellItem(sellItemData);
        }

        kioskLoading.SetActive(false);
    }

    // responceInventoryItemList
    public void onDataReceived(Dictionary<int, int> _data)
    {
        resetInventoryGrid();

        List<int> keyList = new List<int>(_data.Keys);
        keyList.Sort();
        for (int idx = 0; idx < keyList.Count; idx++)
        {
            if (idx >= inventoryCellControllerArr.Length)
            {
                break;
            }

            int id = keyList[idx];
            ItemData itemData = ItemManager.instance.getItemData(id);

            inventoryCellControllerArr[idx].setItem(itemData);
        }

        inventoryLoading.SetActive(false);
    }

    bool onSellItemCellClicked(SellItemData _data)
    {
        shoppingCardPopupController.showPopup(_data.id, true);
        return true;
    }

    bool onInventoryItemCellClicked(ItemData _data)
    {
        shoppingCardPopupController.showPopup(_data.id, false);
        return true;
    }

    internal void responseBuyItem()
    {
        shoppingCardPopupAnimator.SetTrigger(dismissingTrigger);
    }

    internal void responseSellItem()
    {
        shoppingCardPopupAnimator.SetTrigger(dismissingTrigger);
    }
}
