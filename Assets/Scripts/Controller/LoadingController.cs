using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections;

public class LoadingController : MonoBehaviour
{
    [SerializeField]
    Slider slider;
    [SerializeField]
    LanguageTextController textController;

    LoadingComponentPool loadingComponentPool;
    public bool setupLoading(GameObject _targetWindow, Action _callback)
    {
        loadingComponentPool = _targetWindow.GetComponent<LoadingComponentPool>();
        if (loadingComponentPool == null || loadingComponentPool.isAllLoadingCompleted())
        {
            return true;
        }

        slider.value = 0;
        StartCoroutine(checkProgress(_callback));
        return false;
    }

    public IEnumerator checkProgress(Action _callback)
    {
        LoadingComponent[] loadingComponet = loadingComponentPool.getLoadingComponents();
        if (loadingComponet != null)
        {
            slider.maxValue = loadingComponet.Length;
            foreach (LoadingComponent lc in loadingComponet)
            {
                lc.startLoading();
                updateText(lc.getLoadingTextKey());
                yield return new WaitUntil(lc.isLoadingCompleted);

                slider.value += 1;
            }
        }

        loadingComponentPool = null;
        _callback();
    }

    void updateText(string _key)
    {
        if (textController.key != _key)
        {
            textController.key = _key;
            textController.onLanguageChanged();
        }
    }
}