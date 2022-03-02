using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[System.Serializable]
public class SellItemData
{
    public int id;
    public BigInteger price; // when buy
    public BigInteger sellPrice;
    public int remainCount;
    public long deadline;

    public SellItemData()
    {
    }

    public void parse(Dictionary<string, object> _data)
    {
        id = int.Parse(_data["id"].ToString());
        price = BigInteger.Parse(_data["price"].ToString());
        sellPrice = BigInteger.Parse(_data["sellPrice"].ToString());
        remainCount = int.Parse(_data["remainCount"].ToString());
        deadline = long.Parse(_data["deadline"].ToString());
    }

}