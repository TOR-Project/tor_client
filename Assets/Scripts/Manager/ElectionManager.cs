using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ElectionManager : MonoBehaviour
{
    public const int MINE = -1;
    public const int CANDIDATE_DATA_REQUEST_INTERVAL = 300; // 5 min

    public const long SAFETY_PERIOD = Const.QUARTER;
    public const long REBELLION_POSSIBLE_PERIOD = SAFETY_PERIOD + Const.QUARTER * 2;
    public const long MONARCH_REGIST_PERIOD = REBELLION_POSSIBLE_PERIOD + 86400 * 4;
    public const long MONARCH_ELECTION_PERIOD = MONARCH_REGIST_PERIOD + (long)(86400 * 3.5f);

    private List<CandidateObserver> observerList = new List<CandidateObserver>();
    private Dictionary<int, List<CandidateData>> candidateDataMap = new Dictionary<int, List<CandidateData>>();
    private Dictionary<int, List<int>> votingCountMap = new Dictionary<int, List<int>>();
    private Dictionary<int, long> lastRequestBlockMap = new Dictionary<int, long>();
    private ElectionState latestRoundRequestElectionState = ElectionState.SAFETY;
    private List<Predicate<List<int>>> notVotedCallbackList = new List<Predicate<List<int>>>();

    private List<DataObserver<List<RebellionData>>> rebellionObserverList = new List<DataObserver<List<RebellionData>>>();
    private Dictionary<int, long> lastRebellionRequestBlockMap = new Dictionary<int, long>();
    private Dictionary<int, List<int>> votingCompletedIdListMap = new Dictionary<int, List<int>>();
    private Dictionary<int, List<RebellionData>> rebellionDataMap = new Dictionary<int, List<RebellionData>>();


    static ElectionManager mInstance;
    public static ElectionManager instance
    {
        get
        {
            return mInstance;
        }
    }

    private ElectionManager()
    {
        mInstance = this;
    }

    public int getElectionRound()
    {
        long passedBlock = SystemInfoManager.instance.blockNumber - Const.ELECTION_START_BLOCK;
        int round = (int)(passedBlock / Const.ONE_YEAR);
        if (round < 0)
        {
            return 0;
        }

        return round;
    }

    private long getPresentPassedBlock()
    {
        long presentElectionStartBlock = Const.ELECTION_START_BLOCK + getElectionRound() * Const.ONE_YEAR;
        return SystemInfoManager.instance.blockNumber - presentElectionStartBlock;
    }

    public ElectionState getElectionState()
    {
        long passedBlock = getPresentPassedBlock();

        if (passedBlock < SAFETY_PERIOD)
        {
            return ElectionState.SAFETY;
        }
        if (passedBlock < REBELLION_POSSIBLE_PERIOD)
        {
            return ElectionState.REBELLION_POSSIBLE;
        }
        if (passedBlock < MONARCH_REGIST_PERIOD)
        {
            return ElectionState.REGIST;
        }

        return ElectionState.ELECTION;
    }

    public float getElectionProgressValue()
    {
        float value = (float)getPresentPassedBlock() / Const.ONE_YEAR;

        return value < 0 ? 0 : value;
    }

    public void requestRoundCandidateList(int _round)
    {
        long lastRequestBlock = 0;
        if (lastRequestBlockMap.ContainsKey(_round))
        {
            lastRequestBlock = lastRequestBlockMap[_round];
        }

        ElectionState nowState = getElectionState();
        bool stateChanged = latestRoundRequestElectionState != nowState;
        latestRoundRequestElectionState = nowState;

        if (!stateChanged && SystemInfoManager.instance.blockNumber < lastRequestBlock + CANDIDATE_DATA_REQUEST_INTERVAL)
        {
            notifyCandidateListReceived(candidateDataMap[_round]);
            return;
        }

        lastRequestBlockMap[_round] = SystemInfoManager.instance.blockNumber;
        ContractManager.instance.reqRoundCandidateList(_round);
    }

    public void responseRoundCandidateList(Dictionary<string, object> _data)
    {
        int round = int.Parse(_data["round"].ToString());
        List<CandidateData> candidateList;
        if (candidateDataMap.ContainsKey(round))
        {
            candidateList = candidateDataMap[round];
            candidateList.Clear();
        }
        else
        {
            candidateList = new List<CandidateData>();
            candidateDataMap[round] = candidateList;
        }

        votingCountMap[round] = JsonConvert.DeserializeObject<List<int>>(_data["votingCountList"].ToString());

        List<Dictionary<string, object>> cList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(_data["list"].ToString());
        foreach (Dictionary<string, object> cData in cList)
        {
            CandidateData d = new CandidateData();
            d.parseData(cData);
            candidateList.Add(d);
        }

        notifyCandidateListReceived(candidateList);
    }

    internal void requestNotVotedCharacterList(int _round, Predicate<List<int>> _callback)
    {
        notVotedCallbackList.Add(_callback);
        int[] list = new int[CharacterManager.instance.getMyCharacterList().Count];
        for (int i = 0; i < list.Length; i++)
        {
            list[i] = CharacterManager.instance.getMyCharacterList()[i].tokenId;
        }
        ContractManager.instance.reqNotVotedCharacterList(_round, list);
    }

    public void responseNotVotedCharacterList(int[] _data)
    {
        List<int> list = new List<int>(_data);
        foreach (Predicate<List<int>> callback in notVotedCallbackList)
        {
            callback.Invoke(list);
        }

        notVotedCallbackList.Clear();
    }


    public void updateCandidateData(CandidateData _data)
    {
        List<CandidateData> candidateList;

        if (candidateDataMap.ContainsKey(_data.round))
        {
            candidateList = candidateDataMap[_data.round];
        }
        else
        {
            candidateList = new List<CandidateData>();
            candidateDataMap[_data.round] = candidateList;
        }

        for (int i = 0; i < candidateList.Count; i++)
        {
            CandidateData cData = candidateList[i];
            if (cData.countryId == _data.countryId && cData.id == _data.id)
            {
                CharacterData characterData = CharacterManager.instance.getMyCharacterData(cData.tokenId);
                if (characterData != null)
                {
                    characterData.stakingData.purpose = StakingManager.PURPOSE_BREAK;
                }
                candidateList.Remove(cData);
                break;
            }
        }

        if (!_data.canceled)
        {
            CharacterData characterData = CharacterManager.instance.getMyCharacterData(_data.tokenId);
            if (characterData != null)
            {
                characterData.stakingData.purpose = _data.nftReturned ? StakingManager.PURPOSE_BREAK : StakingManager.PURPOSE_MONARCH;
            }

            candidateList.Add(_data);
        }
    }

    internal void requestRebellionDataList(int _round)
    {
        long lastRequestBlock = 0;
        if (lastRebellionRequestBlockMap.ContainsKey(_round))
        {
            lastRequestBlock = lastRebellionRequestBlockMap[_round];
        }

        if (SystemInfoManager.instance.blockNumber < lastRequestBlock + CANDIDATE_DATA_REQUEST_INTERVAL)
        {
            notifyRebellionDataListReceived(rebellionDataMap[_round]);
            return;
        }

        lastRebellionRequestBlockMap[_round] = SystemInfoManager.instance.blockNumber;
        ContractManager.instance.reqRoundRebellionList(_round);
    }

    public void responseRebellionDataList(Dictionary<string, object> _data)
    {
        int round = int.Parse(_data["round"].ToString());
        List<RebellionData> rebellionDataList;
        if (rebellionDataMap.ContainsKey(round))
        {
            rebellionDataList = rebellionDataMap[round];
            rebellionDataList.Clear();
        }
        else
        {
            rebellionDataList = new List<RebellionData>();
            rebellionDataMap[round] = rebellionDataList;
        }

        votingCompletedIdListMap[round] = JsonConvert.DeserializeObject<List<int>>(_data["idList"].ToString());

        List<Dictionary<string, object>> cList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(_data["list"].ToString());
        foreach (Dictionary<string, object> cData in cList)
        {
            RebellionData d = new RebellionData();
            d.parseData(cData);
            rebellionDataList.Add(d);
        }

        notifyRebellionDataListReceived(rebellionDataList);
    }

    public List<int> getVotingCompletedIdList(int _round)
    {
        if (votingCompletedIdListMap.ContainsKey(_round))
        {
            return votingCompletedIdListMap[_round];
        }
        return new List<int>();
    }


    public List<CandidateData> getCandidateList(int _round, int _cid)
    {
        if (candidateDataMap.ContainsKey(_round))
        {
            return candidateDataMap[_round].FindAll(data => data.countryId == _cid);
        }

        return new List<CandidateData>();
    }

    public List<CandidateData> getCandidateListWithIdSorting(int _round, int _cid)
    {
        List<CandidateData> list = getCandidateList(_round, _cid);
        list.Sort(SortByIdAscending);
        return list;
    }

    public int SortByIdAscending(CandidateData _cd1, CandidateData _cd2)
    {
        return _cd1.id - _cd2.id;
    }

    public List<CandidateData> getCandidateListWithRankSorting(int _round, int _cid)
    {
        List<CandidateData> list = getCandidateList(_round, _cid);
        list.Sort(SortByVotingDescending);
        return list;
    }

    public int getRanking(CandidateData _cd)
    {
        List<CandidateData> list = getCandidateListWithRankSorting(_cd.round, _cd.countryId);
        if (list.Count == 0)
        {
            return -1;
        }

        return list.IndexOf(_cd);
    }

    public int SortByVotingDescending(CandidateData _cd1, CandidateData _cd2)
    {
        if (_cd2.votingCount == _cd1.votingCount)
        {
            return _cd1.id - _cd2.id;
        }
        return _cd2.votingCount - _cd1.votingCount;
    }

    public CandidateData getMyCandidateData(int _round, int _cid)
    {
        if (candidateDataMap.ContainsKey(_round))
        {
            return candidateDataMap[_round].Find(data => data.countryId == _cid && UserManager.instance.isMyAddress(data.address));
        }

        return null;
    }

    public int getTotalVotingCount(int _round, int _cid)
    {
        return votingCountMap[_round][_cid];
    }

    public RebellionData getRebellionData(int _round, int _cid)
    {
        if (rebellionDataMap.ContainsKey(_round))
        {
            return rebellionDataMap[_round].Find(data => data.countryId == _cid);
        }

        return null;
    }

    public void updateRebellionData(RebellionData _data)
    {
        List<RebellionData> rebellionDataList;

        if (rebellionDataMap.ContainsKey(_data.round))
        {
            rebellionDataList = rebellionDataMap[_data.round];
        }
        else
        {
            rebellionDataList = new List<RebellionData>();
            rebellionDataMap[_data.round] = rebellionDataList;
        }

        for (int i = 0; i < rebellionDataList.Count; i++)
        {
            RebellionData cData = rebellionDataList[i];
            if (cData.countryId == _data.countryId)
            {
                CharacterData characterData = CharacterManager.instance.getMyCharacterData(cData.tokenId);
                if (characterData != null)
                {
                    characterData.stakingData.purpose = StakingManager.PURPOSE_BREAK;
                }
                rebellionDataList.Remove(cData);
                break;
            }
        }

        CharacterData characterDataNew = CharacterManager.instance.getMyCharacterData(_data.tokenId);
        if (characterDataNew != null)
        {
            characterDataNew.stakingData.purpose = _data.nftReturned ? StakingManager.PURPOSE_BREAK : StakingManager.PURPOSE_REBELLION;
        }

        rebellionDataList.Add(_data);
    }

    internal void resetJoinableIdList(int _round, int _cid)
    {
        List<RebellionData> rebellionDataList = rebellionDataMap[_round];
        for (int i = 0; i < rebellionDataList.Count; i++)
        {
            RebellionData cData = rebellionDataList[i];
            if (cData.countryId == _cid)
            {
                cData.myJoinableCharacterIdList.Clear();
                break;
            }
        }

    }

    public void notifyCandidateListReceived(List<CandidateData> _list)
    {
        foreach (CandidateObserver ob in observerList)
        {
            ob.onCandidateListReceived(_list);
        }
    }

    public void addObserver(CandidateObserver ob)
    {
        observerList.Add(ob);
    }

    public void removeObserver(CandidateObserver ob)
    {
        observerList.Remove(ob);
    }

    public void notifyRebellionDataListReceived(List<RebellionData> _list)
    {
        foreach (DataObserver<List<RebellionData>> ob in rebellionObserverList)
        {
            ob.onDataReceived(_list);
        }
    }

    public void addObserver(DataObserver<List<RebellionData>> ob)
    {
        rebellionObserverList.Add(ob);
    }

    public void removeObserver(DataObserver<List<RebellionData>> ob)
    {
        rebellionObserverList.Remove(ob);
    }
}
