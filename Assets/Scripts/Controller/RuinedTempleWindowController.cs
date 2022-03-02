using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class RuinedTempleWindowController : MonoBehaviour, DataObserver<List<RebellionData>>
{
    private int selectedRound = 0;
    private int selectedCountryId = -1;

    [SerializeField]
    GlobalUIWindowController globalUIWindowController;

    [SerializeField]
    Dropdown roundDropdown;
    [SerializeField]
    Button[] countryButton;
    [SerializeField]
    GameObject mapLoading;

    [SerializeField]
    Text guideText;

    [SerializeField]
    StatementController statementController;

    [SerializeField]
    GameObject characterSelectPopup;
    [SerializeField]
    CharacterGridController characterGridController;
    [SerializeField]
    GameObject noCharacterText;
    [SerializeField]
    Animator characterSelctPopupAnimator;

    private string dismissingTrigger = "dismissing";

    private void OnEnable()
    {
        ElectionManager.instance.addObserver(this);
        ContractManager.instance.reqGetPassword();
        guideText.text = string.Format(LanguageManager.instance.getText("ID_RUINED_TEMPLE_GUIDE"), Const.REBELLION_REGIST_FEE, Const.REBELLION_RECRUITMENT_PERIOD, Const.REBELLION_JOIN_FEE, Const.REBELLION_RECRUITMENT_PERIOD);
        addRoundInfo();
        resetRegionButton();
        statementController.resetAllInstigatorPanel();
        statementController.setRuinedTempleWindowController(this);
    }

    private void OnDisable()
    {
        ElectionManager.instance.removeObserver(this);
    }

    private void resetRegionButton()
    {
        foreach (Button button in countryButton)
        {
            button.interactable = true;
        }

        selectedCountryId = -1;
    }

    private void addRoundInfo()
    {
        roundDropdown.options.Clear();
        int nowRound = ElectionManager.instance.getElectionRound();
        ElectionState nowState = ElectionManager.instance.getElectionState();

        for (int i = nowRound; i > 0; i--)
        {
            string roundTitle = string.Format(LanguageManager.instance.getText("ID_N_MONARCH_TARGET"), i);
            if (i == nowRound)
            {
                if (nowState == ElectionState.REBELLION_POSSIBLE)
                {
                    roundTitle += " " + LanguageManager.instance.getText("ID_REBELLION_POSSIBLE");
                } else
                { 
                    roundTitle += " " + LanguageManager.instance.getText("ID_REBELLION_IMPOSSIBLE");
                }
            }
            else
            {
                roundTitle += " " + LanguageManager.instance.getText("ID_REBELLION_IMPOSSIBLE");
            }

            Dropdown.OptionData optionData = new Dropdown.OptionData();
            optionData.text = roundTitle;
            roundDropdown.options.Add(optionData);
        }
        if (roundDropdown.options.Count > 0)
        {
            roundDropdown.captionText.text = roundDropdown.options[0].text;
        }

        selectedRound = nowRound;
        onRoundSelected();
    }

    public void onRoundSelected()
    {
        if (roundDropdown.options.Count == 0)
        {
            selectedRound = 0;
        } else
        {
            selectedRound = roundDropdown.options.Count - roundDropdown.value;
        }

        resetRegionButton();
        statementController.resetAllInstigatorPanel();
        if (selectedRound > 0)
        {
            mapLoading.SetActive(true);
            ElectionManager.instance.requestRebellionDataList(selectedRound);
        }
    }

    public void onCountryButtonClicked(int _cid)
    {
        resetRegionButton();
        selectedCountryId = _cid;
        countryButton[_cid].interactable = false;
        statementController.updateStatement(ElectionManager.instance.getRebellionData(selectedRound, selectedCountryId), selectedRound, selectedCountryId);
    }

    public void onDataReceived(List<RebellionData> _data)
    {
        mapLoading.SetActive(false);
    }

    public void showCharacterSelectPopup(RebellionData _data)
    {
        List<int> votingCompletedIdList = ElectionManager.instance.getVotingCompletedIdList(selectedRound);
        statementController.setParticleEnabled(false);
        characterGridController.updateCharacterData(data => votingCompletedIdList.Contains(data.tokenId) && data.stakingData.purpose == StakingManager.PURPOSE_BREAK && data.country == _data.countryId);
        noCharacterText.SetActive(characterGridController.isCharacterEmpty());
        characterGridController.setOnButtonClickedCallback(list =>
        {
            characterSelctPopupAnimator.SetTrigger(dismissingTrigger);
            return true;
        });
        characterGridController.setCharacterSelectCallback(data =>
        {
            _data.tokenId = data.tokenId;
            statementController.updateEditPanel(_data);
            characterSelctPopupAnimator.SetTrigger(dismissingTrigger);
            statementController.setParticleEnabled(true);
            return true;
        });
        characterSelectPopup.SetActive(true);
    }

    public void addRebellionData(RebellionData _data)
    {
        string msg = string.Format(LanguageManager.instance.getText("ID_REBELLION_REGIST_CONFIRM_TEXT"), Const.REBELLION_REGIST_FEE);
        globalUIWindowController.showConfirmPopup(msg, () => ContractManager.instance.addRebellionData(_data));
    }

    public void returnRebellionData(RebellionData _data)
    {
        globalUIWindowController.showConfirmPopup(LanguageManager.instance.getText("ID_REBELLION_RETURN_CONFIRM_TEXT"), () => ContractManager.instance.returnRebellionData(_data));
    }

    public void revolusionRebellionData(RebellionData _data)
    {
        int currentRound = ElectionManager.instance.getElectionRound();
        if (_data.round < currentRound)
        {
            globalUIWindowController.showConfirmPopup(LanguageManager.instance.getText("ID_REBELLION_REVOLUTION_CONFIRM_PAST_TEXT"), () => ContractManager.instance.revolutionRebellionData(_data));
        }
        else
        {
            globalUIWindowController.showConfirmPopup(LanguageManager.instance.getText("ID_REBELLION_REVOLUTION_CONFIRM_TEXT"), () => ContractManager.instance.revolutionRebellionData(_data));
        }
    }

    public void joinRebel(RebellionData _data)
    {
        List<int> joinableIdList = _data.myJoinableCharacterIdList;
        if (joinableIdList.Count <= 0)
        {
            globalUIWindowController.showPopupByTextKey("ID_REBELLION_JOIN_REVEL_ERROR_TEXT", null);
            return;
        }

        if (UserManager.instance.getCoinAmount() < Utils.convertToPeb(Const.REBELLION_JOIN_FEE * joinableIdList.Count))
        {
            string errorMsg = string.Format(LanguageManager.instance.getText("ID_REBELLION_JOIN_TRT_LEAK_TEXT"), joinableIdList.Count, joinableIdList.Count * Const.REBELLION_JOIN_FEE);
            globalUIWindowController.showAlertPopup(errorMsg, null);
            return;
        }

        Dictionary<string, float> spec = calculateBattleSpec(joinableIdList, 2, 1);

        string msg = string.Format(LanguageManager.instance.getText("ID_REBELLION_JOIN_REVEL_CONFIRM_TEXT"), joinableIdList.Count * Const.REBELLION_JOIN_FEE, joinableIdList.Count, spec["score"], spec["att"], spec["def"]);
        globalUIWindowController.showConfirmPopup(msg, () => ContractManager.instance.reqJoinRebellion(_data, true));
    }

    public void joinResistance(RebellionData _data)
    {
        List<int> joinableIdList = _data.myJoinableCharacterIdList;
        if (joinableIdList.Count <= 0)
        {
            globalUIWindowController.showPopupByTextKey("ID_REBELLION_JOIN_REGISTANCE_ERROR_TEXT", null);
            return;
        }

        if (UserManager.instance.getCoinAmount() < Utils.convertToPeb(Const.REBELLION_JOIN_FEE * joinableIdList.Count))
        {
            string errorMsg = string.Format(LanguageManager.instance.getText("ID_REBELLION_JOIN_TRT_LEAK_TEXT"), joinableIdList.Count, joinableIdList.Count * Const.REBELLION_JOIN_FEE);
            globalUIWindowController.showAlertPopup(errorMsg, null);
            return;
        }

        Dictionary<string, float> spec = calculateBattleSpec(joinableIdList, 1, 2);

        string msg = string.Format(LanguageManager.instance.getText("ID_REBELLION_JOIN_REGISTANCE_CONFIRM_TEXT"), joinableIdList.Count * Const.REBELLION_JOIN_FEE, joinableIdList.Count, spec["score"], spec["att"], spec["def"]);
        globalUIWindowController.showConfirmPopup(msg, () => ContractManager.instance.reqJoinRebellion(_data, false));
    }

    public void updateRebellionData(RebellionData _data)
    {
        statementController.updateStatement(_data, _data.round, _data.countryId);
    }

    private Dictionary<string, float> calculateBattleSpec(List<int> _idList, float _attMag, float _defMag)
    {
        Dictionary<string, float> spec = new Dictionary<string, float>();

        float totalAtt = 0;
        float totalDef = 0;
        foreach (int id in _idList)
        {
            CharacterData characterData = CharacterManager.instance.getMyCharacterData(id);
            totalAtt += characterData.statusData.att;
            totalDef += characterData.statusData.def;
        }
        float totalScore = totalAtt * _attMag + totalDef * _defMag;
        spec["att"] = totalAtt;
        spec["def"] = totalDef;
        spec["score"] = totalScore;

        return spec;
    }

    private void Update()
    {
        if (string.IsNullOrEmpty(roundDropdown.captionText.text))
        {
            roundDropdown.captionText.text = LanguageManager.instance.getText("ID_NO_REGIST_ELECTION");
        }
    }

    internal void responceJoinRebellion(int _round, int _cid, int[] _list, bool _isRebelJoined)
    {
        string team = _isRebelJoined ? LanguageManager.instance.getText("ID_REBEL") : LanguageManager.instance.getText("ID_RESISTANCE");
        string msg = string.Format(LanguageManager.instance.getText("ID_REBELLION_JOIN_COMPLETED"), _list.Length, team);
        globalUIWindowController.showAlertPopup(msg, null);

        ElectionManager.instance.resetJoinableIdList(_round, _cid);
    }
}