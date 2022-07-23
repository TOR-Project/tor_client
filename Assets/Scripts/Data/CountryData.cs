using Newtonsoft.Json;
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
        List<Dictionary<string, object>> pList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(_data["propertyList"].ToString());
        foreach (Dictionary<string, object> propertyDic in pList)
        {
            PropertyData propertyData = new PropertyData();
            propertyData.parseData(propertyDic);
            propertyList.Add(propertyData);
        }

        logList.Clear();
        List<Dictionary<string, object>> lList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(_data["logList"].ToString());
        foreach (Dictionary<string, object> logDic in lList)
        {
            LogData logData = new LogData();
            logData.parseData(logDic);
            logList.Add(logData);
        }
        logList.Sort(SortByIdDescending);

        castleData.parseData(JsonConvert.DeserializeObject<Dictionary<string, object>>(_data["castleData"].ToString()));
    }

    internal void parseLogData(Dictionary<string, object> _data)
    {
        List<int> idCheckList = new List<int>();
        foreach (LogData logData in logList)
        {
            idCheckList.Add(logData.id);
        }

        List<Dictionary<string, object>> logDataList = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(_data["logList"].ToString());
        foreach (Dictionary<string, object> item in logDataList)
        {
            LogData logData = new LogData();
            logData.parseData(item);
            if (!idCheckList.Contains(logData.id))
            {
                logList.Add(logData);
            }
        }

        logList.Sort(SortByIdDescending);
    }

    public void addLog(LogData _logData)
    {
        _logData.blockNum = SystemInfoManager.instance.blockNumber;
        _logData.id = logList.Count > 0 ? logList[0].id + 1 : 0;
        logList.Add(_logData);

        logList.Sort(SortByIdDescending);
    }

    public int SortByIdDescending(LogData _ld1, LogData _ld2)
    {
        return _ld2.id - _ld1.id;
    }
}