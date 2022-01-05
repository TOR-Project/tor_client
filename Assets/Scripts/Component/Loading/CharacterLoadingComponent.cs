using UnityEngine;
using UnityEditor;
using System.Collections;

public class CharacterLoadingComponent : LoadingComponent
{
    string[] loadingInfoTextKey = { "", "ID_LOADING_CHARACTER", "ID_FINDING_CHARACTER", "ID_CHECKING_CHARACTER", "ID_STAKING_CHARACTER" };

    public override void startLoading()
    {
        CharacterManager.instance.loadCharacter();
    }

    public override string getLoadingTextKey()
    {
        return loadingInfoTextKey[(int)(getProgressCurrent())];
    }

    public override float getProgressMax()
    {
        return 5;
    }

    public override float getProgressCurrent()
    {
        return CharacterManager.instance.loadingStep;
    }
}