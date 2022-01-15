using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ButtonPressedSpriteLoadingComponent : LoadingComponent
{
    [SerializeField] internal string url;
    string loadingInfoTextKey = "ID_LOADING_IMAGE";

    float progress = 0;

    private void Awake()
    {
        if (Application.isEditor)
        {
            startLoading();
        }
    }

    public override void startLoading()
    {
        AssetsLoadManager.instance.requestSprite(url, updateSprite, updateProgress);
    }

    public bool updateSprite(Sprite sprite)
    {
        if (sprite == null)
        {
            AssetsLoadManager.instance.requestSprite(url, updateSprite, updateProgress);
            Debug.Log("btn sprite invalid : " + url);
            return false;
        }

        Button button = GetComponent<Button>();
        SpriteState spriteState = button.spriteState;
        if (spriteState.pressedSprite != sprite)
        {
            spriteState.pressedSprite = sprite;
            button.spriteState = spriteState;
            Debug.Log("btn sprite updated : " + url);
        }

        updateProgress(getProgressMax());
        return true;
    }

    public override void resetAll()
    {
        Button button = GetComponent<Button>();
        SpriteState spriteState = button.spriteState;
        spriteState.pressedSprite = null;
        button.spriteState = spriteState;

        progress = 0;
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