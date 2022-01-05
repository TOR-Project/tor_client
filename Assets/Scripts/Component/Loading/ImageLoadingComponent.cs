using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageLoadingComponent : LoadingComponent
{
    [SerializeField] internal string url;
    string loadingInfoTextKey = "ID_LOADING_IMAGE";

    float progress = 0;

    public override void startLoading()
    {
        AssetsLoadManager.instance.requestSprite(url, updateSprite, updateProgress);
    }

    public bool updateSprite(Sprite sprite)
    {
        if (sprite == null)
        {
            AssetsLoadManager.instance.requestSprite(url, updateSprite, updateProgress);
            Debug.Log("image invalid : " + url);
            return false;
        }

        Image image = GetComponent<Image>();
        if (image.overrideSprite != sprite)
        {
            image.sprite = sprite;
            Debug.Log("image updated : " + url);
        }

        updateProgress(getProgressMax());
        return true;
    }

    public bool updateProgress(float _progress)
    {
        progress = _progress;
        return true;
    }

    public override string getLoadingTextKey()
    {
        return loadingInfoTextKey;
    }

    public override float getProgressCurrent()
    {
        return progress;
    }
}