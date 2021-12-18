using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    [SerializeField]
    string walletAddress = "";
    [SerializeField]
    string nickName = "";
    [SerializeField]
    int termsVersion = 0;
    [SerializeField]
    List<string> friends = new List<string>();
    [SerializeField]
    bool tokenUsing = false;
    [SerializeField]
    bool nftUsing = false;
    [SerializeField]
    BigInteger tokenAmount = BigInteger.Zero;
    [SerializeField]
    List<int> trophyList = new List<int>();

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

    public void setUserData(string _nickName, int _termsVer, string[] _friends, bool _tokenUsing, bool _nftUsing)
    {
        nickName = _nickName;
        termsVersion = _termsVer;

        foreach(string friend in _friends)
        {
            friends.Add(friend);
        }

        tokenUsing = _tokenUsing;
        nftUsing = _nftUsing;
    }

    public void setNickname(string _nickName)
    {
        nickName = _nickName;
    }

    public string getNickname()
    {
        return nickName;
    }

    public void setTermsVer(int _termsVer)
    {
        termsVersion = _termsVer;
    }

    public int getTermsVer()
    {
        return termsVersion;
    }

    public void addFriend(string _friendAddr)
    {
        if (friends.Contains(_friendAddr))
        {
            return;
        }
        friends.Add(_friendAddr);
    }

    public void removeFriend(string _friendAddr)
    {
        friends.Remove(_friendAddr);
    }

    public List<string> getFriends()
    {
        return friends;
    }

    public void setTokenUsing(bool _set)
    {
        tokenUsing = _set;
    }

    public bool isTokenUsing()
    {
        return tokenUsing;
    }

    public void setNFTUsing(bool _set)
    {
        nftUsing = _set;
    }

    public bool isNFTUsing()
    {
        return nftUsing;
    }

    public void setTokenAmount(BigInteger _amount)
    {
        tokenAmount = _amount;
    }

    public BigInteger getTokenAmount()
    {
        return tokenAmount;
    }

    public void setTrophyList(int[] _list)
    {
        foreach (int trophy in _list)
        {
            trophyList.Add(trophy);
        }
    }

    public List<int> getTrophyList()
    {
        return trophyList;
    }
}