using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class LoginController : MonoBehaviour
{
    [SerializeField]
    Text noticeText;
    [SerializeField]
    GameObject noticeLoadingIcon;

    [SerializeField]
    GameObject walletConnectPanel;

    private void Awake()
    {
        StartCoroutine(updateNotice());
    }

    private IEnumerator updateNotice()
    {
        noticeLoadingIcon.SetActive(true);

        while (!ContractManager.instance.isUnityInstanceLoaded())
        {
            yield return new WaitForSeconds(1);
        }

        ContractManager.instance.reqLatestNotice();
    } 

    public void showingConnectWalletLoading()
    {
        walletConnectPanel.SetActive(true);
    }

    internal void showingLoginError(int err)
    {
        walletConnectPanel.SetActive(false);
    }

    internal void connectWallet(string addr)
    {
        walletConnectPanel.SetActive(false);
    }

    internal void displayNotice(string title, long date, string contents)
    {
        noticeLoadingIcon.SetActive(false);
        noticeText.text = title + "\n\n" + contents;
    }
}