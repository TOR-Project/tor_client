using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SpriteLoadingComponent : LoadingComponent
{
    [SerializeField] internal string url;
    string loadingInfoTextKey = "ID_LOADING_IMAGE";

    bool loadingCompleted = false;

    public override void startLoading()
    {
        AssetsLoadManager.instance.requestSprite(url, updateSprite);
    }

    public bool updateSprite(Sprite sprite)
    {
        if (sprite == null)
        {
            AssetsLoadManager.instance.requestSprite(url, updateSprite);
            ContractManager.instance.printLog("sprite invalid : " + url);
            return false;
        }

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (renderer.sprite != sprite)
        {
            renderer.sprite = sprite;
            ContractManager.instance.printLog("sprite updated : " + url);
        }

        loadingCompleted = true;
        return true;
    }

    public override string getLoadingTextKey()
    {
        return loadingInfoTextKey;
    }

    public override int getProgressCurrent()
    {
        return loadingCompleted ? 1 : 0;
    }
}