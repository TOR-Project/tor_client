using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class LoginController : MonoBehaviour
{
    [SerializeField]
    Text noticeTitle;
    [SerializeField]
    Text noticeText;
    [SerializeField]
    GameObject noticeLoadingIcon;

    [SerializeField]
    GameObject walletConnectPanel;

    [SerializeField]
    WindowController windowController;
    [SerializeField]
    GameObject termsWindow;
    [SerializeField]
    GameObject titleWindow;

    private void Awake()
    {
        StartCoroutine(updateNotice());
    }

    private IEnumerator updateNotice()
    {
        noticeLoadingIcon.SetActive(true);

        yield return new WaitUntil(ContractManager.instance.isUnityInstanceLoaded);

        ContractManager.instance.reqLatestNotice();
    } 

    public void showingConnectWalletLoading()
    {
        walletConnectPanel.SetActive(true);
    }

    internal void connectWallet(string addr)
    {
        UserManager.instance.setWalletAddress(addr);
        ContractManager.instance.reqLoginInfomation(addr);
    }

    internal void displayNotice(string title, long date, string contents)
    {
        noticeLoadingIcon.SetActive(false);
        noticeTitle.text = title;
        noticeText.text = contents;
    }

    internal void showErrorPopup(int err)
    {
        walletConnectPanel.SetActive(false);
    }

    internal void enterNextPage(bool _hasUserData, bool _latestTerms, bool _tokenUsing, bool _nftUsing)
    {
        walletConnectPanel.SetActive(false);
        if (_hasUserData && _latestTerms && _tokenUsing && _nftUsing)
        {
            windowController.OpenWindow(titleWindow);
        } else
        {
            windowController.OpenWindow(termsWindow);
        }
    }
}