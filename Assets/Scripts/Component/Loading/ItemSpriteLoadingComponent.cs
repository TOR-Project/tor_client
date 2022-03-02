using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ItemSpriteLoadingComponent : LoadingComponent
{
    string loadingInfoTextKey = "ID_ITEM_LOADING";

    public override void startLoading()
    {
        ItemManager.instance.startItemSpriteLoading();
    }

    public override string getLoadingTextKey()
    {
        return "";
    }

    public override string getLoadingText()
    {
        return string.Format(LanguageManager.instance.getText(loadingInfoTextKey), getProgressCurrent(), getProgressMax());
    }

    public override float getProgressMax()
    {
        return ItemManager.instance.getItemSpriteLoadedMax();
    }

    public override float getProgressCurrent()
    {
        return ItemManager.instance.getItemSpriteLoadedCount();
    }
}