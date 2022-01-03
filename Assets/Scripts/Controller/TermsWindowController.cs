using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class TermsWindowController : MonoBehaviour
{
    [SerializeField]
    GameObject termsPanel;
    [SerializeField]
    Animator termsPanelAnimator;
    [SerializeField]
    GameObject tokenUsingPanel;
    [SerializeField]
    Animator tokenUsingPanelAnimator;
    [SerializeField]
    GameObject nftUsingPanel;
    [SerializeField]
    Animator nftUsingPanelAnimator;
    [SerializeField]
    GameObject nicknamePanel;
    [SerializeField]
    LanguageTextController nicknameExpTextController;
    [SerializeField]
    Animator nicknamePanelAnimator;
    [SerializeField]
    Text redundancyText;
    LanguageTextController redundancyTextController;
    [SerializeField]
    Color redundancyFailedColor;
    [SerializeField]
    Color redundancySuccessColor;
    [SerializeField]
    TMP_InputField nicknameText;

    private string showingTrigger = "showing";
    private string dismissingTrigger = "dismissing";

    [SerializeField]
    GlobalUIWindowController globalUIController;
    [SerializeField]
    WindowController windowController;
    [SerializeField]
    GameObject titleWindow;

    private void Awake()
    {
        redundancyTextController = redundancyText.GetComponent<LanguageTextController>();
    }

    private void OnEnable()
    {
        globalUIController.hideInfoPanel();

        if (UserManager.instance.getTermsVer() < Const.TERMS_VERSION)
        {
            showTermsPanel();
            return;
        }

        if (!UserManager.instance.isTokenUsing())
        {
            showTokenUsingPanel();
            return;
        }

        if (!UserManager.instance.isNFTUsing())
        {
            showNFTUsingPanel();
            return;
        }

        if (UserManager.instance.getNickname().Equals(""))
        {
            registTokenToWallet();
            return;
        }

        if(UserManager.instance.isNeedMigration())
        {
            showNicknamePanel();
            return;
        }
    }

    private void showTermsPanel()
    {
        termsPanel.SetActive(true);
    }

    public void agreeTerms()
    {
        if (UserManager.instance.getNickname().Equals(""))
        {
            termsPanelAnimator.SetTrigger(dismissingTrigger);
        }
        else
        {
            ContractManager.instance.reqAgreeTerms(Const.TERMS_VERSION);
        }
    }

    public void resAgreeTerms()
    {
        termsPanelAnimator.SetTrigger(dismissingTrigger);
    }

    public void dismissTermsPanel()
    {
        termsPanel.SetActive(false);

        if (!UserManager.instance.isTokenUsing())
        {
            showTokenUsingPanel();
            return;
        }

        if (!UserManager.instance.isNFTUsing())
        {
            showNFTUsingPanel();
            return;
        }

        if (UserManager.instance.getNickname().Equals(""))
        {
            registTokenToWallet();
            return;
        }

        enterNextPage();
    }


    private void showTokenUsingPanel()
    {
        tokenUsingPanel.SetActive(true);
    }

    public void usingTokenConfirm()
    {
        ContractManager.instance.reqUsingToken();
    }

    public void resUsingTokenConfirm()
    {
        tokenUsingPanelAnimator.SetTrigger(dismissingTrigger);
    }

    public void dismissUsingTokenPanel()
    {
        tokenUsingPanel.SetActive(false);

        if (!UserManager.instance.isNFTUsing())
        {
            showNFTUsingPanel();
            return;
        }

        if (UserManager.instance.getNickname().Equals(""))
        {
            registTokenToWallet();
            return;
        }

        enterNextPage();
    }


    private void showNFTUsingPanel()
    {
        nftUsingPanel.SetActive(true);
    }


    public void usingNFTConfirm()
    {
        ContractManager.instance.reqUsingNFT();
    }

    public void resUsingNFTConfirm()
    {
        nftUsingPanelAnimator.SetTrigger(dismissingTrigger);
    }

    public void dismissUsingNFTPanel()
    {
        tokenUsingPanel.SetActive(false);

        if (UserManager.instance.getNickname().Equals(""))
        {
            registTokenToWallet();
            return;
        }

        enterNextPage();
    }

    private void registTokenToWallet()
    {
        globalUIController.showPopupByTextKey("ID_REGIST_TOKEN", ContractManager.instance.reqRegistTokenToWallet);
    }

    public void resRegistTokenToWallet()
    {
        showNicknamePanel();
    }

    private void showNicknamePanel()
    {
        if (UserManager.instance.isNeedMigration())
        {
            nicknameExpTextController.key = "ID_MIGRATION_EXP";
            nicknameExpTextController.onLanguageChanged();
            nicknameText.text = UserManager.instance.getNickname();
        }
        nicknamePanel.SetActive(true);
    }

    public void checkRedundancy()
    {
        string nickName = nicknameText.text.ToString().Trim();
        if (nickName.Length < 2 || nickName.Length > 10)
        {
            showUnavailableNicknameFeedback();
            return;
        }
        ContractManager.instance.reqCheckRedundancy(nickName);
    }

    private void showUnavailableNicknameFeedback()
    {
        redundancyTextController.key = "ID_REDUNDANCY_UNAVAILABLE";
        redundancyText.color = redundancyFailedColor;
        redundancyTextController.onLanguageChanged();
    }

    public void resCheckResundancy(bool _available)
    {
        if (_available)
        {
            redundancyTextController.key = "ID_REDUNDANCY_SUCCESS";
            redundancyText.color = redundancySuccessColor;
        } else
        {
            redundancyTextController.key = "ID_REDUNDANCY_FAIL";
            redundancyText.color = redundancyFailedColor;
        }
        redundancyTextController.onLanguageChanged();
    }

    public void createUser()
    {
        string nickName = nicknameText.text.ToString().Trim();
        if (nickName.Length < 2 || nickName.Length > 10)
        {
            showUnavailableNicknameFeedback();
            return;
        }
        ContractManager.instance.reqCreateUser(nickName);

    }

    public void resCreateUser()
    {
        nicknamePanelAnimator.SetTrigger(dismissingTrigger);
    }

    public void dismissNicknamePanel()
    {
        nicknamePanel.SetActive(false);
        enterNextPage();
    }


    internal void enterNextPage()
    {
        windowController.OpenWindow(titleWindow);
    }
}