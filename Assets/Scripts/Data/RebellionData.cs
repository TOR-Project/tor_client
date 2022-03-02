using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RebellionData
{
    public int round;
    public int tokenId;
    public int countryId;
    public string address;
    public string nickname;
    public string title;
    public string contents;
    public string url;
    public Dictionary<string, float> rebelStat;
    public Dictionary<string, float> registanceStat;
    public List<int> myJoinableCharacterIdList;
    public bool nftReturned;
    public long registBlock;

    internal void parseData(Dictionary<string, object> _data)
    {
        round = int.Parse(_data["round"].ToString());
        tokenId = int.Parse(_data["tokenId"].ToString());
        countryId = int.Parse(_data["country"].ToString());
        address = _data["address"].ToString();
        nickname = _data["nickname"].ToString();
        title = _data["title"].ToString();
        contents = _data["contents"].ToString();
        url = _data["url"].ToString();
        rebelStat = JsonConvert.DeserializeObject<Dictionary<string, float>>(_data["rebelStat"].ToString());
        registanceStat = JsonConvert.DeserializeObject<Dictionary<string, float>>(_data["registanceStat"].ToString());
        myJoinableCharacterIdList = JsonConvert.DeserializeObject<List<int>>(_data["idList"].ToString());
        nftReturned = bool.Parse(_data["nftReturned"].ToString());
        registBlock = long.Parse(_data["registBlock"].ToString());
    }

    public RebellionData clone()
    {
        RebellionData data = new RebellionData();
        data.round = round;
        data.tokenId = tokenId;
        data.countryId = countryId;
        data.address = address;
        data.nickname = nickname;
        data.title = title;
        data.contents = contents;
        data.url = url;
        data.nftReturned = nftReturned;
        data.registBlock = registBlock;
        return data;
    }
}