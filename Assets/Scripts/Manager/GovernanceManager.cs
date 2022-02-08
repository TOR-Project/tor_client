using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GovernanceManager : MonoBehaviour
{
    [SerializeField]
    long requestTermBlock = 300;

    private List<GovernanceObserver> observerList = new List<GovernanceObserver>();

    private List<AgendaData> agendaList = new List<AgendaData>();

    private long lastDataReceivedBlock = 0;

    static GovernanceManager mInstance;
    public static GovernanceManager instance {
        get {
            return mInstance;
        }
    }

    private GovernanceManager()
    {
        mInstance = this;
    }

    internal void requestAgendaList()
    {
        if (lastDataReceivedBlock + requestTermBlock < SystemInfoManager.instance.blockNumber)
        {
            ContractManager.instance.reqAgendaList();
            return;
        }

        ContractManager.instance.reqAgendaListCount();
    }

    public void responseAgendaCount(int _count)
    {
        if (agendaList.Count != _count)
        {
            ContractManager.instance.reqAgendaList();
            return;
        }

        notifyAgendaListReceived(agendaList);
    }

    public void responseAgendaList(List<Dictionary<string, object>> _data)
    {
        lastDataReceivedBlock = SystemInfoManager.instance.blockNumber;

        agendaList.Clear();

        foreach(Dictionary<string, object> data in _data)
        {
            AgendaData agendaData = new AgendaData();
            agendaData.parseData(data);
            agendaList.Add(agendaData);
        }

        agendaList.Sort(SortByStartBlockDescending);

        notifyAgendaListReceived(agendaList);
    }

    public void updateAgendaData(AgendaData _data)
    {
        bool addItem = true;
        foreach(AgendaData agendaData in agendaList)
        {
            if (agendaData.id == _data.id)
            {
                agendaList.Remove(agendaData);
                addItem = false;
                break;
            }
        }

        if (addItem)
        {
            foreach (CharacterData data in CharacterManager.instance.getMyCharacterList())
            {
                if (!_data.proposalTokenIdList.Contains(data.tokenId))
                {
                    continue;
                }
                data.stakingData.tokenId = _data.id;
                data.stakingData.purpose = StakingManager.PURPOSE_GOVERNANCE;
                data.stakingData.startBlock = SystemInfoManager.instance.blockNumber;
            }
        } else if (_data.nftReturned)
        {
            foreach (CharacterData data in CharacterManager.instance.getMyCharacterList())
            {
                if (!_data.proposalTokenIdList.Contains(data.tokenId))
                {
                    continue;
                }
                data.stakingData.tokenId = _data.id;
                data.stakingData.purpose = StakingManager.PURPOSE_BREAK;
            }
        }

        agendaList.Add(_data);

        agendaList.Sort(SortByStartBlockDescending);

        notifyAgendaListReceived(agendaList);
    }

    public int SortByStartBlockDescending(AgendaData _ad1, AgendaData _ad2)
    {
        return (int)(_ad2.startBlock - _ad1.startBlock);
    }

    public void notifyAgendaListReceived(List<AgendaData> _list)
    {
        foreach (GovernanceObserver ob in observerList)
        {
            ob.onAgendaListReceived(_list);
        }
    }

    public void addObserver(GovernanceObserver ob)
    {
        observerList.Add(ob);
    }

    public void removeObserver(GovernanceObserver ob)
    {
        observerList.Remove(ob);
    }
}
