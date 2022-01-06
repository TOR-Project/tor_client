
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
}