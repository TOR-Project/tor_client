using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    [SerializeField]
    private string walletAddress;

    static UserManager mInstance;
    public static UserManager instance {
        get {
            return mInstance;
        }
    }

    private UserManager()
    {
        mInstance = this;
    }

    public void setWalletAddress(string _addr)
    {
        walletAddress = _addr;
    }

    public string getWalletAddress()
    {
        return walletAddress;
    }
}