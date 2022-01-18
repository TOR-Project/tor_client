using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MiningTaxData
{
    public float tax; // 000000 -> 00.0000
    public long startBlock;
    public long endBlock;

    internal void parseData(Dictionary<string, object> _data)
    {
        tax = float.Parse(_data["tax"].ToString()) / 10000f;
        startBlock = long.Parse(_data["startBlock"].ToString());
        endBlock = long.Parse(_data["endBlock"].ToString());
    }
}