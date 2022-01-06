using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SDFFontLoadingComponent : LoadingComponent
{
    [SerializeField]
    string fileName;
    string loadingInfoTextKey = "ID_FONT_LOADING";

    float progress = 0;

    public override void startLoading()
    {
        AssetsLoadManager.instance.requestAssets(fileName, updateFont, updateProgress);
    }

    public bool updateFont(AssetBundle _bundle)
    {
        TMP_FontAsset font = _bundle.LoadAsset<TMP_FontAsset>("ON_L SDF");
        if (font == null)
        {
            AssetsLoadManager.instance.requestAssets(fileName, updateFont, updateProgress);
            Debug.Log("font invalid : " + fileName);
            return false;
        }

        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        if (text.font != font)
        {
            text.font = font;
            Debug.Log("font updated : " + fileName);
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

    public override float getProgressMax()
    {
        return 10;
    }

    public override float getProgressCurrent()
    {
        return progress * getProgressMax();
    }
}