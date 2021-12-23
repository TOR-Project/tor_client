using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class GlobalUIWindowController : MonoBehaviour
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

    private Action callback;

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
        showPopupByTextKey(key, null);
    }

    public void showBanPopup(long _startBlock, long _endBlock, string _reason)
    {
        string contents = LanguageManager.instance.getText("ID_ERR_USER_BANNED");
        contents += "\n" + _reason + "\n" + "(" + _startBlock + " ~ " + _endBlock + " block )";
        showPopup(contents, null);
    }

    public void showPopupByTextKey(string _contentsKey, Action _callback)
    {
        showPopup(LanguageManager.instance.getText(_contentsKey), _callback);
    }

    public void showPopup(string _contents, Action _callback)
    {
        callback = _callback;
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
        callback?.Invoke();
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