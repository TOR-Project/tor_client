
public interface IContractCommunicator
{
    void reqConnectWallet();
    void resConnectWallet(string addr, int err);

    void reqLatestNotice();
    void resLatestNotice(string title, long date, string contents);
}