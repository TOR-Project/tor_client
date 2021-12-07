using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;
using Newtonsoft.Json;

public class EditorContractCommunicator : IContractCommunicator
{
    private ContractManager mContractManager;

    public EditorContractCommunicator(ContractManager cm)
    {
        mContractManager = cm;
        mContractManager.readyToUnityInstance();
    }

    public void reqConnectWallet()
    {
        mContractManager.StartCoroutine(progConnectWallet());
    }

    private IEnumerator progConnectWallet()
    {
        yield return new WaitForSeconds(1);

        bool serverBlocked = false;
        bool userBanned = false;
        bool versionMismatched = false;
        bool networkMismatched = false;
        bool walletConnectFailed = false;

        int errCode = 0;
        if (serverBlocked)
        {
            errCode = 1;
        } else if (userBanned)
        {
            errCode = 2;
        } else if (versionMismatched)
        {
            errCode = 3;
        } else if (networkMismatched)
        {
            errCode = 4;
        }

        resConnectWallet("0x0000000000000000", errCode);
    }

    public void resConnectWallet(string addr, int err)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["addr"] = addr;
        data["err"] = err;
        var values = JsonConvert.SerializeObject(data);
        Debug.Log(values);
        mContractManager.resConnectWallet(values);
    }


    public void reqLatestNotice()
    {
        mContractManager.StartCoroutine(progLatestNotice());
    }

    private IEnumerator progLatestNotice()
    {
        yield return new WaitForSeconds(1);

        resLatestNotice("TOR의 세계에 오신 것을 환영합니다.", 1638792000, "안녕하세요. Crow입니다.\nTOR의 세계에 오신 모험가 분들을 진심으로 환영합니다.\n즐거운 모험 되시길 바랍니다.");
    }

    public void resLatestNotice(string title, long date, string contents)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["title"] = title;
        data["date"] = date;
        data["contents"] = contents;
        var values = JsonConvert.SerializeObject(data);
        Debug.Log(values);
        mContractManager.resLatestNotice(values);
    }
}