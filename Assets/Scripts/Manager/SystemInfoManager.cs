using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SystemInfoManager : MonoBehaviour
{
    [SerializeField]
    public long blockNumber = 0;
    [SerializeField]
    public string connectedWalletAddress = "";
    [SerializeField]
    public bool serverAvailable = false;
    [SerializeField]
    bool blockNumberChecking = false;
    [SerializeField]
    bool walletAddressChecking = false;
    [SerializeField]
    bool serverStateChecking = false;
    float passingTime = 0;

    private List<BlockNumberObserever> blockNumberObserverList = new List<BlockNumberObserever>();
    private List<WalletAddressObserever> walletAddressObserverList = new List<WalletAddressObserever>();
    private List<ServerStateObserever> serverStateObserverList = new List<ServerStateObserever>();

    static SystemInfoManager mInstance;
    public static SystemInfoManager instance {
        get {
            return mInstance;
        }
    }

    private SystemInfoManager()
    {
        mInstance = this;
    }

    public void startBlockNumberChecker()
    {
        Debug.Log("startBlockNumberChecker");
        blockNumberChecking = true;
        StartCoroutine(blockNumberSyncronizer());
    }

    public void stopBlockNumberChecker()
    {
        Debug.Log("stopBlockNumberChecker");
        blockNumberChecking = false;
    }

    public void startWalletAddressChecker(string _initAddress)
    {
        Debug.Log("startWalletAddressChecker " + _initAddress);
        connectedWalletAddress = _initAddress;
        walletAddressChecking = true;
        StartCoroutine(walletAddressSyncronizer());
    }

    public void stopWalletAddressChecker()
    {
        Debug.Log("stopWalletAddressChecker");
        walletAddressChecking = false;
    }

    public void startServerStateChecker(bool _initServerAvailable)
    {
        Debug.Log("startServerStateChecker " + _initServerAvailable);
        serverAvailable = _initServerAvailable;
        serverStateChecking = true;
        StartCoroutine(serverStateSyncronizer());
    }

    public void stopServerStateChecker()
    {
        Debug.Log("stopServerStateChecker");
        serverStateChecking = false;
    }

    private IEnumerator blockNumberSyncronizer()
    {
        while(true)
        {
            ContractManager.instance.reqBlockNumber();

            yield return new WaitForSeconds(300);
        }
    }

    private IEnumerator walletAddressSyncronizer()
    {
        while (walletAddressChecking)
        {
            ContractManager.instance.reqConnectedWalletAddr();

            yield return new WaitForSeconds(30);
        }
    }

    private IEnumerator serverStateSyncronizer()
    {
        while (serverStateChecking)
        {
            ContractManager.instance.reqServerState();

            yield return new WaitForSeconds(60);
        }
    }

    private void Update()
    {
        passingTime += Time.deltaTime;
        if (passingTime >= 1)
        {
            passingTime -= 1;
            blockNumber += 1;
            notifyBlockNumberChanged();
        }
    }

    public void setBlockNumber(long _num)
    {
        blockNumber = _num;
        notifyBlockNumberChanged();
    }

    public void setConnectedWalletAddress(string _addr)
    {
        if (connectedWalletAddress != _addr)
        {
            connectedWalletAddress = _addr;
            notifyWalletAddressChanged();
        }
    }

    public void setServerState(bool _available)
    {
        if (serverAvailable != _available)
        {
            serverAvailable = _available;
            notifyServerStateChanged();
        }
    }

    public void notifyBlockNumberChanged()
    {
        foreach (BlockNumberObserever ob in blockNumberObserverList)
        {
            ob.onBlockNumberChanged(blockNumber);
        }
    }

    public void addBlockNumberObserver(BlockNumberObserever ob)
    {
        blockNumberObserverList.Add(ob);
        ob.onBlockNumberChanged(blockNumber);
    }

    public void removeBlockNumberObserver(BlockNumberObserever ob)
    {
        blockNumberObserverList.Remove(ob);
    }

    public void notifyWalletAddressChanged()
    {
        foreach (WalletAddressObserever ob in walletAddressObserverList)
        {
            ob.onWalletAddressChanged(connectedWalletAddress);
        }
    }

    public void addWalletAddressObserver(WalletAddressObserever ob)
    {
        walletAddressObserverList.Add(ob);
    }

    public void removeWalletAddressObserver(WalletAddressObserever ob)
    {
        walletAddressObserverList.Remove(ob);
    }

    public void notifyServerStateChanged()
    {
        foreach (ServerStateObserever ob in serverStateObserverList)
        {
            ob.onServerStateChanged(serverAvailable);
        }
    }

    public void addServerStateObserver(ServerStateObserever ob)
    {
        serverStateObserverList.Add(ob);
    }

    public void removeServerStateObserver(ServerStateObserever ob)
    {
        serverStateObserverList.Remove(ob);
    }
}