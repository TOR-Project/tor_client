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
        return loadingInfoTextKey[getProgressCurrent()];
    }

    public override int getProgressMax()
    {
        return 5;
    }

    public override int getProgressCurrent()
    {
        return CharacterManager.instance.loadingStep;
    }
}