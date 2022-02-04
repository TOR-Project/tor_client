using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class ElectionOfficeWindowController : MonoBehaviour, CandidateObserver
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
    PosterController posterController;

    [SerializeField]
    GameObject characterSelectPopup;
    [SerializeField]
    CharacterGridController characterGridController;
    [SerializeField]
    GameObject noCharacterText;
    [SerializeField]
    Animator characterSelctPopupAnimator;

    [SerializeField]
    GameObject congratulationPopup;
    [SerializeField]
    Text congratulationPopupText;

    private string dismissingTrigger = "dismissing";


    private void OnEnable()
    {
        ElectionManager.instance.addObserver(this);
        guideText.text = string.Format(LanguageManager.instance.getText("ID_ELECTION_OFFICE_GUIDE"), Const.MONARCH_REGIST_FEE);
        addRoundInfo();
        resetRegionButton();
        posterController.resetAllPosterPanel();
        posterController.setElectionOfficeWindowController(this);
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
        if ((int)(nowState) >= (int)(ElectionState.REGIST))
        {
            nowRound++;
        }

        for (int i = nowRound; i > 0; i--)
        {
            string roundTitle = string.Format(LanguageManager.instance.getText("ID_N_NEXT_MONARCH_ELECTION"), i);
            if (i == nowRound)
            {
                if (nowState == ElectionState.REGIST)
                {
                    roundTitle += " " + LanguageManager.instance.getText("ID_ELECTION_REGISTING");
                } else if (nowState == ElectionState.ELECTION)
                {
                    roundTitle += " " + LanguageManager.instance.getText("ID_ELECTION_VOTING");
                } else
                {
                    roundTitle += " " + LanguageManager.instance.getText("ID_ELECTION_ENDED");
                }
            }
            else
            {
                roundTitle += " " + LanguageManager.instance.getText("ID_ELECTION_ENDED");
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
        posterController.resetAllPosterPanel();
        if (selectedRound > 0)
        {
            mapLoading.SetActive(true);
            ElectionManager.instance.requestRoundCandidateList(selectedRound);
        }
    }

    public void onCountryButtonClicked(int _cid)
    {
        resetRegionButton();
        selectedCountryId = _cid;
        countryButton[_cid].interactable = false;
        posterController.updatePoster(ElectionManager.instance.getMyCandidateData(selectedRound, selectedCountryId), PosterController.PosterMode.OFFICE, selectedRound, selectedCountryId);
    }

    public void onCandidateListReceived(List<CandidateData> _list)
    {
        mapLoading.SetActive(false);
    }

    public void showCharacterSelectPopup(CandidateData _data)
    {
        posterController.setParticleEnabled(false);
        characterGridController.updateCharacterData(data => data.stakingData.purpose == StakingManager.PURPOSE_BREAK && data.country == _data.countryId);
        noCharacterText.SetActive(characterGridController.isCharacterEmpty());
        characterGridController.setCharacterSelectCallback(data =>
        {
            _data.tokenId = data.getCharacterData().tokenId;
            posterController.updateEditPanel(_data);
            characterSelctPopupAnimator.SetTrigger(dismissingTrigger);
            posterController.setParticleEnabled(true);
            return true;
        });
        characterSelectPopup.SetActive(true);
    }

    public void addCandidateData(CandidateData _data)
    {
        if (_data.id == -1)
        {
            string msg = string.Format(LanguageManager.instance.getText("ID_REGIST_CONFIRM_TEXT"), Const.MONARCH_REGIST_FEE);
            globalUIWindowController.showConfirmPopup(msg, () => ContractManager.instance.addCandidateData(_data));
        } else
        {
            globalUIWindowController.showConfirmPopup(LanguageManager.instance.getText("ID_EDIT_CONFIRM_TEXT"), () => ContractManager.instance.editCandidateData(_data));
        }
    }

    public void editCandidateData(CandidateData _data)
    {
        posterController.updateEditPanel(_data);
    }

    public void cancelCandidateData(CandidateData _data)
    {
        globalUIWindowController.showConfirmPopup(LanguageManager.instance.getText("ID_CANCEL_CONFIRM_TEXT"), () => ContractManager.instance.cancelCandidateData(_data));
    }

    public void returnCandidateData(CandidateData _data)
    {
        globalUIWindowController.showConfirmPopup(LanguageManager.instance.getText("ID_RETURN_CONFIRM_TEXT"), () => ContractManager.instance.returnCandidateData(_data));
    }

    public void appointmentCandidateData(CandidateData _data)
    {
        globalUIWindowController.showConfirmPopup(LanguageManager.instance.getText("ID_APPOINTMENT_CONFIRM_TEXT"), () => ContractManager.instance.appointmentCandidateData(_data));
    }

    internal void responceAppointmentMonarch(CandidateData _data)
    {
        string castleName = string.Format(LanguageManager.instance.getText("ID_COUNTRY_CASTLE"), CountryManager.instance.getCountryName(_data.countryId));
        string msg = string.Format(LanguageManager.instance.getText("ID_CONGRATULATION_APPOINTMENT"), CountryManager.instance.getCountryName(_data.countryId), _data.round, castleName);
        congratulationPopupText.text = msg;
        congratulationPopup.SetActive(true);
        posterController.setParticleEnabled(false);
    }

    public void updateCandidateData(CandidateData _data)
    { 
        posterController.updatePoster(_data.canceled ? null : _data, PosterController.PosterMode.OFFICE, _data.round, _data.countryId);
    }

    private void Update()
    {
        if (string.IsNullOrEmpty(roundDropdown.captionText.text))
        {
            roundDropdown.captionText.text = LanguageManager.instance.getText("ID_NO_REGIST_ELECTION");
        }
    }
}