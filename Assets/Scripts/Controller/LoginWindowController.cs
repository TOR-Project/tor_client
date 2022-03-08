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

    [SerializeField]
    SoundLoadingComponent soundLoadingComponent;

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

        SoundManager.instance.requestSound("main", "https://project-ks1.s3.ap-northeast-2.amazonaws.com/2_tor_nft/4_assets/sound/main+menu/bgm/main.mp3", true, () => SoundManager.instance.playBgm("main"), null);
    }

    private IEnumerator updateNotice()
    {
        noticeLoadingIcon.SetActive(true);

        yield return new WaitUntil(ContractManager.instance.isUnityInstanceLoaded);
        yield return new WaitUntil(() => ContractManager.instance.isContractLoaded("notice"));

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