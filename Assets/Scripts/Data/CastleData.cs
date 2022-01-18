using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[System.Serializable]
public class CastleData
{
    public bool hasMonarch;
    public string name;
    public int monarchId;
    public string monarchOwnerNickname;
    public List<int> formerMonarchList = new List<int>();
    public MiningTaxData lastMiningTaxData = new MiningTaxData();
    public BigInteger treasury;
    public BigInteger personalSafe;
    public long nextTaxSettableBlock;

    internal void parseData(Dictionary<string, object> _data)
    {
        hasMonarch = bool.Parse(_data["hasMonarch"].ToString());
        name = _data["name"].ToString();
        monarchId = int.Parse(_data["monarchId"].ToString());
        monarchOwnerNickname = _data["monarchOwnerNickname"].ToString();
        formerMonarchList = JsonConvert.DeserializeObject<List<int>>(_data["formerMonarchList"].ToString());
        lastMiningTaxData.parseData(JsonConvert.DeserializeObject<Dictionary<string, object>>(_data["lastMiningTaxData"].ToString()));
        treasury = BigInteger.Parse(_data["treasury"].ToString());
        personalSafe = BigInteger.Parse(_data["personalSafe"].ToString());
        nextTaxSettableBlock = long.Parse(_data["nextTaxSettableBlock"].ToString());
    }
}