using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MiningManager : MonoBehaviour
{
    public const int IDX_BASIC = 0;
    public const int IDX_TAX = 1;
    public const int IDX_COUNTRY = 2;
    public const int IDX_REBELLION = 3;
    public const int IDX_EARLYBIRD = 4;
    public const int IDX_ACCOUNT_RECEIVABLE = 5;
    public const int IDX_COMMISSION = 6;
    public const int IDX_FINAL = 7;
    public const int IDX_MAX = 8;

    public Dictionary<int, MiningData> miningMap = new Dictionary<int, MiningData>();
    public Dictionary<int, List<MiningDataObserever>> miningObserverMap = new Dictionary<int, List<MiningDataObserever>>();
    int syncronizerSeq = 0;
    List<int> miningDataCheckingList = new List<int>();

    static MiningManager mInstance;
    public static MiningManager instance {
        get {
            return mInstance;
        }
    }

    private MiningManager()
    {
        mInstance = this;
    }

    public void startMiningAmountSyncronizer()
    {
        syncronizerSeq++;
        StartCoroutine(updateMiningData(syncronizerSeq));
    }

    public void stopMiningAmountSyncronizer()
    {
        syncronizerSeq++;
    }

    public void requestMiningData(int[] _reqIdList, Action _callback)
    {
        miningDataCheckingList.Clear();
        StartCoroutine(reqMiningData(_reqIdList, _callback));
    }

    private IEnumerator reqMiningData(int[] _reqIdList, Action _callback)
    {
        foreach (int tokenId in _reqIdList)
        {
            ContractManager.instance.reqCalculateMiningAmount(tokenId);
        }

        yield return new WaitUntil(() => miningDataCheckingList.Count == _reqIdList.Length);

        _callback();
    }

    private IEnumerator updateMiningData(int _seq)
    {
        while(_seq == syncronizerSeq)
        {
            int maxCount = miningObserverMap.Count;
            int requestCount = 0;

            foreach (int tokenId in miningObserverMap.Keys)
            {
                if (miningObserverMap[tokenId].Count > 0)
                {
                    ContractManager.instance.reqCalculateMiningAmount(tokenId);
                    requestCount++;
                }
            }
            Debug.Log("request count = " + requestCount);

            yield return new WaitForSeconds(maxCount / 100 + 1);
        }
    }

    public void resMiningAmount(Dictionary<string, object> _miningData)
    {
        int tokenId = int.Parse(_miningData["tokenId"].ToString());
        if (!miningDataCheckingList.Contains(tokenId))
        {
            miningDataCheckingList.Add(tokenId);
        }
        if (!miningMap.ContainsKey(tokenId))
        {
            miningMap.Add(tokenId, new MiningData());
        }

        miningMap[tokenId].parse(_miningData);

        notifyMiningDataChaged(tokenId);
    }

    public MiningData getMiningData(int _tokenId)
    {
        if (!miningMap.ContainsKey(_tokenId))
        {
            miningMap.Add(_tokenId, new MiningData());
        }
        return miningMap[_tokenId];
    }

    public void resetAllMiningData()
    {
        foreach (MiningData md in miningMap.Values)
        {
            md.resetAmount();
        }
    }

    private void notifyMiningDataChaged(int _tokenId)
    {
        if (!miningObserverMap.ContainsKey(_tokenId) || !miningMap.ContainsKey(_tokenId))
        {
            return;
        }

        foreach (MiningDataObserever ob in miningObserverMap[_tokenId])
        {
            ob.onMiningDataChaged(miningMap[_tokenId]);
        }
    }

    public void addMiningDataObserver(int _tokenId, MiningDataObserever _ob)
    {
        if (!miningObserverMap.ContainsKey(_tokenId))
        {
            miningObserverMap.Add(_tokenId, new List<MiningDataObserever>());
        }
        miningObserverMap[_tokenId].Add(_ob);
    }

    public void removeMiningDataObserver(int _tokenId, MiningDataObserever _ob)
    {
        if (miningObserverMap.ContainsKey(_tokenId))
        {
            miningObserverMap[_tokenId].Remove(_ob);
        }
    }
}
