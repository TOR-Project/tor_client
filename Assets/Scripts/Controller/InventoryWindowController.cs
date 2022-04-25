using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryWindowController : MonoBehaviour, DataObserver<Dictionary<int, int>>
{
    [SerializeField]
    GameObject inventoryLoading;
    [SerializeField]
    InventoryItemCellController[] inventoryCellControllerArr;
    [SerializeField]
    InventoryItemInfoPanelController inventoryItemInfoPanelController;
    [SerializeField]
    CharacterGridController characterGridController;
    [SerializeField]
    GameObject characterSelectPopup;
    [SerializeField]
    GameObject confirmCheckPopup;
    [SerializeField]
    GlobalUIWindowController globalUIWindowController;

    DragonModule dragonModule;
    List<CharacterData> tempList;
    int tempCount;

    private void OnEnable()
    {
        initModule();

        resetInventoryGrid();
        inventoryLoading.SetActive(true);
        InventoryManager.instance.addObserver(this);
        InventoryManager.instance.requestInventoryItemList();

        inventoryItemInfoPanelController.resetItem();
        inventoryItemInfoPanelController.setUseAction(onItemUseButtonClicked);
    }

    private void initModule()
    {
        dragonModule = new DragonModule();
        ContractManager.instance.reqDragonDetectRate();
    }

    public void responseDragonDetectRate(int rate)
    {
        dragonModule.setDragonDetectRate(rate);
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
        InventoryManager.instance.removeObserver(this);
    }

    // responceInventoryItemList
    public void onDataReceived(Dictionary<int, int> _data)
    {
        resetInventoryGrid();
        inventoryItemInfoPanelController.setItem(0);

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

    bool onInventoryItemCellClicked(ItemData _data)
    {
        inventoryItemInfoPanelController.setItem(_data.id);
        return true;
    }

    void onItemUseButtonClicked(ItemData _data)
    {
        switch (_data.id)
        {
            case 1: // Dragon check scroll
                int maxCount = InventoryManager.instance.getMyItemCount(_data.id);
                characterGridController.setMaxSelectedCount(maxCount);
                characterGridController.setOnButtonClickedCallback(list =>
                {
                    useDragonCheckScroll(maxCount, list);
                    return true;
                });
                characterSelectPopup.SetActive(true);
                break;
            default:

                break;
        }
    }

    private void useDragonCheckScroll(int maxCount, List<CharacterData> list)
    {
        if (list.Count < maxCount)
        {
            tempCount = maxCount;
            tempList = list;
            confirmCheckPopup.SetActive(true);
        }
        else
        {
            List<int> tokenIdList = list.FindAll(cd => cd.race != CharacterManager.RACE_DRAGON).ConvertAll(cd => cd.tokenId);
            globalUIWindowController.showConfirmPopup(LanguageManager.instance.getText("ID_ITEM_USE_DRAGON_CHECK_SCROLL"), () => ContractManager.instance.reqUseDragonCheckScroll(list, maxCount, dragonModule.getDragon(tokenIdList, maxCount)));
        }
    }

    public void onUsePartButtonClicked()
    {
        List<int> tokenIdList = tempList.FindAll(cd => cd.race != CharacterManager.RACE_DRAGON).ConvertAll(cd => cd.tokenId);
        globalUIWindowController.showConfirmPopup(LanguageManager.instance.getText("ID_ITEM_USE_DRAGON_CHECK_SCROLL"), () => ContractManager.instance.reqUseDragonCheckScroll(tempList, tempList.Count, dragonModule.getDragon(tokenIdList, tempList.Count)));
    }

    public void onUseAllButtonClicked()
    {
        List<int> tokenIdList = tempList.FindAll(cd => cd.race != CharacterManager.RACE_DRAGON).ConvertAll(cd => cd.tokenId);
        globalUIWindowController.showConfirmPopup(LanguageManager.instance.getText("ID_ITEM_USE_DRAGON_CHECK_SCROLL"), () => ContractManager.instance.reqUseDragonCheckScroll(tempList, tempCount, dragonModule.getDragon(tokenIdList, tempCount)));
    }

    public void responseUseDragonCheckScroll(int[] tokenIdList, bool success)
    {
        StartCoroutine(startDragonCheckEffect(tokenIdList, success));
    }

    IEnumerator startDragonCheckEffect(int[] tokenIdList, bool success)
    {
        characterGridController.setEnableAllButtons(false);
        characterGridController.showDragonCheckEffect(tokenIdList);

        yield return new WaitForSeconds(3);

        characterGridController.setEnableAllButtons(true);

        if (success)
        {
            globalUIWindowController.showPopupByTextKey("ID_ITEM_USE_DRAGON_CHECK_SCROLL_SUCCESSED", () => characterSelectPopup.SetActive(false));
        }
        else
        {
            globalUIWindowController.showPopupByTextKey("ID_ITEM_USE_DRAGON_CHECK_SCROLL_FAILED", () => characterSelectPopup.SetActive(false));
        }
    }
}
