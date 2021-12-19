
public interface IContractCommunicator
{
    void printLog(string _log);

    void reqConnectWallet();

    void reqLatestNotice();

    void reqLoginInfomation(string _addr);

    void reqAgreeTerms(int _ver);

    void reqUsingToken();

    void reqUsingNFT();

    void reqCheckRedundancy(string _nickname);

    void reqCreateUser(string _nickname);

}