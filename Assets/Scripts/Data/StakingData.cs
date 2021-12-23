using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StakingData
{
    public int tokenId = -1;
    public long startBlock = 0;
    public long endBlock = 0;
    public int purpose = 0;

    internal void parsing(Dictionary<string, object> _stakingData)
    {
        tokenId = int.Parse(_stakingData["id"].ToString());
        startBlock = long.Parse(_stakingData["startBlock"].ToString());
        endBlock = long.Parse(_stakingData["endBlock"].ToString());
        purpose = int.Parse(_stakingData["purpose"].ToString());
    }
}