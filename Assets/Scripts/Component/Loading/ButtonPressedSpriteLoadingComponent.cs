using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ButtonPressedSpriteLoadingComponent : LoadingComponent
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
            ContractManager.instance.printLog("btn sprite invalid : " + url);
            return false;
        }

        Button button = GetComponent<Button>();
        SpriteState spriteState = button.spriteState;
        if (spriteState.pressedSprite != sprite)
        {
            spriteState.pressedSprite = sprite;
            button.spriteState = spriteState;
            ContractManager.instance.printLog("btn sprite updated : " + url);
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