
public interface IContractCommunicator
{
    void printLog(string log);

    void reqConnectWallet();

    void reqLatestNotice();

    void reqLoginInfomation(string addr);
}