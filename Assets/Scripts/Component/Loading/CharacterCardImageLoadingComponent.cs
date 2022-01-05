using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CharacterCardImageLoadingComponent : LoadingComponent
{
    string loadingInfoTextKey = "ID_CHARACTER_CARD_IMAGE_LOADING";

    public override void startLoading()
    {
        CharacterManager.instance.startAvatarImageLoading();
        CountryManager.instance.startFlagImageLoading();
        ItemManager.instance.startGemImageLoading();
    }

    public override string getLoadingTextKey()
    {
        return loadingInfoTextKey;
    }

    public override float getProgressMax()
    {
        return 3;
    }

    public override float getProgressCurrent()
    {
        int step = 0;
        step += CharacterManager.instance.isAvatarImageLoaded() ? 1 : 0;
        step += CountryManager.instance.isFlagImageLoaded() ? 1 : 0;
        step += ItemManager.instance.isGemImageLoaded() ? 1 : 0;
        return step;
    }
}