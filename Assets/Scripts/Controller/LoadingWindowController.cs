using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections;

public class LoadingWindowController : MonoBehaviour
{
    [SerializeField]
    Slider slider;
    [SerializeField]
    Text loadingText;
    [SerializeField]
    GlobalUIWindowController globalUIController;

    LoadingComponentPool loadingComponentPool;

    private void OnEnable()
    {
        globalUIController.hideInfoPanel();
    }

    public bool setupLoading(GameObject _targetWindow, Action _callback)
    {
        loadingComponentPool = _targetWindow.GetComponent<LoadingComponentPool>();
        if (loadingComponentPool == null)
        {
            return true;
        }

        loadingComponentPool.init();
        if (loadingComponentPool.isAllLoadingCompleted()) {
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
            float max = 0;
            foreach (LoadingComponent lc in loadingComponet)
            {
                lc.startLoading();
                max += lc.getProgressMax();
            }
            slider.maxValue = max;
            foreach (LoadingComponent lc in loadingComponet)
            {
                float progress = slider.value;
                while (!lc.isLoadingCompleted())
                {
                    loadingText.text = lc.getLoadingText();
                    slider.value = progress + lc.getProgressCurrent();
                    yield return new WaitForSeconds(0.1f);
                    // Debug.Log(lc.gameObject.name + " " + lc.getProgressCurrent() + " / " + lc.getProgressMax());
                }
                slider.value = progress + lc.getProgressMax();
            }
        }

        loadingComponentPool = null;
        _callback();
    }

    internal void clearAll(GameObject _activeWindow)
    {
        if (!Const.MEMORY_SAVE)
        {
            return;
        }

        AssetsLoadManager.instance.cleanAll();
        SoundManager.instance.cleanAll();

        LoadingComponentPool loadingComponentPool = _activeWindow.GetComponent<LoadingComponentPool>();
        if (loadingComponentPool == null)
        {
            return;
        }

        LoadingComponent[] loadingComponet = loadingComponentPool.getLoadingComponents();
        if (loadingComponet != null)
        {
            foreach (LoadingComponent lc in loadingComponet)
            {
                lc.resetAll();
            }
        }

        System.GC.Collect();
        Resources.UnloadUnusedAssets();
    }
}