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
    private Dictionary<string, bool> contractLoadedMap = new Dictionary<string, bool>();

    [SerializeField]
    LoginWindowController loginController;
    [SerializeField]
    LoadingWindowController loadingController;
    [SerializeField]
    TermsWindowController termsController;
    [SerializeField]
    MiningWindowController miningController;
    [SerializeField]
    NovelWindowController novelController;
    [SerializeField]
    CastlePanelController castlePanelController;
    [SerializeField]
    ElectionOfficeWindowController electionOfficeController;
    [SerializeField]
    PollsPlaceController pollsPlaceController;
    [SerializeField]
    GovernanaceWindowController governanaceWindowController;

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

    public void readyToContract(string _name)
    {
        Debug.Log("readyToContract() " + _name);

        if (!contractLoadedMap.ContainsKey(_name))
        {
            contractLoadedMap[_name] = true;
        }
    }


    public bool isContractLoaded(string _name)
    {
        if (contractLoadedMap.ContainsKey(_name))
        {
            return contractLoadedMap[_name];
        }
        return false;
    }

    public void printLog(string log)
    {
        mContractCommunicator.printLog(log);
    }

    public void resTransactionError(string _err)
    {
        globalUIController.hideLoading();
        globalUIController.showAlertPopup(_err, null);
    }

    public void reqBlockNumber()
    {
        mContractCommunicator.reqBlockNumber();
    }

    public void resBlockNumber(string _json)
    {
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);

        long number = long.Parse(values["block"].ToString());
        SystemInfoManager.instance.setBlockNumber(number);
    }

    public void reqConnectedWalletAddr()
    {
        mContractCommunicator.reqConnectedWalletAddr();
    }

    public void resConnectedWalletAddr(string _json)
    {
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);

        string addr = values["address"].ToString();
        SystemInfoManager.instance.setConnectedWalletAddress(addr);
    }

    public void reqServerState()
    {
        mContractCommunicator.reqServerState();
    }

    public void resServerState(string _json)
    {
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);

        bool available = bool.Parse(values["available"].ToString());
        SystemInfoManager.instance.setServerState(available);
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
            bool hasUserDataLegacy = bool.Parse(values["hasUserDataLegacy"].ToString());
            bool isAdmin = bool.Parse(values["isAdmin"].ToString());
            bool latestTerms = false;
            bool tokenUsing = bool.Parse(values["tokenUsing"].ToString());
            bool nftUsing = bool.Parse(values["nftUsing"].ToString());
            bool needMigration = false;

            if (hasUserData)
            {
                string nickName = values["nickName"].ToString();
                int termsVersion = int.Parse(values["termsVersion"].ToString());
                latestTerms = termsVersion >= Const.TERMS_VERSION;
                string[] friends = JsonConvert.DeserializeObject<string[]>(values["friends"].ToString());

                UserManager.instance.setUserData(nickName, termsVersion, friends, tokenUsing, nftUsing, false, isAdmin);
            } else
            {
                string nickName = "";
                int termsVersion = 0;
                if (hasUserDataLegacy)
                {
                    needMigration = true;
                    nickName = values["nickNameLegacy"].ToString();
                    termsVersion = int.Parse(values["termsVersionLegacy"].ToString());
                }
                UserManager.instance.setUserData(nickName, termsVersion, new string[] { }, tokenUsing, nftUsing, needMigration, isAdmin);
            }

            loginController.enterNextPage(hasUserData, latestTerms, tokenUsing, nftUsing, needMigration);
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

    public void reqConstantValues()
    {
        Debug.Log("reqContantValues()");
        mContractCommunicator.reqConstantValues();
    }

    public void resConstantValues(string _json)
    {
        Debug.Log("resConstantValues() json = " + _json);
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);

        Const.START_BLOCK = long.Parse(values["startBlock"].ToString());
        Const.ELECTION_START_BLOCK = long.Parse(values["electionStartBlock"].ToString());
        Const.SUBSCRIBE_FEE = int.Parse(values["subscribeFee"].ToString());
        Const.MONARCH_REGIST_FEE = int.Parse(values["monarchRegistFee"].ToString());
        Const.MINING_TAX_SETTLING_DELAY = int.Parse(values["miningTaxSettlingDelay"].ToString());
        Const.CONSTANT_LOADED = true;
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
        mContractCommunicator.reqGetPassword();
    }

    public void resGetPassword(string _json)
    { 
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);

        int password = int.Parse(values["password"].ToString());
        UserManager.instance.setPassword(password);
    }

    public void reqGetStorySummery(int _id)
    {
        Debug.Log("reqGetStorySummery()");

        mContractCommunicator.reqGetStorySummery(_id);
    }

    public void resGetStorySummery(string _json)
    {
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);

        novelController.setNovelData(new NovelData(values));
    }

    public void reqGetStoryCount()
    {
        Debug.Log("reqGetStoryCount()");

        mContractCommunicator.reqGetStoryCount();
    }

    public void resGetStoryCount(string _json)
    {
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);

        int count = int.Parse(values["count"].ToString());
        novelController.setStoryCount(count);
    }

    public void reqSubscribeStory(int _id)
    {
        globalUIController.showLoading();

        Debug.Log("reqSubscribeStory()");

        mContractCommunicator.reqSubscribeStory(_id);
    }

    public void resSubscribeStory(string _json)
    {
        globalUIController.hideLoading();

        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);

        int id = int.Parse(values["id"].ToString());
        bool success = bool.Parse(values["success"].ToString());
        novelController.onSubscribingCompleted(id, success);
    }

    public void reqGetStoryDataFull(int _id)
    {
        Debug.Log("reqGetStoryDataFull()");

        mContractCommunicator.reqGetStoryDataFull(_id);
    }

    public void resGetStoryDataFull(string _json)
    {
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);

        bool success = bool.Parse(values["success"].ToString());
        
        int id = int.Parse(values["id"].ToString());
        string cotents = null;
        string illustrationUrl = null;

        if (success)
        {
            cotents = values["contents"].ToString();
            illustrationUrl = values["illustrationUrl"].ToString();
        }

        novelController.onFullDataLoaded(success, id, cotents, illustrationUrl);
    }

    public void reqGetCommentLast(int _novelId, int _count)
    {
        Debug.Log("reqGetCommentLast()");

        mContractCommunicator.reqGetCommentLast(_novelId, _count);
    }

    public void reqGetComment(int _novelId, int _fromCommentId, int _count)
    {
        Debug.Log("reqGetComment()");

        mContractCommunicator.reqGetComment(_novelId, _fromCommentId, _count);
    }

    public void resGetComment(string _json)
    {
        var values = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(_json);

        novelController.responseComment(values);
    }


    public void reqSendComment(int _novelId, string _mainTitle, string _comment)
    {
        Debug.Log("reqSendComment()");

        mContractCommunicator.reqSendComment(_novelId, _mainTitle, _comment);
    }

    internal void resSendComment(string _json)
    {
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);

        novelController.responseCommnetLatestOne(values);
    }

    internal void reqCountryData(int _cid)
    {
        Debug.Log("reqCountryData()");

        mContractCommunicator.reqCountryData(_cid);
    }

    internal void resCountryData(string _json)
    {
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);

        CountryManager.instance.responseCountyData(values);
    }

    internal void reqDonate(int _cid, BigInteger _value)
    {
        globalUIController.showLoading();
        Debug.Log("reqDonate()");

        mContractCommunicator.reqDonate(_cid, _value);
    }

    internal void resDonate(string _json)
    {
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);

        int cid = int.Parse(values["cid"].ToString());
        BigInteger value = BigInteger.Parse(values["value"].ToString());

        globalUIController.hideLoading();
        Debug.Log("resDonate()");

        reqCoinAmount();
        castlePanelController.onDonateSuccessed(cid, value);
    }

    internal void reqSetMiningTax(int _cid, int _tax)
    {
        globalUIController.showLoading();
        Debug.Log("reqSetMiningTax()");

        mContractCommunicator.reqSetMiningTax(_cid, _tax);
    }

    internal void resSetMiningTax(string _json)
    {
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);

        int cid = int.Parse(values["cid"].ToString());
        int tax = int.Parse(values["tax"].ToString());

        globalUIController.hideLoading();
        Debug.Log("resSetMiningTax()");

        castlePanelController.onMiningTaxSettingSuccessed(cid, tax);
    }

    internal void reqDepositMonarchSafe(int _cid, BigInteger _value)
    {
        globalUIController.showLoading();
        Debug.Log("reqDepositMonarchSafe()");

        mContractCommunicator.reqDepositMonarchSafe(_cid, _value);
    }

    internal void resDepositMonarchSafe(string _json)
    {
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);

        int cid = int.Parse(values["cid"].ToString());
        BigInteger value = BigInteger.Parse(values["value"].ToString());

        globalUIController.hideLoading();
        Debug.Log("resDepositMonarchSafe()");

        reqCoinAmount();
        castlePanelController.onDepositSuccssed(cid, value);
    }

    internal void reqWithdrawMonarchSafe(int _cid, BigInteger _value)
    {
        globalUIController.showLoading();
        Debug.Log("reqWithdrawMonarchSafe()");

        mContractCommunicator.reqWithdrawMonarchSafe(_cid, _value);
    }

    internal void resWithdrawMonarchSafe(string _json)
    {
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);

        int cid = int.Parse(values["cid"].ToString());
        BigInteger value = BigInteger.Parse(values["value"].ToString());

        globalUIController.hideLoading();
        Debug.Log("resWithdrawMonarchSafe()");

        reqCoinAmount();
        castlePanelController.onWithdrawSuccssed(cid, value);
    }

    internal void reqMoreLogData(int _cid, int _fromId, int _count)
    {
        Debug.Log("reqMoreLogData()");
        mContractCommunicator.reqMoreLogData(_cid, _fromId, _count);
    }

    internal void resMoreLogData(string _json)
    {
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);

        CountryManager.instance.responseLogData(values);
    }

    internal void reqRoundCandidateList(int _round)
    {
        Debug.Log("reqRoundCandidateList() " + _round);
        mContractCommunicator.reqRoundCandidateList(_round);
    }

    public void resRoundCandidateList(string _json)
    {
        globalUIController.hideLoading();

        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);

        ElectionManager.instance.responseRoundCandidateList(values);
    }

    internal void addCandidateData(CandidateData _data)
    {
        Debug.Log("addCandidateData()");
        mContractCommunicator.addCandidateData(_data);
    }
    
    internal void editCandidateData(CandidateData _data)
    {
        Debug.Log("editCandidateData()");
        mContractCommunicator.editCandidateData(_data);
    }

    internal void cancelCandidateData(CandidateData _data)
    {
        Debug.Log("cancelCandidateData()");
        mContractCommunicator.cancelCandidateData(_data);
    }

    internal void appointmentCandidateData(CandidateData _data)
    {
        Debug.Log("appointmentCandidateData()");
        mContractCommunicator.appointmentCandidateData(_data);
    }

    internal void returnCandidateData(CandidateData _data)
    {
        Debug.Log("returnCandidateData()");
        mContractCommunicator.returnCandidateData(_data);
    }

    public void resUpdateCandidateData(string _json)
    {
        globalUIController.hideLoading();

        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);
        CandidateData data = new CandidateData();
        data.parseData(values);

        ElectionManager.instance.updateCandidateData(data);
        electionOfficeController.updateCandidateData(data);

        reqCoinAmount();
    }

    internal void resAppointmentMonarch(string _json)
    {
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);
        CandidateData data = new CandidateData();
        data.parseData(values);

        electionOfficeController.responceAppointmentMonarch(data);
    }

    internal void reqNotVotedCharacterList(int _round, int[] _list)
    {
        Debug.Log("reqNotVotedCharacterList()");
        mContractCommunicator.reqNotVotedCharacterList(_round, _list);
    }

    public void resNotVotedCharacterList(string _json)
    {
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);
        int[] list = JsonConvert.DeserializeObject<int[]>(values["characterIdList"].ToString());

        ElectionManager.instance.responseNotVotedCharacterList(list);
    }

    public void reqVoteMonarchElection(int _round, int[] _candidateIds, int[] _voteCounts, int[] _idList)
    {
        Debug.Log("reqVoteMonarchElection()");
        mContractCommunicator.reqVoteMonarchElection(_round, _candidateIds, _voteCounts, _idList);
    }

    public void resVoteMonarchElection(string _json)
    {
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);
        int[] list = JsonConvert.DeserializeObject<int[]>(values["characterIdList"].ToString());

        pollsPlaceController.responceVoteCompleted(list);
    }

    internal void reqAgendaListCount()
    {
        Debug.Log("reqAgendaListCount()");
        mContractCommunicator.reqAgendaListCount();
    }

    internal void resAgendaListCount(string _json)
    {
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);
        int count = int.Parse(values["count"].ToString());
        GovernanceManager.instance.responseAgendaCount(count);
    }

    internal void reqAgendaList()
    {
        Debug.Log("reqAgendaList()");
        int[] myCharacterTokenIdList = CharacterManager.instance.getMyCharacterList().ConvertAll(cd => cd.tokenId).ToArray();
        mContractCommunicator.reqAgendaList(myCharacterTokenIdList);
    }

    internal void resAgendaList(string _json)
    {
        var values = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(_json);
        GovernanceManager.instance.responseAgendaList(values);
    }

    internal void reqOfferAgenda(AgendaData _agendaData)
    {
        Debug.Log("reqOfferAgenda()");
        mContractCommunicator.reqOfferAgenda(_agendaData);
    }

    internal void resOfferAgenda(string _json)
    {
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);

        AgendaData agendaData = new AgendaData();
        agendaData.parseData(values);

        GovernanceManager.instance.updateAgendaData(agendaData);
        governanaceWindowController.responseOfferAgenda(agendaData);
    }

    internal void reqCancelAgenda(AgendaData _agendaData)
    {
        Debug.Log("reqCancelAgenda()");
        mContractCommunicator.reqCancelAgenda(_agendaData);
    }

    internal void reqReturnCharacterFromAgenda(AgendaData _agendaData)
    {
        Debug.Log("reqReturnCharacterFromAgenda()");
        mContractCommunicator.reqReturnCharacterFromAgenda(_agendaData);
    }

    internal void reqVoteAgenda(int _selectedIdx, AgendaData _agendaData)
    {
        Debug.Log("reqVoteAgenda()");
        mContractCommunicator.reqVoteAgenda(_selectedIdx, _agendaData);
    }

    internal void resUpdateAgendaData(string _json)
    {
        var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(_json);

        AgendaData agendaData = new AgendaData();
        agendaData.parseData(values);

        GovernanceManager.instance.updateAgendaData(agendaData);
    }

}
