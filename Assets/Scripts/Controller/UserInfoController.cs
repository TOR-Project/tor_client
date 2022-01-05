using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Numerics;

public class UserInfoController : MonoBehaviour, UserInfoObserever
{
    [SerializeField]
    MonoBehaviour globalUIWindow;

    [SerializeField]
    Text walletAddressText;
    [SerializeField]
    Text nicknameText;
    [SerializeField]
    Text coinAmountText;

    private void Awake()
    {
        UserManager.instance.addObserver(this);
    }

    private void OnDestroy()
    {
        UserManager.instance.removeObserver(this);
    }

    private IEnumerator requestCoinAmountConstantly()
    {
        while(UserManager.instance.getWalletAddress() != "")
        {
            ContractManager.instance.reqCoinAmount();

            yield return new WaitForSeconds(10);
        }
    }

    public void onCoinAmountChanged(BigInteger _amount)
    {
        coinAmountText.text = Utils.convertPebToTorStr(_amount) + " " + Const.TOR_COIN;
    }

    public void onNicknameChanged(string _nickname)
    {
        nicknameText.text = _nickname;
    }

    public void onWalletAddressChanged(string _address)
    {
        if (walletAddressText != null)
        {
            walletAddressText.text = _address;
        }

        // globalUIWindow.StartCoroutine(requestCoinAmountConstantly());
        ContractManager.instance.reqCoinAmount();
    }

    public void onTrophyDataChanged(List<int> _trophyList)
    {
        // TODO
    }
}