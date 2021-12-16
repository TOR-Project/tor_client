﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImageLoadingComponent : LoadingComponent
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
            Debug.Log("sprite invalid");
            return false;
        }

        Image image = GetComponent<Image>();
        if (image.overrideSprite != sprite)
        {
            image.sprite = sprite;
            Debug.Log("sprite updated");
        }

        loadingCompleted = true;
        return true;
    }

    public override bool isLoadingCompleted()
    {
        return loadingCompleted;
    }

    public override string getLoadingTextKey()
    {
        return loadingInfoTextKey;
    }
}