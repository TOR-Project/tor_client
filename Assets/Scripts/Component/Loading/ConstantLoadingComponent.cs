using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ConstantLoadingComponent : LoadingComponent
{
    string loadingInfoTextKey = "ID_CONSTANT_LOADING";

    public override void startLoading()
    {
        ContractManager.instance.reqConstantValues();
    }

    public override string getLoadingTextKey()
    {
        return loadingInfoTextKey;
    }

    public override float getProgressCurrent()
    {
        return Const.CONSTANT_LOADED ? 1 : 0;
    }
}