using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[System.Serializable]
public class LogData
{
    public int id;
    public long blockNum;
    public int logType;
    public string whoAddr = "";
    public string whoNickName = "";
    public BigInteger dataInt = BigInteger.Zero;
    public string dataStr = "";

    internal void parseData(Dictionary<string, object> _data)
    {
        id = int.Parse(_data["id"].ToString());
        blockNum = long.Parse(_data["blockNum"].ToString());
        logType = int.Parse(_data["logType"].ToString());
        if (_data.ContainsKey("who"))
        {
            whoAddr = _data["who"].ToString();
        }
        if (_data.ContainsKey("nickName"))
        {
            whoNickName = _data["nickName"].ToString();
        }
        if (_data.ContainsKey("dataInt"))
        {
            dataInt = BigInteger.Parse(_data["dataInt"].ToString());
        }
        if (_data.ContainsKey("dataStr"))
        {
            dataStr = _data["dataStr"].ToString();
        }
    }
}