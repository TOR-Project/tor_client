using UnityEngine;
using UnityEditor;
using System.Collections;

public class CharacterCheckingComponent : LoadingComponent
{
    string loadingInfoTextKey = "ID_CHECKING_CHARACTER";

    public override void startLoading()
    {
    }

    public override bool isLoadingCompleted()
    {
        return CharacterManager.instance.loadingStep >= 4;
    }

    public override string getLoadingTextKey()
    {
        return loadingInfoTextKey;
    }
}