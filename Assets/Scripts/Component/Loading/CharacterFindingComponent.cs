using UnityEngine;
using UnityEditor;
using System.Collections;

public class CharacterFindingComponent : LoadingComponent
{
    string loadingInfoTextKey = "ID_FINDING_CHARACTER";

    public override void startLoading()
    {
    }
    
    public override bool isLoadingCompleted()
    {
        return CharacterManager.instance.isCharacterFindingCompleted();
    }

    public override string getLoadingTextKey()
    {
        return loadingInfoTextKey;
    }
}