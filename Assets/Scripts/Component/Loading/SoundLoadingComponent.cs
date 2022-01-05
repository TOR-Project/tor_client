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

    float progress = 0;

    public override void startLoading()
    {
        SoundManager.instance.requestSound(key, url, isBgm, onLoadingCompleted, updateProgress);
    }

    public void onLoadingCompleted()
    {
        updateProgress(getProgressMax());
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