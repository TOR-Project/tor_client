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

    public void reqBlockNumber()
    {
        Application.ExternalCall("reqBlockNumber", null);
    }

    public void reqConnectedWalletAddr()
    {
        Application.ExternalCall("reqConnectedWalletAddr", null);
    }

    public void reqServerState()
    {
        Application.ExternalCall("reqServerState", null);
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
        data["address"] = UserManager.instance.getWalletAddress();
        data["nickname"] = _nickname;
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqCheckRedundancy", values);
    }

    public void reqRegistTokenToWallet()
    {
        Application.ExternalCall("reqRegistTokenToWallet", "");
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

    public void reqCharacterCount()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = UserManager.instance.getWalletAddress();
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqCharacterCount", values);
    }

    public void reqCharacterList(int _characterCount)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = UserManager.instance.getWalletAddress();
        data["characterCount"] = _characterCount;
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqCharacterList", values);
    }

    public void reqNotInitCharacterList()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = UserManager.instance.getWalletAddress();
        data["version"] = 1;
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqNotInitCharacterList", values);
    }

    public void reqInitCharacter(int[] _idList, int[] _characterDataList, int[] _statusDataList, int[] _equipDataList)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["idList"] = _idList;
        data["characterDataList"] = _characterDataList;
        data["statusDataList"] = _statusDataList;
        data["equipDataList"] = _equipDataList;
        data["address"] = UserManager.instance.getWalletAddress();
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqInitCharacter", values);
    }

    public void reqCharacterData(int[] _characterIdList)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["characterIdList"] = _characterIdList;
        data["address"] = UserManager.instance.getWalletAddress();
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqCharacterData", values);
    }

    public void reqStakingData(int[] _idList)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["idList"] = _idList;
        data["address"] = UserManager.instance.getWalletAddress();
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqStakingData", values);
    }

    public void reqAddMiningStaking(int[] _idList)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["idList"] = _idList;
        data["address"] = UserManager.instance.getWalletAddress();
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqAddMiningStaking", values);
    }

    public void reqGetBackMiningStaking(int[] _idList)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["idList"] = _idList;
        data["address"] = UserManager.instance.getWalletAddress();
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqGetBackMiningStaking", values);
    }

    public void reqReceiveMiningAmount(int[] _idList, string[] _countryTax, string _finalAmount, string _commissionAmount, int _password)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["idList"] = _idList;
        data["countryTax"] = _countryTax;
        data["finalAmount"] = _finalAmount;
        data["commissionAmount"] = _commissionAmount;
        data["password"] = _password;
        data["address"] = UserManager.instance.getWalletAddress();
        var values = JsonConvert.SerializeObject(data);

        Debug.Log(values);
        Application.ExternalCall("reqReceiveMiningAmount", values);
    }

    public void reqCalculateMiningAmount(int _id)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["id"] = _id;
        data["address"] = UserManager.instance.getWalletAddress();
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqCalculateMiningAmount", values);
    }

    public void reqGetPassword()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = UserManager.instance.getWalletAddress();
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqGetPassword", values);
    }

    public void reqGetStorySummery(int _id)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = UserManager.instance.getWalletAddress();
        data["id"] = _id;
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqGetStorySummery", values);
    }

    public void reqGetStoryCount()
    {
        Application.ExternalCall("reqGetStoryCount", "");
    }

    public void reqSubscribeStory(int _id)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = UserManager.instance.getWalletAddress();
        data["id"] = _id;
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqSubscribeStory", values);
    }

    public void reqGetStoryDataFull(int _id)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = UserManager.instance.getWalletAddress();
        data["id"] = _id;
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqGetStoryDataFull", values);
    }
}