using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using UnityEngine;
using UnityEngine.Networking;

public class ContractManager : MonoBehaviour
{
    private IContractCommunicator mContractCommunicator;

    private bool unityInstanceLoaded = false;

    [SerializeField]
    LoginWindowController loginController;
    [SerializeField]
    LoadingWindowController loadingController;
    [SerializeField]
    TermsWindowController termsController;
    [SerializeField]
    MiningWindowController miningController;

    [SerializeField]
    GlobalUIWindowController globalUIController;

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

    public void resTransactionError(string _err)
    {
        globalUIController.hideLoading();
        globalUIController.showPopup(_err, null);
    }

    public void reqConnectWallet()
    {
        globalUIController.showLoading();
        Debug.Log("reqConnectWallet()");
        mContractCommunicator.reqConnectWallet();
    }

    public void resConnectWallet(string _json)
    {
        globalUIController.hideLoading();
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);

        int err = int.Parse(values["err"].ToString());

        Debug.Log("resConnectWallet() json = " + _json);
        if (err != Const.NO_ERROR)
        {
            globalUIController.showErrorPopup(err);
        }
        else
        {
            string addr = values["addr"].ToString();

            loginController.connectWallet(addr);
            UserManager.instance.setWalletAddress(addr);
        }
    }

    public void reqLatestNotice()
    {
        Debug.Log("reqLatestNotice()");
        mContractCommunicator.reqLatestNotice();
    }

    public void resLatestNotice(string _json)
    {
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);

        string title = values["title"].ToString();
        long date =  long.Parse(values["date"].ToString());
        string contents = values["contents"].ToString();

        Debug.Log("resLatestNotice() json = " + _json);
        loginController.displayNotice(title, date, contents);
    }

    public void reqLoginInfomation(string addr)
    {
        globalUIController.showLoading();
        Debug.Log("reqLoginInfomation()");
        mContractCommunicator.reqLoginInfomation(addr);
    }

    public void resLoginInfomation(string _json)
    {
        globalUIController.hideLoading();
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);

        int err = int.Parse(values["err"].ToString());

        Debug.Log("resLoginInfomation() json = " + _json);
        if (err == Const.ERR_USER_BANNED)
        {
            long startBlock = long.Parse(values["startBlock"].ToString());
            long endBlock = long.Parse(values["endBlock"].ToString());
            string reason = values["reason"].ToString();
            globalUIController.showBanPopup(startBlock, endBlock, reason);
        }
        else if (err != Const.NO_ERROR)
        {
            globalUIController.showErrorPopup(err);
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
        }
    }

    public void reqAgreeTerms(int _ver)
    {
        globalUIController.showLoading();
        Debug.Log("reqAgreeTerms() ver = " + _ver);
        mContractCommunicator.reqAgreeTerms(_ver);
    }

    public void resAgreeTerms(string _json)
    {
        globalUIController.hideLoading();
        Debug.Log("resAgreeTerms()");
        UserManager.instance.setTermsVer(Const.TERMS_VERSION);
        termsController.resAgreeTerms();
    }

    public void reqUsingToken()
    {
        globalUIController.showLoading();
        Debug.Log("reqUsingToken()");
        mContractCommunicator.reqUsingToken();
    }

    public void resUsingToken(string _json)
    {
        globalUIController.hideLoading();
        Debug.Log("resUsingToken()");
        UserManager.instance.setTokenUsing(true);
        termsController.resUsingTokenConfirm();
    }

    public void reqUsingNFT()
    {
        globalUIController.showLoading();
        Debug.Log("reqUsingNFT()");
        mContractCommunicator.reqUsingNFT();
    }

    public void resUsingNFT(string _json)
    {
        globalUIController.hideLoading();
        Debug.Log("resUsingNFT()");
        UserManager.instance.setNFTUsing(true);
        termsController.resUsingNFTConfirm();
    }

    public void reqCheckRedundancy(string _nickname)
    {
        globalUIController.showLoading();
        Debug.Log("reqCheckRedundancy() nickname = " + _nickname);
        mContractCommunicator.reqCheckRedundancy(_nickname);
    }

    public void resCheckRedundancy(string _json)
    {
        globalUIController.hideLoading();
        Debug.Log("resCheckRedundancy() json = " + _json);

        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);

        bool available = bool.Parse(values["available"].ToString());

        termsController.resCheckResundancy(available);
    }

    public void reqRegistTokenToWallet()
    {
        globalUIController.showLoading();
        Debug.Log("reqRegistTokenToWallet()");
        mContractCommunicator.reqRegistTokenToWallet();
    }

    public void resRegistTokenToWallet(string _json)
    {
        globalUIController.hideLoading();
        Debug.Log("resRegistTokenToWallet()");

        termsController.resRegistTokenToWallet();
    }

    public void reqCreateUser(string _nickname)
    {
        globalUIController.showLoading();
        Debug.Log("reqCreateUser() nickname = " + _nickname);
        mContractCommunicator.reqCreateUser(_nickname, Const.TERMS_VERSION);
    }

    public void resCreateUser(string _json)
    {
        globalUIController.hideLoading();
        Debug.Log("resCreateUser() json = " + _json);
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);

        string nickname = values["nickname"].ToString();

        UserManager.instance.setNickname(nickname);
        termsController.resCreateUser();
    }

    public void reqCoinAmount()
    {
        Debug.Log("reqCoinAmount()");
        mContractCommunicator.reqCoinAmount();
    }

    public void resCoinAmount(string _json)
    {
        Debug.Log("resCoinAmount() json = " + _json);

        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);

        BigInteger amount = BigInteger.Parse(values["amount"].ToString());

        UserManager.instance.setCoinAmount(amount);
    }

    public void reqCharacterCount()
    {
        Debug.Log("reqCharacterCount()");
        mContractCommunicator.reqCharacterCount();
    }

    public void resCharacterCount(string _json)
    {
        Debug.Log("resCharacterCount() json = " + _json);

        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);

        int count = int.Parse(values["characterCount"].ToString());
        int stakingCount = int.Parse(values["stakingCharacterCount"].ToString());
        CharacterManager.instance.setCharacterCount(count, stakingCount);
    }

    public void reqCharacterList(int _characterCount)
    {
        Debug.Log("reqCharacterList()");
        mContractCommunicator.reqCharacterList(_characterCount);
    }

    public void resCharacterList(string _json)
    {
        Debug.Log("resCharacterList() json = " + _json);

        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);
        int[] characterIdList = JsonConvert.DeserializeObject<int[]>(values["characterIdList"].ToString());
        int[] stakingCharacterIdList = JsonConvert.DeserializeObject<int[]>(values["stakingCharacterIdList"].ToString());
        CharacterManager.instance.setCharacterIdList(characterIdList, stakingCharacterIdList);
    }

    public void reqNotInitCharacterList()
    {
        Debug.Log("reqNotInitCharacterList()");
        mContractCommunicator.reqNotInitCharacterList();
    }

    public void resNotInitCharacterList(string _json)
    {
        Debug.Log("resNotInitCharacterList() json = " + _json);

        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);
        int[] characterIdList = JsonConvert.DeserializeObject<int[]>(values["characterIdList"].ToString());

        Debug.Log("resNotInitCharacterList() length = " + characterIdList.Length);

        CharacterManager.instance.setNotInitCharacterIdList(characterIdList);
    }

    public void reqInitCharacter(int[] _idList, int[] _characterDataList, int[] _statusDataList, int[] _equipDataList)
    {
        globalUIController.showLoading();
        Debug.Log("reqInitCharacter() size = " + _idList.Length);

        mContractCommunicator.reqInitCharacter(_idList, _characterDataList, _statusDataList, _equipDataList);
    }

    public void resInitCharacter(string _json)
    {
        globalUIController.hideLoading();
        Debug.Log("resInitCharacter()");

        CharacterManager.instance.initCharacterCompleted();
    }

    public void reqCharacterData(int[] _characterIdList)
    {
        Debug.Log("reqCharacterData() size = " + _characterIdList.Length);

        mContractCommunicator.reqCharacterData(_characterIdList);
    }

    public void resCharacterData(string _json)
    {
        Debug.Log("resCharacterData() json = " + _json);

        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);
        var characterData = JsonConvert.DeserializeObject < Dictionary<string, object> > (values["characterData"].ToString());
        var statusData = JsonConvert.DeserializeObject<Dictionary<string, object>>(values["statusData"].ToString());
        var equipData = JsonConvert.DeserializeObject<Dictionary<string, object>>(values["equipData"].ToString());

        CharacterManager.instance.parsingCharacterData(characterData, statusData, equipData);
    }

    public void reqStakingData(int[] _idList)
    {
        Debug.Log("reqStakingData()");

        mContractCommunicator.reqStakingData(_idList);
    }

    public void resStakingData(string _json)
    {
        // Debug.Log("resStakingData() json = " + _json);

        CharacterManager.instance.parsingStakingData(JsonConvert.DeserializeObject<Dictionary<string, object>>(_json));
    }

    public void reqAddMiningStaking(int[] _idList)
    {
        globalUIController.showLoading();

        Debug.Log("reqAddMiningStaking() size = " + _idList.Length);

        mContractCommunicator.reqAddMiningStaking(_idList);
    }

    public void resAddMiningStaking(string _json)
    {
        globalUIController.hideLoading();

        Debug.Log("resAddMiningStaking()");

        miningController.resAddMiningStaking();
    }

    public void reqGetBackMiningStaking(int[] _idList)
    {
        globalUIController.showLoading();

        Debug.Log("reqGetBackMiningStaking() size = " + _idList.Length);

        mContractCommunicator.reqGetBackMiningStaking(_idList);
    }

    public void resGetBackMiningStaking(string _json)
    {
        globalUIController.hideLoading();

        Debug.Log("resGetBackMiningStaking()");

        miningController.resGetBackMiningStaking();
    }

    public void reqReceiveMiningAmount(int[] _idList, string[] _countryTax, string _finalAmount, string _commissionAmount, int _password)
    {
        globalUIController.showLoading();

        Debug.Log("reqReceiveMiningAmount() size = " + _idList.Length);

        mContractCommunicator.reqReceiveMiningAmount(_idList, _countryTax, _finalAmount, _commissionAmount, _password);
    }

    public void resReceiveMiningAmount(string _json)
    {
        globalUIController.hideLoading();

        Debug.Log("resReceiveMiningAmount()");

        miningController.resReceiveMiningAmount();
    }

    public void reqCalculateMiningAmount(int _id)
    {
        // Debug.Log("reqCalculateMiningAmount()");

        mContractCommunicator.reqCalculateMiningAmount(_id);
    }

    public void resCalculateMiningAmount(string _json)
    {
        // Debug.Log("resCalculateMiningAmount() json = " + _json);

        MiningManager.instance.resMiningAmount(JsonConvert.DeserializeObject<Dictionary<string, object>>(_json));
    }

    public void reqGetPassword()
    {
        Debug.Log("reqGetPassword()");

        mContractCommunicator.reqGetPassword();
    }

    public void resGetPassword(string _json)
    {
        Debug.Log("resGetPassword()");

        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);

        int password = int.Parse(values["password"].ToString());
        UserManager.instance.setPassword(password);
    }
}
