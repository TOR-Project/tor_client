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
        Debug.Log("requestCoinAmountConstantly()");
        while(UserManager.instance.getWalletAddress() != "")
        {
            Debug.Log("requestCoinAmountConstantly() gogo");
            ContractManager.instance.reqCoinAmount();

            yield return new WaitForSeconds(10);
        }
    }

    public void onCoinAmountChanged(BigInteger _amount)
    {
        BigInteger remains;
        BigInteger gold = BigInteger.DivRem(_amount, BigInteger.Parse("1000000000000000000"), out remains);
        BigInteger silver = BigInteger.Divide(remains, BigInteger.Parse("100000000000000"));

        coinAmountText.text = gold.ToString("#,0") + "." + silver.ToString("0000") + " ToR";
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

        globalUIWindow.StartCoroutine(requestCoinAmountConstantly());
    }

    public void onTrophyDataChanged(List<int> _trophyList)
    {
        // TODO
    }
}