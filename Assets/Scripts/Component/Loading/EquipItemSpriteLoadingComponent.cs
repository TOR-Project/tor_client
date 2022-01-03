using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class EquipItemSpriteLoadingComponent : LoadingComponent
{
    string loadingInfoTextKey = "ID_EQUIP_ITEM_LOADING";

    public override void startLoading()
    {
        ItemManager.instance.startEquipItemSpriteLoading();
    }

    public override string getLoadingTextKey()
    {
        return "";
    }

    public override string getLoadingText()
    {
        return string.Format(LanguageManager.instance.getText(loadingInfoTextKey), getProgressCurrent(), getProgressMax());
    }

    public override int getProgressMax()
    {
        return ItemManager.instance.getEquipItemSpriteLoadedMax();
    }

    public override int getProgressCurrent()
    {
        return ItemManager.instance.getEquipItemSpriteLoadedCount();
    }
}