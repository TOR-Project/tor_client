using UnityEngine;
using UnityEditor;
using System.Collections;

public class CharacterStakingComponent : LoadingComponent
{
    string loadingInfoTextKey = "ID_STAKING_CHARACTER";

    public override void startLoading()
    {
    }

    public override bool isLoadingCompleted()
    {
        return CharacterManager.instance.loadingStep >= 5;
    }

    public override string getLoadingTextKey()
    {
        return loadingInfoTextKey;
    }
}