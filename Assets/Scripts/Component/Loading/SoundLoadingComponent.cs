using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SoundLoadingComponent : LoadingComponent
{
    [SerializeField] internal string key;
    [SerializeField] internal string url;
    [SerializeField] internal bool isBgm;
    string loadingInfoTextKey = "ID_LOADING_SOUND";

    bool loadingCompleted = false;

    public override void startLoading()
    {
        SoundManager.instance.requestSound(key, url, isBgm, onLoadingCompleted);
    }

    public void onLoadingCompleted()
    {
        loadingCompleted = true;
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