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
    GlobalUIController globalUIController;

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
        globalUIController.hideInfoPanel();
    }

    private IEnumerator updateNotice()
    {
        noticeLoadingIcon.SetActive(true);

        yield return new WaitUntil(ContractManager.instance.isUnityInstanceLoaded);

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

    internal void enterNextPage(bool _hasUserData, bool _latestTerms, bool _tokenUsing, bool _nftUsing)
    {
        if (_hasUserData && _latestTerms && _tokenUsing && _nftUsing)
        {
            windowController.OpenWindow(titleWindow);
        } else
        {
            windowController.OpenWindow(termsWindow);
        }
    }
}