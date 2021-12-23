﻿using UnityEngine;
using UnityEditor;
using System.Collections;

public class CharacterLoadingComponent : LoadingComponent
{
    string loadingInfoTextKey = "ID_LOADING_CHARACTER";

    public override void startLoading()
    {
        CharacterManager.instance.loadCharacter();
    }

    public override bool isLoadingCompleted()
    {
        return CharacterManager.instance.loadingStep >= 2;
    }

    public override string getLoadingTextKey()
    {
        return loadingInfoTextKey;
    }
}