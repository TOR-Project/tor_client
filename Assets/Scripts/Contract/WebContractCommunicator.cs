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
}