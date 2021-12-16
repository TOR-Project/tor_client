using UnityEngine;
using UnityEditor;
using System.Collections;

public class CharacterLoadingComponent : LoadingComponent
{
    string loadingInfoTextKey = "ID_LOADING_CHARACTER";

    public override void startLoading()
    {
    }

    public override bool isLoadingCompleted()
    {
        return CharacterManager.instance.isCharacterLoadingCompleted();
    }

    public override string getLoadingTextKey()
    {
        return loadingInfoTextKey;
    }
}