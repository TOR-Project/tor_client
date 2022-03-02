using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class ShoppingCardPopupController : MonoBehaviour
{
    [SerializeField]
    Text titleText;
    [SerializeField]
    Image itemIconImage;
    [SerializeField]
    Text nameText;
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
    Slider countSlider;
    [SerializeField]
    Text countText;
    [SerializeField]
    Text priceText;
    [SerializeField]
    Text confirmButtonText;
    [SerializeField]
    GameObject amountPanel;
    [SerializeField]
    GameObject pricePanel;
    [SerializeField]
    GameObject warningPanel;
    [SerializeField]
    Button confirmButton;

    [SerializeField]
    GlobalUIWindowController globalUIWindowController;

    bool isBuyMode = false;
    ItemData currentItemData;
    SellItemData currentSellItemData;

    public void showPopup(int _itemId, bool _isBuyMode)
    {
        ItemData itemData = ItemManager.instance.getItemData(_itemId);
        SellItemData sellItemData = SecretShopManager.instance.getSellItemData(_itemId);

        titleText.text = confirmButtonText.text = LanguageManager.instance.getText(_isBuyMode ? "ID_BUY" : "ID_SELL");
        itemIconImage.sprite = itemData.imageSpirte;
        nameText.text = LanguageManager.instance.getText(itemData.nameKey);
        expText.text = LanguageManager.instance.getText(itemData.expKey);
        gradeText.text = ItemManager.instance.getGradeName(itemData.grade);
        categoryText.text = ItemManager.instance.getCategoryName(itemData.category);

        countSlider.value = countSlider.minValue = 1;
        countSlider.maxValue = _isBuyMode ? getMaxItemCount(sellItemData) : InventoryManager.instance.getMyItemCount(_itemId);

        amountPanel.SetActive(_isBuyMode || sellItemData != null);
        pricePanel.SetActive(_isBuyMode || sellItemData != null);
        warningPanel.SetActive(!_isBuyMode && sellItemData == null);
        confirmButton.interactable = _isBuyMode || sellItemData != null;

        isBuyMode = _isBuyMode;
        currentItemData = itemData;
        currentSellItemData = sellItemData;

        gameObject.SetActive(true);

        onCountChanged();
    }

    private int getMaxItemCount(SellItemData _sellItemData)
    {
        int remainCount = _sellItemData.remainCount < 0 ? int.MaxValue : _sellItemData.remainCount;
        int buyableCount = int.Parse((UserManager.instance.getCoinAmount() / _sellItemData.price).ToString());

        return Math.Max(1, Math.Min(remainCount, buyableCount));
    }

    public void minusCount(int _c)
    {
        if (countSlider.value - countSlider.minValue >= _c)
        {
            countSlider.value -= _c;
        }
        else
        {
            countSlider.value = countSlider.minValue;
        }
    }

    public void plusCount(int _c)
    {
        if (countSlider.maxValue - countSlider.value >= _c)
        {
            countSlider.value += _c;
        }
        else
        {
            countSlider.value = countSlider.maxValue;
        }
    }

    public void onCountChanged()
    {
        countText.text = string.Format(LanguageManager.instance.getText("ID_N_EA"), countSlider.value);
        priceText.text = Utils.convertPebToTorStr((int)(countSlider.value) * (isBuyMode ? currentSellItemData.price : currentSellItemData.sellPrice)) + " " + Const.TOR_COIN;
    }

    public void onConfirmButton()
    {
        int amount = (int)countSlider.value;
        if (isBuyMode)
        {
            BigInteger totalPrice = amount * currentSellItemData.price;

            if (totalPrice > UserManager.instance.getCoinAmount())
            {
                globalUIWindowController.showPopupByTextKey("ID_LEAK_TRT_BUY_ITEM", null);
            } else if (currentSellItemData.remainCount >= 0 && currentSellItemData.remainCount < amount)
            {
                globalUIWindowController.showPopupByTextKey("ID_LEAK_AMOUNT_BUY_ITEM", null);
            } else if (currentSellItemData.deadline < SystemInfoManager.instance.blockNumber)
            {
                globalUIWindowController.showPopupByTextKey("ID_BLOCK_OVER_BUY_ITEM", null);
            } else
            {
                string msg = string.Format(LanguageManager.instance.getText("ID_BUY_ITEM_CONFIRM"), LanguageManager.instance.getText(currentItemData.nameKey), amount, Utils.convertPebToTorStr(totalPrice));
                globalUIWindowController.showConfirmPopup(msg, () => ContractManager.instance.reqBuySecretShopItem(currentItemData.id, amount));
            }
        }
        else
        {
            BigInteger totalPrice = amount * currentSellItemData.sellPrice;

            string msg = string.Format(LanguageManager.instance.getText("ID_SELL_ITEM_CONFIRM"), LanguageManager.instance.getText(currentItemData.nameKey), amount, Utils.convertPebToTorStr(totalPrice));
            globalUIWindowController.showConfirmPopup(msg, () => ContractManager.instance.reqSellSecretShopItem(currentItemData.id, amount));
        }
    }
}
