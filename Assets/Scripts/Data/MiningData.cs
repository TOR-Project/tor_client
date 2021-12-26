using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

[System.Serializable]
public class MiningData
{
    public BigInteger[] amount = new BigInteger[MiningManager.IDX_MAX];

    internal void parse(Dictionary<string, object> _miningData)
    {
        amount[MiningManager.IDX_BASIC] = BigInteger.Parse(_miningData["basicAmount"].ToString());
        amount[MiningManager.IDX_TAX] = BigInteger.Parse(_miningData["miningTaxAmount"].ToString());
        amount[MiningManager.IDX_COUNTRY] = BigInteger.Parse(_miningData["countryAmount"].ToString());
        amount[MiningManager.IDX_REBELLION] = BigInteger.Parse(_miningData["rebellionAmount"].ToString());
        amount[MiningManager.IDX_EARLYBIRD] = BigInteger.Parse(_miningData["earlybirdAmount"].ToString());
        amount[MiningManager.IDX_ACCOUNT_RECEIVABLE] = BigInteger.Parse(_miningData["accountReceivableAmount"].ToString());
        amount[MiningManager.IDX_COMMISSION] = BigInteger.Parse(_miningData["commissionAmount"].ToString());
        amount[MiningManager.IDX_FINAL] = BigInteger.Parse(_miningData["finalAmount"].ToString());
    }

    internal void resetAmount()
    {
        for (int i = 0; i < amount.Length; i++)
        {
            amount[i] = BigInteger.Zero;
        }
    }
}