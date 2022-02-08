using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Numerics;

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

    public void reqGetCommentLast(int _novelId, int _count)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["count"] = _count;
        data["novelId"] = _novelId;
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqGetCommentLast", values);
    }

    public void reqSendComment(int _novelId, string _mainTitle, string _comment)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["mainTitle"] = _mainTitle;
        data["novelId"] = _novelId;
        data["comment"] = _comment;
        data["address"] = UserManager.instance.getWalletAddress();
        data["nickname"] = UserManager.instance.getNickname();
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqSendComment", values);
    }

    public void reqGetComment(int _novelId, int _fromCommentId, int _count)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["count"] = _count;
        data["fromCommentId"] = _fromCommentId;
        data["novelId"] = _novelId;
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqGetComment", values);
    }

    public void reqCountryData(int _cid)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = UserManager.instance.getWalletAddress();
        data["cid"] = _cid;
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqCountryData", values);
    }

    public void reqDonate(int _cid, BigInteger _value)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = UserManager.instance.getWalletAddress();
        data["cid"] = _cid;
        data["value"] = _value.ToString();
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqDonate", values);
    }

    public void reqSetMiningTax(int _cid, int _tax)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = UserManager.instance.getWalletAddress();
        data["cid"] = _cid;
        data["tax"] = _tax;
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqSetMiningTax", values);
    }

    public void reqDepositMonarchSafe(int _cid, BigInteger _value)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = UserManager.instance.getWalletAddress();
        data["cid"] = _cid;
        data["value"] = _value.ToString();
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqDepositMonarchSafe", values);
    }

    public void reqWithdrawMonarchSafe(int _cid, BigInteger _value)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = UserManager.instance.getWalletAddress();
        data["cid"] = _cid;
        data["value"] = _value.ToString();
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqWithdrawMonarchSafe", values);
    }

    public void reqMoreLogData(int _cid, int _fromId, int _count)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = UserManager.instance.getWalletAddress();
        data["cid"] = _cid;
        data["fromId"] = _fromId;
        data["count"] = _count;
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqMoreLogData", values);
    }

    public void reqRoundCandidateList(int _round)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = UserManager.instance.getWalletAddress();
        data["round"] = _round;
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqRoundCandidateList", values);
    }

    public void addCandidateData(CandidateData _data)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = UserManager.instance.getWalletAddress();
        data["round"] = _data.round;
        data["tokenId"] = _data.tokenId;
        data["country"] = _data.countryId;
        data["title"] = _data.title;
        data["contents"] = _data.contents;
        data["url"] = _data.url;
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqAddCandidateData", values);
    }

    public void editCandidateData(CandidateData _data)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = UserManager.instance.getWalletAddress();
        data["id"] = _data.id;
        data["country"] = _data.countryId;
        data["round"] = _data.round;

        data["tokenId"] = _data.tokenId;
        data["title"] = _data.title;
        data["contents"] = _data.contents;
        data["url"] = _data.url;
        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqEditCandidateData", values);
    }

    public void cancelCandidateData(CandidateData _data)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = UserManager.instance.getWalletAddress();
        data["id"] = _data.id;
        data["country"] = _data.countryId;
        data["round"] = _data.round;

        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqCancelCandidateData", values);
    }

    public void appointmentCandidateData(CandidateData _data)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = UserManager.instance.getWalletAddress();
        data["id"] = _data.id;
        data["country"] = _data.countryId;
        data["round"] = _data.round;

        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqAppointmentCandidateData", values);
    }

    public void returnCandidateData(CandidateData _data)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = UserManager.instance.getWalletAddress();
        data["id"] = _data.id;
        data["country"] = _data.countryId;
        data["round"] = _data.round;

        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqReturnCandidateData", values);
    }

    public void reqNotVotedCharacterList(int _round, int[] _list)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = UserManager.instance.getWalletAddress();
        data["round"] = _round;
        data["list"] = _list;

        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqNotVotedCharacterList", values);
    }

    public void reqVoteMonarchElection(int _round, int[] _candidateIds, int[] _voteCount, int[] _idList)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = UserManager.instance.getWalletAddress();
        data["round"] = _round;
        data["candidateIds"] = _candidateIds;
        data["voteCount"] = _voteCount;
        data["idList"] = _idList;

        var values = JsonConvert.SerializeObject(data);

        Application.ExternalCall("reqVoteMonarchElection", values);
    }

    public void reqConstantValues()
    {
        Application.ExternalCall("reqConstantValues", "");
    }

    public void reqAgendaListCount()
    {
        Application.ExternalCall("reqAgendaListCount", "");
    }

    public void reqAgendaList(int[] _myCharacterTokenIdList)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = UserManager.instance.getWalletAddress();
        data["idList"] = _myCharacterTokenIdList;

        var values = JsonConvert.SerializeObject(data);
        Application.ExternalCall("reqAgendaListCount", values);
    }

    public void reqOfferAgenda(AgendaData _agendaData)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = UserManager.instance.getWalletAddress();
        data["agenda"] = _agendaData.generateData();

        var values = JsonConvert.SerializeObject(data); 
        Application.ExternalCall("reqOfferAgenda", values);
    }

    public void reqCancelAgenda(AgendaData _agendaData)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = UserManager.instance.getWalletAddress();
        data["agenda"] = _agendaData.generateData();

        var values = JsonConvert.SerializeObject(data); 
        Application.ExternalCall("reqCancelAgenda", values);
    }

    public void reqReturnCharacterFromAgenda(AgendaData _agendaData)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = UserManager.instance.getWalletAddress();
        data["agenda"] = _agendaData.generateData();

        var values = JsonConvert.SerializeObject(data); 
        Application.ExternalCall("reqReturnCharacterFromAgenda", values);
    }

    public void reqVoteAgenda(int _selectedIdx, AgendaData _agendaData)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["address"] = UserManager.instance.getWalletAddress();
        data["selectedIdx"] = _selectedIdx;
        data["agenda"] = _agendaData.generateData();

        var values = JsonConvert.SerializeObject(data);
        Application.ExternalCall("reqVoteAgenda", values);
    }
}