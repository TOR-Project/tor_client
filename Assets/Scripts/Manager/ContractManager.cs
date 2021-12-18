﻿using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class ContractManager : MonoBehaviour
{
    private IContractCommunicator mContractCommunicator;

    private bool unityInstanceLoaded = false;

    [SerializeField]
    LoginController loginController;
    [SerializeField]
    LoadingController loadingController;

    [SerializeField]
    GlobalPopupController globalPopupController;

    private static ContractManager mInstance;
    public static ContractManager instance {
        get {
            return mInstance;
        }
    }

    private ContractManager()
    {
        mInstance = this;

        if (Application.platform == RuntimePlatform.Android)
        {

            Debug.Log("Create AndroidContractCommunicator");
        }
        else if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            mContractCommunicator = new WebContractCommunicator(this);
            Debug.Log("Create WebContractCommunicator");
        }
        else
        {
            mContractCommunicator = new EditorContractCommunicator(this);
            Debug.Log("Create EditorContractCommunicator");
        }
    }

    public void readyToUnityInstance()
    {
        unityInstanceLoaded = true;
    }

    public bool isUnityInstanceLoaded()
    {
        return unityInstanceLoaded;
    }

    public void printLog(string log)
    {
        mContractCommunicator.printLog(log);
    }

    public void reqConnectWallet()
    {
        Debug.Log("reqConnectWallet()");
        loginController.showingConnectWalletLoading();
        mContractCommunicator.reqConnectWallet();
    }

    public void resConnectWallet(string json)
    {
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

        string addr = values["addr"].ToString();
        int err = int.Parse(values["err"].ToString());

        Debug.Log("resConnectWallet() json = " + json);
        if (err != 0)
        {
            globalPopupController.showErrorPopup(err);
        }
        else
        {
            loginController.connectWallet(addr);
        }
    }

    public void reqLatestNotice()
    {
        Debug.Log("reqLatestNotice()");
        mContractCommunicator.reqLatestNotice();
    }

    public void resLatestNotice(string json)
    {
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

        string title = values["title"].ToString();
        long date =  long.Parse(values["date"].ToString());
        string contents = values["contents"].ToString();

        Debug.Log("resLatestNotice() json = " + json);
        loginController.displayNotice(title, date, contents);
    }

    public void reqLoginInfomation(string addr)
    {
        Debug.Log("reqLoginInfomation()");
        loginController.showingConnectWalletLoading();
        mContractCommunicator.reqLoginInfomation(addr);
    }

    public void resLoginInfomation(string json)
    {
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

        int err = int.Parse(values["err"].ToString());

        Debug.Log("resLoginInfomation() json = " + json);
        if (err == 5)
        {
            long startBlock = long.Parse(values["startBlock"].ToString());
            long endBlock = long.Parse(values["endBlock"].ToString());
            string reason = values["reason"].ToString();
            globalPopupController.showBanPopup(startBlock, endBlock, reason);
        }
        else if (err != 0)
        { 
            globalPopupController.showErrorPopup(err);
        }
        else
        {
            bool hasUserData = bool.Parse(values["hasUserData"].ToString());
            bool latestTerms = false;
            bool tokenUsing = false;
            bool nftUsing = false;

            if (hasUserData)
            {
                string nickName = values["nickName"].ToString();
                int termsVersion = int.Parse(values["termsVersion"].ToString());
                latestTerms = termsVersion >= Const.TERMS_VERSION;
                string[] friends = JsonConvert.DeserializeObject<string[]>(values["friends"].ToString());

                tokenUsing = bool.Parse(values["tokenUsing"].ToString());
                nftUsing = bool.Parse(values["nftUsing"].ToString());

                UserManager.instance.setUserData(nickName, termsVersion, friends, tokenUsing, nftUsing);
            }

            loginController.enterNextPage(hasUserData, latestTerms, tokenUsing, nftUsing);
            int max = int.Parse(values["characterCount"].ToString());
            CharacterManager.instance.setCharacterCount(max);
        }
    }

    public void resFindingCharacter(string json)
    {
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

        int progress = int.Parse(values["progress"].ToString());

        CharacterManager.instance.setFoundCharacterCount(progress + 1);
    }

    public void resFoundCharacter(string json)
    {
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
        int[] characterIdList = JsonConvert.DeserializeObject<int[]>(values["characterIdList"].ToString());
        CharacterManager.instance.loadCharacter(characterIdList);
    }
}
