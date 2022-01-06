using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class TitleWindowController : MonoBehaviour
{
    [SerializeField]
    GameObject initCharacterPopupPanel;
    [SerializeField]
    Text initCharacterPopupText;
    private int characterInitMaxStep = 0;

    [SerializeField]
    Animator initCharacterPopupPanelPanelAnimator;

    [SerializeField]
    GameObject characterWindow;
    [SerializeField]
    WindowController windowController;
    [SerializeField]
    GlobalUIWindowController globalUIController;
    [SerializeField]
    string enterCharacterWindowErrorKey;

    private string dismissingTrigger = "dismissing";

    private void OnEnable()
    {
        CharacterManager.instance.reqNotInitCharacterList(showPopup);
        ContractManager.instance.reqGetPassword();
        SystemInfoManager.instance.startWalletAddressChecker(UserManager.instance.getWalletAddress());
        SystemInfoManager.instance.startServerStateChecker(true);
        SystemInfoManager.instance.startBlockNumberChecker();
    }

    public bool showPopup(int _part)
    {
        if (_part > 1)
        {
            characterInitMaxStep = _part;
            initCharacterPopupText.text = LanguageManager.instance.getText("ID_INIT_CHARACTER_EXP") + string.Format(LanguageManager.instance.getText("ID_TOO_MANY_CHARACTER"), 1, _part);
        } else
        {
            initCharacterPopupText.text = LanguageManager.instance.getText("ID_INIT_CHARACTER_EXP");
        }
        initCharacterPopupPanel.SetActive(true);

        return true;
    }

    public void reqInitCharacterTx()
    {
        CharacterManager.instance.reqInitCharacter(confirmPopup);
    }

    public bool confirmPopup(int _step)
    {
        if (_step >= characterInitMaxStep)
        {
            initCharacterPopupPanelPanelAnimator.SetTrigger(dismissingTrigger);
        }
        else
        {
            initCharacterPopupText.text = LanguageManager.instance.getText("ID_INIT_CHARACTER_EXP") + string.Format(LanguageManager.instance.getText("ID_TOO_MANY_CHARACTER"), _step + 1, characterInitMaxStep);
        }

        return true;
    }

    public void dismissPopup()
    {
        initCharacterPopupPanel.SetActive(false);
    }

    public void enterCharacterWindow()
    {
        if(CharacterManager.instance.thereIsNotInitCharacter)
        {
            globalUIController.showPopupByTextKey(enterCharacterWindowErrorKey, null);
        } else
        {
            windowController.OpenWindow(characterWindow);
        }
    }
}