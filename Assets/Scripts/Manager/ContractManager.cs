using Newtonsoft.Json;
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
            loginController.showErrorPopup(err);
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
        if (err != 0)
        {
            loginController.showErrorPopup(err);
        }
        else
        {
            object userInfo = values["userInfo"].ToString();
            int[] characterIdList = JsonConvert.DeserializeObject<int[]>(values["characterIdList"].ToString());
            CharacterManager.instance.loadCharacter(characterIdList, () => loginController.enterTitlePage());
        }
    }
}
