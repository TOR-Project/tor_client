using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public interface UserInfoObserever
{
    void onNicknameChanged(string _nickname);
    void onWalletAddressChanged(string _address);
    void onCoinAmountChanged(BigInteger _amount);
    void onTrophyDataChanged(List<int> _trophyList);
}
