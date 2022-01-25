using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ButtonStateSpriteLoadingComponent : LoadingComponent
{
    [SerializeField] internal string highlightUrl;
    [SerializeField] internal string pressedUrl;
    [SerializeField] internal string selectedUrl;
    [SerializeField] internal string disabledUrl;
    string loadingInfoTextKey = "ID_LOADING_IMAGE";

    float highlightProgress = 0;
    float pressedProgress = 0;
    float selectedProgress = 0;
    float disabledProgress = 0;

    float maxProgress = 0;

    private void Awake()
    {
        if (Application.isEditor)
        {
            startLoading();
        }
    }

    public override void startLoading()
    {
        if (highlightUrl != null && highlightUrl != "")
        {
            AssetsLoadManager.instance.requestSprite(highlightUrl, updateHighlightSprite, updateHighlightProgress);
            maxProgress += 1;
        }

        if (pressedUrl != null && pressedUrl != "")
        {
            AssetsLoadManager.instance.requestSprite(pressedUrl, updatePressedSprite, updatePressedProgress);
            maxProgress += 1;
        }

        if (selectedUrl != null && selectedUrl != "")
        {
            AssetsLoadManager.instance.requestSprite(selectedUrl, updateSelectedSprite, updateSelectedProgress);
            maxProgress += 1;
        }

        if (disabledUrl != null && disabledUrl != "")
        {
            AssetsLoadManager.instance.requestSprite(disabledUrl, updateDisabledSprite, updateDisabledProgress);
            maxProgress += 1;
        }
    }

    public bool updateHighlightSprite(Sprite sprite)
    {
        if (sprite == null)
        {
            AssetsLoadManager.instance.requestSprite(highlightUrl, updateHighlightSprite, updateHighlightProgress);
            Debug.Log("btn highlighted sprite invalid : " + highlightUrl);
            return false;
        }

        Button button = GetComponent<Button>();
        SpriteState spriteState = button.spriteState;
        if (spriteState.highlightedSprite != sprite)
        {
            spriteState.highlightedSprite = sprite;
            button.spriteState = spriteState;
            Debug.Log("btn highlighted sprite updated : " + highlightUrl);
        }

        updateHighlightProgress(1);
        return true;
    }

    public bool updatePressedSprite(Sprite sprite)
    {
        if (sprite == null)
        {
            AssetsLoadManager.instance.requestSprite(pressedUrl, updatePressedSprite, updatePressedProgress);
            Debug.Log("btn pressed sprite invalid : " + pressedUrl);
            return false;
        }

        Button button = GetComponent<Button>();
        SpriteState spriteState = button.spriteState;
        if (spriteState.pressedSprite != sprite)
        {
            spriteState.pressedSprite = sprite;
            button.spriteState = spriteState;
            Debug.Log("btn pressed sprite updated : " + pressedUrl);
        }

        updatePressedProgress(1);
        return true;
    }

    public bool updateSelectedSprite(Sprite sprite)
    {
        if (sprite == null)
        {
            AssetsLoadManager.instance.requestSprite(selectedUrl, updateSelectedSprite, updateSelectedProgress);
            Debug.Log("btn selected sprite invalid : " + selectedUrl);
            return false;
        }

        Button button = GetComponent<Button>();
        SpriteState spriteState = button.spriteState;
        if (spriteState.selectedSprite != sprite)
        {
            spriteState.selectedSprite = sprite;
            button.spriteState = spriteState;
            Debug.Log("btn selected sprite updated : " + selectedUrl);
        }

        updateSelectedProgress(1);
        return true;
    }

    public bool updateDisabledSprite(Sprite sprite)
    {
        if (sprite == null)
        {
            AssetsLoadManager.instance.requestSprite(disabledUrl, updateDisabledSprite, updateDisabledProgress);
            Debug.Log("btn disabled sprite invalid : " + disabledUrl);
            return false;
        }

        Button button = GetComponent<Button>();
        SpriteState spriteState = button.spriteState;
        if (spriteState.disabledSprite != sprite)
        {
            spriteState.disabledSprite = sprite;
            button.spriteState = spriteState;
            Debug.Log("btn disabled sprite updated : " + disabledUrl);
        }

        updateDisabledProgress(1);
        return true;
    }

    public override float getProgressMax()
    {
        return maxProgress;
    }

    public override void resetAll()
    {
        Button button = GetComponent<Button>();
        SpriteState spriteState = button.spriteState;
        spriteState.highlightedSprite = null;
        spriteState.pressedSprite = null;
        spriteState.selectedSprite = null;
        spriteState.disabledSprite = null;
        button.spriteState = spriteState;

        highlightProgress = pressedProgress = selectedProgress = disabledProgress = maxProgress = 0;
    }

    public bool updateHighlightProgress(float _progress)
    {
        highlightProgress = _progress;
        return true;
    }

    public bool updatePressedProgress(float _progress)
    {
        pressedProgress = _progress;
        return true;
    }

    public bool updateSelectedProgress(float _progress)
    {
        selectedProgress = _progress;
        return true;
    }

    public bool updateDisabledProgress(float _progress)
    {
        disabledProgress = _progress;
        return true;
    }


    public override string getLoadingTextKey()
    {
        return loadingInfoTextKey;
    }

    public override float getProgressCurrent()
    {
        return highlightProgress + pressedProgress + selectedProgress + disabledProgress;
    }
}