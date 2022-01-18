using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PropertyData
{
    public int propertyCategory;
    public int propertyType;
    public float value; // 000000 -> 00.0000
    public long startBlock;

    internal void parseData(Dictionary<string, object> _data)
    {
        propertyCategory = int.Parse(_data["propertyCategory"].ToString());
        propertyType = int.Parse(_data["propertyType"].ToString());
        value = float.Parse(_data["value"].ToString()) / 10000f;
        startBlock = long.Parse(_data["startBlock"].ToString());
    }
}