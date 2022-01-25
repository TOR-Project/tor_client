using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CandidateData
{
    public int id;
    public int round;
    public int tokenId;
    public int countryId;
    public string addr;
    public string nickname;
    public string title;
    public string contents;
    public string url;
    public bool canceled;
    public int votingCount;
    public bool nftReturned;
    public long registBlock;

    internal void parseData(Dictionary<string, object> _data)
    {
        id = int.Parse(_data["id"].ToString());
        round = int.Parse(_data["round"].ToString());
        tokenId = int.Parse(_data["tokenId"].ToString());
        countryId = int.Parse(_data["country"].ToString());
        addr = _data["addr"].ToString();
        nickname = _data["nickname"].ToString();
        title = _data["title"].ToString();
        contents = _data["contents"].ToString();
        url = _data["url"].ToString();
        canceled = bool.Parse(_data["canceled"].ToString());
        votingCount = int.Parse(_data["votingCount"].ToString());
        nftReturned = bool.Parse(_data["nftReturned"].ToString());
        registBlock = long.Parse(_data["registBlock"].ToString());
    }
}