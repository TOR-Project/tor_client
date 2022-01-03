using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    [SerializeField]
    string walletAddress = "";
    [SerializeField]
    string nickname = "";
    [SerializeField]
    int termsVersion = 0;
    [SerializeField]
    List<string> friends = new List<string>();
    [SerializeField]
    bool tokenUsing = false;
    [SerializeField]
    bool nftUsing = false;
    [SerializeField]
    bool needMigration = false;
    [SerializeField]
    BigInteger coinAmount = BigInteger.Zero;
    [SerializeField]
    List<int> trophyList = new List<int>();
    [SerializeField]
    int password = 0;

    private ArrayList observerList = new ArrayList();

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

        notifyWalletAddressChanged();
    }

    public string getWalletAddress()
    {
        return walletAddress;
    }

    public void setUserData(string _nickName, int _termsVer, string[] _friends, bool _tokenUsing, bool _nftUsing, bool _needMigration)
    {
        nickname = _nickName;
        termsVersion = _termsVer;

        foreach(string friend in _friends)
        {
            friends.Add(friend);
        }

        tokenUsing = _tokenUsing;
        nftUsing = _nftUsing;
        needMigration = _needMigration;

        notifyNicknameChanged();
    }

    public void setNickname(string _nickName)
    {
        nickname = _nickName;

        notifyNicknameChanged();
    }

    public string getNickname()
    {
        return nickname;
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

    public bool isNeedMigration()
    {
        return needMigration;
    }

    public void setCoinAmount(BigInteger _amount)
    {
        coinAmount = _amount;

        notifyCoinAmountChanged();
    }

    public BigInteger getCoinAmount()
    {
        return coinAmount;
    }

    public void setTrophyList(int[] _list)
    {
        foreach (int trophy in _list)
        {
            trophyList.Add(trophy);
        }

        notifyTrophyDataChanged();
    }

    public List<int> getTrophyList()
    {
        return trophyList;
    }

    public void setPassword(int _password)
    {
        password = _password;
    }

    public int getPassword()
    {
        return password;
    }

    public void notifyNicknameChanged()
    {
        foreach (UserInfoObserever ob in observerList)
        {
            ob.onNicknameChanged(nickname);
        }
    }

    public void notifyCoinAmountChanged()
    {
        foreach (UserInfoObserever ob in observerList)
        {
            ob.onCoinAmountChanged(coinAmount);
        }
    }

    public void notifyWalletAddressChanged()
    {
        foreach (UserInfoObserever ob in observerList)
        {
            ob.onWalletAddressChanged(walletAddress);
        }
    }

    public void notifyTrophyDataChanged()
    {
        foreach (UserInfoObserever ob in observerList)
        {
            ob.onTrophyDataChanged(trophyList);
        }
    }

    public void addObserver(UserInfoObserever ob)
    {
        observerList.Add(ob);
        ob.onNicknameChanged(nickname);
        ob.onCoinAmountChanged(coinAmount);
        ob.onWalletAddressChanged(walletAddress);
        ob.onTrophyDataChanged(trophyList);
    }

    public void removeObserver(UserInfoObserever ob)
    {
        observerList.Remove(ob);
    }
}