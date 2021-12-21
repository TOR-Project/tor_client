using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Newtonsoft.Json;

public class WebContractCommunicator : IContractCommunicator
{
    private ContractManager mContractManager;

    public WebContractCommunicator(ContractManager cm)
    {
        mContractManager = cm;
    }

    public void printLog(string log)
    {
        Application.ExternalCall("printLog", log);
    }

    public void reqConnectWallet()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["version"] = Application.version;
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("connectWallet", values);
    }

    public void reqLatestNotice()
    {
        Application.ExternalCall("reqLatestNotice", null);
    }

    public void reqLoginInfomation(string addr)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = addr;
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqLoginInfomation", values);
    }

    public void reqAgreeTerms(int _ver)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = UserManager.instance.getWalletAddress();
        data["termsVer"] = _ver;
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqAgreeTerms", values);
    }

    public void reqUsingToken()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = UserManager.instance.getWalletAddress();
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqUsingToken", values);
    }

    public void reqUsingNFT()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = UserManager.instance.getWalletAddress();
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqUsingNFT", values);
    }

    public void reqCheckRedundancy(string _nickname)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["nickname"] = _nickname;
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqCheckRedundancy", values);
    }

    public void reqCreateUser(string _nickname, int _ver)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = UserManager.instance.getWalletAddress();
        data["termsVer"] = _ver;
        data["nickname"] = _nickname;
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqCreateUser", values);
    }

    public void reqCoinAmount()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = UserManager.instance.getWalletAddress();
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqCoinAmount", values);
    }
}