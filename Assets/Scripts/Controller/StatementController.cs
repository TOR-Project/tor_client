using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using System.Runtime.InteropServices;
public class StatementController : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void OpenTab(string url);

    int round = 0;
    int cid = 0;
    RebellionData rebellionData = null;

    [SerializeField]
    Text statementTitleText;

    [SerializeField]
    GameObject selectGuidePanel;
    [SerializeField]
    GameObject noDataPanel;
    [SerializeField]
    GameObject newRegistPanel;
    [SerializeField]
    GameObject instigatorPanel;

    [SerializeField]
    Text newRegistText;

    [SerializeField]
    GameObject statementShowingPanel;
    [SerializeField]
    GameObject statementEditingPanel;

    [SerializeField]
    Text nicknameText;
    [SerializeField]
    Text tokenIdText;

    [SerializeField]
    Text titleText;
    [SerializeField]
    Text contentsText;
    [SerializeField]
    Text urlText;

    [SerializeField]
    TMP_InputField titleInputField;
    [SerializeField]
    TMP_InputField contentsInputField;
    [SerializeField]
    TMP_InputField urlInputField;

    [SerializeField]
    GameObject characterSelectPanel;
    [SerializeField]
    CharacterImageController characterImageController;
    [SerializeField]
    Image flagImage;
    [SerializeField]
    GameObject characterLoading;

    [SerializeField]
    GameObject battleScorePanel;
    [SerializeField]
    Text rebelBattleScoreTitleText;

    [SerializeField]
    GameObject registButton;
    [SerializeField]
    GameObject returnButton;
    [SerializeField]
    GameObject revolutionButton;
    [SerializeField]
    GameObject joinRebelButton;
    [SerializeField]
    GameObject joinRegistanceButton;

    [SerializeField]
    SlidingSliderController rebelBattleSliderSldingController;
    [SerializeField]
    SlidingFloatingNumberController rebelBattleScoreSlidingController;
    [SerializeField]
    SlidingSliderController registanceBattleSliderSldingController;
    [SerializeField]
    SlidingFloatingNumberController registanceBattleScoreSlidingController;

    [SerializeField]
    GlobalUIWindowController globalUIWindowController;

    private RuinedTempleWindowController ruinedTempleWindowController;

    private void OnEnable()
    {
        rebelBattleScoreSlidingController.setFormat("{0:#,###}");
        registanceBattleScoreSlidingController.setFormat("{0:#,###}");
    }

    public void setRuinedTempleWindowController(RuinedTempleWindowController _controller)
    {
        ruinedTempleWindowController = _controller;
    }

    public void updateStatement(RebellionData _data, int _round, int _cid)
    {
        selectGuidePanel.SetActive(false);
        noDataPanel.SetActive(false);
        newRegistPanel.SetActive(false);
        instigatorPanel.SetActive(false);

        rebellionData = _data;
        round = _round;
        cid = _cid;

        if (_data == null)
        {
            statementTitleText.text = "";

            int nowRound = ElectionManager.instance.getElectionRound();
            ElectionState nowState = ElectionManager.instance.getElectionState();

            if (_round >= nowRound && nowState == ElectionState.REBELLION_POSSIBLE)
            {
                newRegistText.color = Color.white;
                newRegistText.text = string.Format(LanguageManager.instance.getText("ID_REBELLION_REGIST_EXP"), CountryManager.instance.getCountryName(cid), _round);
                newRegistPanel.SetActive(true);
            }
            else
            {
                noDataPanel.SetActive(true);
            }
        }
        else
        {
            updateStatementPanel(_data);
            instigatorPanel.SetActive(true);
        }
    }

    private void updateStatementPanel(RebellionData _data)
    {
        characterImageController.gameObject.SetActive(false);
        characterLoading.SetActive(true);
        CharacterManager.instance.getCharacterDataAsync(_data.tokenId, cData =>
        {
            if (cData.tokenId != rebellionData.tokenId)
            {
                return false;
            }
            tokenIdText.text = cData.name;
            characterImageController.updateCharacterImage(cData);
            characterImageController.gameObject.SetActive(true);
            characterLoading.SetActive(false);
            return true;
        });

        nicknameText.text = _data.nickname;

        titleText.text = _data.title;
        contentsText.text = _data.contents;
        urlText.text = _data.url;

        flagImage.sprite = CountryManager.instance.getSmallFlagImage(cid);

        characterSelectPanel.SetActive(false);
        statementShowingPanel.SetActive(true);
        statementEditingPanel.SetActive(false);

        int nowRound = ElectionManager.instance.getElectionRound();
        ElectionState nowState = ElectionManager.instance.getElectionState();

        bool rebellionFinished = _data.registBlock + Const.REBELLION_RECRUITMENT_PERIOD <= SystemInfoManager.instance.blockNumber;
        bool isRevolution = false;

        battleScorePanel.SetActive(rebellionFinished);
        if (battleScorePanel.activeSelf)
        {
            resetBattleScore();
            float rebelBattleScore = getTotalBattleScore(_data.rebelStat, 2, 1);
            float registanceBattleScore = getTotalBattleScore(_data.registanceStat, 1, 2);
            setBattleScore(rebelBattleScore, registanceBattleScore);

            isRevolution = rebelBattleScore >= registanceBattleScore;
        }

        if (!rebellionFinished)
        {
            statementTitleText.text = LanguageManager.instance.getText("ID_REBELLION_STATEMENT_TITLE");
        } else
        {
            statementTitleText.text = isRevolution ? LanguageManager.instance.getText("ID_WIN_REBEL") : LanguageManager.instance.getText("ID_WIN_RESISTANCE");
            rebelBattleScoreTitleText.text = isRevolution ? LanguageManager.instance.getText("ID_REBEL_BATTLE_SCORE_WON") : LanguageManager.instance.getText("ID_REBEL_BATTLE_SCORE");
        }

        registButton.SetActive(false);
        returnButton.SetActive(rebellionFinished && !isRevolution && !_data.nftReturned && UserManager.instance.isMyAddress(_data.address));
        revolutionButton.SetActive(rebellionFinished && isRevolution && !_data.nftReturned && UserManager.instance.isMyAddress(_data.address));
        joinRebelButton.SetActive(!rebellionFinished);
        joinRegistanceButton.SetActive(!rebellionFinished);
    }

    public float getTotalBattleScore(Dictionary<string, float> _stat, float _attMag, float _defMag)
    {
        float att = 0;
        float def = 0;
        float attBoost = 1;
        float defBoost = 1;

        if (_stat.ContainsKey("att"))
        {
            att = _stat["att"];
        }

        if (_stat.ContainsKey("def"))
        {
            def = _stat["def"];
        }

        if (_stat.ContainsKey("attBoost"))
        {
            attBoost = 1 + _stat["attBoost"] / 10000;
        }

        if (_stat.ContainsKey("defBoost"))
        {
            defBoost = 1 + _stat["defBoost"] / 10000;
        }

        att *= attBoost;
        def *= defBoost;

        return att * _attMag + def * _defMag;
    }

    public void updateEditPanel(RebellionData _data)
    {
        rebellionData = _data;

        CharacterData characterData = _data.tokenId == -1 ? null : CharacterManager.instance.getMyCharacterData(_data.tokenId);
        nicknameText.text = _data.nickname;
        tokenIdText.text = characterData == null ? "" : characterData.name;

        characterImageController.gameObject.SetActive(characterData != null);
        characterSelectPanel.SetActive(true);
        statementShowingPanel.SetActive(false);
        statementEditingPanel.SetActive(true);

        if (characterData != null)
        {
            characterImageController.updateCharacterImage(characterData);
        }
        titleInputField.text = _data.title;
        contentsInputField.text = _data.contents;
        urlInputField.text = _data.url;

        flagImage.sprite = CountryManager.instance.getSmallFlagImage(cid);

        battleScorePanel.SetActive(false);

        registButton.SetActive(true);
        returnButton.SetActive(false);
        revolutionButton.SetActive(false);
        joinRebelButton.SetActive(false);
        joinRegistanceButton.SetActive(false);
    }

    public void resetAllInstigatorPanel()
    {
        selectGuidePanel.SetActive(true);
        newRegistPanel.SetActive(false);
        noDataPanel.SetActive(false);
        instigatorPanel.SetActive(false);
    }

    public void onRegistButtonClicked()
    {
        if (rebellionData.tokenId < 0)
        {
            globalUIWindowController.showPopupByTextKey("ID_REBELLION_PLEASE_SELECT_CHARACTER", null);
            return;
        }

        if (string.IsNullOrEmpty(titleInputField.text))
        {
            globalUIWindowController.showPopupByTextKey("ID_REBELLION_PLEASE_FILL_TITLE", null);
            return;
        }

        if(string.IsNullOrEmpty(contentsInputField.text))
        {
            globalUIWindowController.showPopupByTextKey("ID_REBELLION_PLEASE_FILL_CONTENTS", null);
            return;
        }

        rebellionData.title = titleInputField.text;
        rebellionData.contents = contentsInputField.text;
        rebellionData.url = urlInputField.text;
        ruinedTempleWindowController?.addRebellionData(rebellionData);
    }

    public void setParticleEnabled(bool _set)
    {
        characterImageController.setParticleEnabled(_set);
    }

    public void onNewButtonClicked()
    {
        if (UserManager.instance.getCoinAmount() < Utils.convertToPeb(Const.MONARCH_REGIST_FEE))
        {
            newRegistText.color = Color.red;
            newRegistText.text = string.Format(LanguageManager.instance.getText("ID_REBELLION_REGIST_FEE_LEAK"), Const.REBELLION_REGIST_FEE);
            return;
        }

        rebellionData = new RebellionData();
        rebellionData.countryId = cid;
        rebellionData.round = round;
        rebellionData.tokenId = -1;
        rebellionData.address = UserManager.instance.getWalletAddress();
        rebellionData.nickname = UserManager.instance.getNickname();
        updateEditPanel(rebellionData);

        newRegistPanel.SetActive(false);
        instigatorPanel.SetActive(true);
    }

    public void onReturnButtonClicked()
    {
        ruinedTempleWindowController?.returnRebellionData(rebellionData);
    }

    public void onRevolutionButtonClicked()
    {
        ruinedTempleWindowController?.revolusionRebellionData(rebellionData);
    }

    public void onCharacterAddButtonClicked()
    {
        ruinedTempleWindowController?.showCharacterSelectPopup(rebellionData);
    }

    public void onJoinRebelButtonClicked()
    {
        ruinedTempleWindowController?.joinRebel(rebellionData);
    }

    public void onJoinResistanceButtonClicked()
    {
        ruinedTempleWindowController?.joinResistance(rebellionData);
    }

    public void onUrlButtonClicked()
    {
        if (rebellionData != null && !string.IsNullOrEmpty(rebellionData.url))
        {
            OpenTab(rebellionData.url);
        }
    }

    public void setBattleScore(float _rebelScore, float _registanceScore)
    {
        rebelBattleScoreSlidingController.setNumber(_rebelScore);
        registanceBattleScoreSlidingController.setNumber(_registanceScore);
        rebelBattleSliderSldingController.setMaxValue(_rebelScore + _registanceScore);
        registanceBattleSliderSldingController.setMaxValue(_rebelScore + _registanceScore);
        rebelBattleSliderSldingController.setValue(_rebelScore);
        registanceBattleSliderSldingController.setValue(_registanceScore);
    }

    public void resetBattleScore()
    {
        rebelBattleScoreSlidingController.reset();
        registanceBattleScoreSlidingController.reset();

        rebelBattleSliderSldingController.reset();
        registanceBattleSliderSldingController.reset();
    }
}