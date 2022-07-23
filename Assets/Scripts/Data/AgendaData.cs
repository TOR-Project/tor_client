using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AgendaData
{
    public int id;
    public string address;
    public List<int> proposalTokenIdList;
    public string nickname;
    public string title;
    public string contents;
    public List<string> items;
    public int[] votingData;
    public long startBlock;
    public long endBlock;
    public long periodBlock;
    public bool blind;
    public bool canceled;
    public bool nftReturned;
    public int[] notVotedIdList;

    internal void parseData(Dictionary<string, object> _data)
    {
        id = int.Parse(_data["id"].ToString());
        address = _data["address"].ToString();
        proposalTokenIdList = new List<int>(JsonConvert.DeserializeObject<int[]>(_data["proposalTokenIdList"].ToString()));
        nickname = _data["nickname"].ToString();
        title = _data["title"].ToString();
        contents = _data["contents"].ToString();
        items = new List<string>(JsonConvert.DeserializeObject<string[]>(_data["items"].ToString()));
        votingData = JsonConvert.DeserializeObject<int[]>(_data["votingData"].ToString());
        startBlock = long.Parse(_data["startBlock"].ToString());
        endBlock = long.Parse(_data["endBlock"].ToString());
        periodBlock = endBlock - startBlock;
        blind = bool.Parse(_data["blind"].ToString());
        canceled = bool.Parse(_data["canceled"].ToString());
        nftReturned = bool.Parse(_data["nftReturned"].ToString());
        notVotedIdList = JsonConvert.DeserializeObject<int[]>(_data["notVotedIdList"].ToString());
    }

    public Dictionary<string, object> generateData()
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        data["id"] = id;
        data["address"] = address;
        data["proposalTokenIdList"] = proposalTokenIdList.ToArray();
        data["nickname"] = nickname;
        data["title"] = title;
        data["contents"] = contents;
        data["items"] = items.ToArray();
        data["votingData"] = votingData;
        data["startBlock"] = startBlock;
        data["endBlock"] = endBlock;
        data["periodBlock"] = periodBlock;
        data["blind"] = blind;
        data["canceled"] = canceled;
        data["nftReturned"] = nftReturned;
        data["notVotedIdList"] = notVotedIdList;

        return data;
    }
}