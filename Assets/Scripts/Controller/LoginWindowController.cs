using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class LoginWindowController : MonoBehaviour
{
    [SerializeField]
    Text noticeTitle;
    [SerializeField]
    Text noticeText;
    [SerializeField]
    GameObject noticeLoadingIcon;
    [SerializeField]
    GlobalUIWindowController globalUIController;

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

    private void OnEnable()
    {
        SystemInfoManager.instance.stopWalletAddressChecker();
        SystemInfoManager.instance.stopServerStateChecker();
        SystemInfoManager.instance.stopBlockNumberChecker();
        globalUIController.hideInfoPanel();
    }

    private IEnumerator updateNotice()
    {
        noticeLoadingIcon.SetActive(true);

        Debug.Log("updateNotice() 1111 ");
        yield return new WaitUntil(ContractManager.instance.isUnityInstanceLoaded);
        Debug.Log("updateNotice() 2222 ");
        yield return new WaitUntil(() => ContractManager.instance.isContractLoaded("notice"));
        Debug.Log("updateNotice() 3333 ");

        ContractManager.instance.reqLatestNotice();
    }

    internal void connectWallet(string addr)
    {
        ContractManager.instance.reqLoginInfomation(addr);
    }

    internal void displayNotice(string title, long date, string contents)
    {
        noticeLoadingIcon.SetActive(false);
        noticeTitle.text = title;
        noticeText.text = contents;
    }

    internal void enterNextPage(bool _hasUserData, bool _latestTerms, bool _tokenUsing, bool _nftUsing, bool _needMigration)
    {
        if (_hasUserData && _latestTerms && _tokenUsing && _nftUsing && !_needMigration)
        {
            windowController.OpenWindow(titleWindow);
        } else
        {
            windowController.OpenWindow(termsWindow);
        }
    }
}