using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CountryData
{
    public int id;
    public List<PropertyData> propertyList = new List<PropertyData>();
    public CastleData castleData = new CastleData();
    public List<LogData> logList = new List<LogData>();
    public int population;

    public long lastUpdatedBlock;

    internal void parseData(Dictionary<string, object> _data)
    {
        id = int.Parse(_data["id"].ToString());
        population = int.Parse(_data["population"].ToString());
        lastUpdatedBlock = SystemInfoManager.instance.blockNumber;

        propertyList.Clear();
        List<string> pList = JsonConvert.DeserializeObject<List<string>>(_data["propertyList"].ToString());
        foreach (string propertyStr in pList)
        {
            PropertyData propertyData = new PropertyData();
            propertyData.parseData(JsonConvert.DeserializeObject<Dictionary<string, object>>(propertyStr));
            propertyList.Add(propertyData);
        }

        logList.Clear();
        List<string> lList = JsonConvert.DeserializeObject<List<string>>(_data["logList"].ToString());
        foreach (string logStr in lList)
        {
            LogData logData = new LogData();
            logData.parseData(JsonConvert.DeserializeObject<Dictionary<string, object>>(logStr));
            logList.Add(logData);
        }
        logList.Sort(SortByBlockDescending);

        castleData.parseData(JsonConvert.DeserializeObject<Dictionary<string, object>>(_data["castleData"].ToString()));
    }
    public int SortByBlockDescending(LogData _ld1, LogData _ld2)
    {
        return (int)(_ld2.blockNum - _ld1.blockNum);
    }
}