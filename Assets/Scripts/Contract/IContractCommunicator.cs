
using System.Numerics;

public interface IContractCommunicator
{
    void printLog(string _log);

    void reqBlockNumber();

    void reqConnectedWalletAddr();

    void reqServerState();

    void reqConnectWallet();

    void reqLatestNotice();

    void reqLoginInfomation(string _addr);

    void reqAgreeTerms(int _ver);

    void reqUsingToken();

    void reqUsingNFT();

    void reqCheckRedundancy(string _nickname);

    void reqRegistTokenToWallet();

    void reqCreateUser(string _nickname, int _ver);

    void reqCoinAmount();

    void reqCharacterCount();

    void reqCharacterList(int _characterCount);

    void reqNotInitCharacterList();

    void reqInitCharacter(int[] _idList, int[] _characterDataList, int[] _statusDataList, int[] _equipDataList);

    void reqCharacterData(int[] _characterIdList);

    void reqStakingData(int[] _idList);

    void reqAddMiningStaking(int[] _idList);

    void reqGetBackMiningStaking(int[] _idList);

    void reqReceiveMiningAmount(int[] _idList, string[] _countryTax, string _finalAmount, string _commissionAmount, int _password);

    void reqCalculateMiningAmount(int _id);

    void reqGetPassword();

    void reqGetStorySummery(int _id);

    void reqGetStoryCount();

    void reqSubscribeStory(int _id);

    void reqGetStoryDataFull(int _id);

    void reqGetCommentLast(int _novelId, int _count);

    void reqGetComment(int _novelId, int _fromCommentId, int _count);

    void reqSendComment(int _novelId, string _mainTitle, string _comment);

    void reqCountryData(int _cid);

    void reqDonate(int _cid, BigInteger _value);

    void reqSetMiningTax(int _cid, int _tax);

    void reqDepositMonarchSafe(int _cid, BigInteger _value);

    void reqWithdrawMonarchSafe(int _cid, BigInteger _value);

    void reqMoreLogData(int _cid, int _fromId, int _count);

    void reqRoundCandidateList(int _round);

    void addCandidateData(CandidateData _data);

    void editCandidateData(CandidateData _data);

    void cancelCandidateData(CandidateData _data);

    void appointmentCandidateData(CandidateData _data);

    void returnCandidateData(CandidateData _data);

    void reqNotVotedCharacterList(int _round, int[] _list);

    void reqVoteMonarchElection(int _round, int[] _candidateIds, int[] _voteCounts, int[] _idList);

    void reqRoundRebellionList(int _round);
    void addRebellionData(RebellionData _data);
    void revolutionRebellionData(RebellionData _data);
    void returnRebellionData(RebellionData _data);
    void reqJoinRebellion(RebellionData _data, bool _isRebel);

    void reqConstantValues();

    void reqAgendaListCount();

    void reqAgendaList(int[] _myCharacterTokenIdList);

    void reqOfferAgenda(AgendaData _agendaData);

    void reqCancelAgenda(AgendaData _agendaData);

    void reqReturnCharacterFromAgenda(AgendaData _agendaData);

    void reqVoteAgenda(int _selectedIdx, AgendaData _agendaData);
    void reqSellItemList();
    void reqInventoryItemList();

}