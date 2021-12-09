using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using MedievalKingdomUI.Scripts.Window;

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
    AnimatedWindowController windowController;
    [SerializeField]
    GameObject titleWindow;

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

    internal void enterTitlePage()
    {
        walletConnectPanel.SetActive(false);
        windowController.OpenWindow(titleWindow);
    }
}