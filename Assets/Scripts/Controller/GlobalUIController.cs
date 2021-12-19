using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class GlobalUIController : MonoBehaviour
{
    [SerializeField]
    GameObject popupPanel;
    [SerializeField]
    GameObject loadingPanel;
    [SerializeField]
    GameObject infoPanel;

    [SerializeField]
    Text popupContentsText;
    [SerializeField]
    Animator popupPanelAnimator;

    [SerializeField]
    WindowController windowController;

    [SerializeField]
    GameObject loginWindow;

    private bool moveToLogin = false;
    private string showingTrigger = "showing";
    private string dismissingTrigger = "dismissing";

    public void showErrorPopup(int _err)
    {
        string key = "";
        switch(_err)
        {
            case Const.ERR_SERVER_BLOCKED:
                key = "ID_ERR_SERVER_BLOCKED";
                break;
            case Const.ERR_VERSION_MISMATCHED:
                key = "ID_ERR_VERSION_MISMATCHED";
                break;
            case Const.ERR_NETWORK_MISMATCHED:
                key = "ID_ERR_NETWORK_MISMATCHED";
                break;
            case Const.ERR_WALLET_CONNECTION_FAILED:
                key = "ID_ERR_WALLET_CONNECTION_FAILED";
                break;
            case Const.ERR_NO_CHARACTER:
                key = "ID_ERR_NO_CHARACTER";
                break;
            default:

                break;
        }
        showPopupByTextKey(key, false);
    }

    public void showBanPopup(long _startBlock, long _endBlock, string _reason)
    {
        string contents = LanguageManager.instance.getText("ID_ERR_USER_BANNED");
        contents += "\n" + _reason + "\n" + "(" + _startBlock + " ~ " + _endBlock + " block )";
        showPopup(contents, false);
    }

    public void showPopupByTextKey(string _contentsKey, bool _moveToLogin)
    {
        showPopup(LanguageManager.instance.getText(_contentsKey), _moveToLogin);
    }

    public void showPopup(string _contents, bool _moveToLogin)
    {
        moveToLogin = _moveToLogin;
        popupContentsText.text = _contents;

        popupPanel.SetActive(true);
    }

    public void confirmPopup()
    {
        popupPanelAnimator.SetTrigger(dismissingTrigger);
    }

    public void dismissPopup()
    {
        popupPanel.SetActive(false);
        if (moveToLogin)
        {
            windowController.OpenWindow(loginWindow);
        }
    }

    public void showLoading()
    {
        loadingPanel.SetActive(true);
    }

    public void hideLoading()
    {
        loadingPanel.SetActive(false);
    }

    public void showInfoPanel()
    {
        infoPanel.SetActive(true);
    }

    public void hideInfoPanel()
    {
        infoPanel.SetActive(false);
    }
}