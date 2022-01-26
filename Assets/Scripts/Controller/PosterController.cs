using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class PosterController : MonoBehaviour
{
    public enum PosterMode
    {
        OFFICE, BORDER
    }

    int round = 0;
    int cid = 0;
    CandidateData candidateData = null;

    [SerializeField]
    GameObject selectGuidePanel;
    [SerializeField]
    GameObject noDataPanel;
    [SerializeField]
    GameObject newRegistPanel;
    [SerializeField]
    GameObject candidatePanel;

    [SerializeField]
    Text newRegistText;

    [SerializeField]
    GameObject monarchIcon;
    [SerializeField]
    GameObject monarchIconParticle;
    [SerializeField]
    Text numText;

    [SerializeField]
    GameObject promiseShowingPanel;
    [SerializeField]
    GameObject promiseEditingPanel;

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
    GameObject buttonPanel;
    [SerializeField]
    GameObject voteInfoPanel;

    [SerializeField]
    GameObject registButton;
    [SerializeField]
    GameObject cancelButton;
    [SerializeField]
    GameObject editButton;
    [SerializeField]
    GameObject returnButton;
    [SerializeField]
    GameObject appointButton;

    [SerializeField]
    Slider voteRateSlider;
    [SerializeField]
    Text voteRateText;

    [SerializeField]
    GlobalUIWindowController globalUIWindowController;

    [SerializeField]
    float animatedTime = 1.5f;

    string format = "{0:0.0}%";

    private float desiredNumber;
    private float initialNumber;
    private float currentNumber;

    private ElectionOfficeWindowController electionOfficeWindowController;

    public void setElectionOfficeWindowController(ElectionOfficeWindowController _controller)
    {
        electionOfficeWindowController = _controller;
    }

    public void updatePoster(CandidateData _data, PosterMode _mode, int _round, int _cid)
    {
        selectGuidePanel.SetActive(false);
        noDataPanel.SetActive(false);
        newRegistPanel.SetActive(false);
        candidatePanel.SetActive(false);

        candidateData = _data;
        round = _round;
        cid = _cid;

        if (_data == null)
        {
            int nowRound = ElectionManager.instance.getElectionRound();
            ElectionState nowState = ElectionManager.instance.getElectionState();

            if (_mode == PosterMode.OFFICE && _round > nowRound && nowState == ElectionState.REGIST)
            {
                newRegistText.color = Color.white;
                newRegistText.text = string.Format(LanguageManager.instance.getText("ID_CANDIDATE_REGIST_EXP"), CountryManager.instance.getCountryName(cid), _round);
                newRegistPanel.SetActive(true);
            }
            else
            {
                noDataPanel.SetActive(true);
            }
        }
        else
        {
            updateCandidatePanel(_data, _mode);
            candidatePanel.SetActive(true);
        }
    }

    private void updateCandidatePanel(CandidateData _data, PosterMode _mode)
    {
        CharacterData characterData = null;
        if (_mode == PosterMode.OFFICE)
        {
            characterData = CharacterManager.instance.getMyCharacterData(_data.tokenId);
            tokenIdText.text = characterData.name;
            characterImageController.updateCharacterImage(characterData);
        } else
        {
            characterImageController.gameObject.SetActive(false);
            characterLoading.SetActive(true);
            CharacterManager.instance.getCharacterDataAsync(_data.tokenId, cData =>
            {
                if (cData.tokenId != candidateData.tokenId)
                {
                    return false;
                }
                tokenIdText.text = cData.name;
                characterImageController.updateCharacterImage(cData);
                characterImageController.gameObject.SetActive(true);
                characterLoading.SetActive(false);
                return true;
            });
        }
        numText.text = string.Format(LanguageManager.instance.getText("ID_POSTER_NUM_TITLE"), _data.id);
        nicknameText.text = _data.nickname;

        titleText.text = _data.title;
        contentsText.text = _data.contents;
        urlText.text = _data.url;

        flagImage.sprite = CountryManager.instance.getFlagImage(cid);

        characterSelectPanel.SetActive(false);
        promiseShowingPanel.SetActive(true);
        promiseEditingPanel.SetActive(false);

        int nowRound = ElectionManager.instance.getElectionRound();
        ElectionState nowState = ElectionManager.instance.getElectionState();

        bool voteFinished = round <= nowRound && _data.votingCount >= 0 && ElectionManager.instance.getTotalVotingCount(_data.round, _data.countryId) > 0;
        bool isWinner = false;
        if (voteFinished)
        {
            isWinner = ElectionManager.instance.getRanking(_data) == 0;
            monarchIcon.SetActive(isWinner);
        } else
        {
            monarchIcon.SetActive(false);
        }

        voteInfoPanel.SetActive(voteFinished && _mode == PosterMode.BORDER);
        if (voteInfoPanel.activeSelf)
        {
            resetVotingRate();
            setVotingRate((float)_data.votingCount / ElectionManager.instance.getTotalVotingCount(_data.round, _data.countryId));
            Debug.Log("updateProgress " + _data.votingCount + " / " + ElectionManager.instance.getTotalVotingCount(_data.round, _data.countryId));
        }

        buttonPanel.SetActive(_mode == PosterMode.OFFICE);
        if (buttonPanel.activeSelf)
        {
            registButton.SetActive(false);
            cancelButton.SetActive(round > nowRound && nowState == ElectionState.REGIST);
            editButton.SetActive(round > nowRound && nowState == ElectionState.REGIST);
            returnButton.SetActive(voteFinished && !isWinner && !_data.nftReturned);
            appointButton.SetActive(voteFinished && isWinner && !_data.nftReturned);
        }
    }

    public void updateEditPanel(CandidateData _data)
    {
        CharacterData characterData = _data.tokenId == -1 ? null : CharacterManager.instance.getMyCharacterData(_data.tokenId);
        numText.text = _data.id == -1 ? string.Format(LanguageManager.instance.getText("ID_CANDIDATE_REGIST_TITLE"), CountryManager.instance.getCountryName(_data.countryId), _data.round) : string.Format(LanguageManager.instance.getText("ID_POSTER_NUM_TITLE"), _data.id);
        nicknameText.text = _data.nickname;
        tokenIdText.text = characterData == null ? "" : characterData.name;

        characterImageController.gameObject.SetActive(characterData != null);
        characterSelectPanel.SetActive(true);
        promiseShowingPanel.SetActive(false);
        promiseEditingPanel.SetActive(true);

        if (characterData != null)
        {
            characterImageController.updateCharacterImage(characterData);
        }
        titleInputField.text = _data.title;
        contentsInputField.text = _data.contents;
        urlInputField.text = _data.url;

        flagImage.sprite = CountryManager.instance.getFlagImage(cid);

        monarchIcon.SetActive(false);
        voteInfoPanel.SetActive(false);
        buttonPanel.SetActive(true);

        registButton.SetActive(true);
        cancelButton.SetActive(false);
        editButton.SetActive(false);
        returnButton.SetActive(false);
        appointButton.SetActive(false);
    }

    public void resetAllPosterPanel()
    {
        selectGuidePanel.SetActive(true);
        newRegistPanel.SetActive(false);
        noDataPanel.SetActive(false);
        candidatePanel.SetActive(false);
    }

    public void onRegistButtonClicked()
    {
        if (candidateData.tokenId < 0)
        {
            globalUIWindowController.showPopupByTextKey("ID_PLEASE_SELECT_CHARACTER", null);
            return;
        }
        if (string.IsNullOrEmpty(titleInputField.text))
        {
            globalUIWindowController.showPopupByTextKey("ID_PLEASE_FILL_TITLE", null);
            return;
        }

        if(string.IsNullOrEmpty(contentsInputField.text))
        {
            globalUIWindowController.showPopupByTextKey("ID_PLEASE_FILL_CONTENTS", null);
            return;
        }
        candidateData.title = titleInputField.text;
        candidateData.contents = contentsInputField.text;
        candidateData.url = urlInputField.text;
        electionOfficeWindowController?.addCandidateData(candidateData);
    }

    public void onRegistCancelButtonClicked()
    {
        electionOfficeWindowController?.cancelCandidateData(candidateData);
    }

    public void onEditButtonClicked()
    {
        updateEditPanel(candidateData);
        candidatePanel.SetActive(true);
    }

    public void setParticleEnabled(bool _set)
    {
        monarchIconParticle.SetActive(_set);
        characterImageController.setParticleEnabled(_set);
    }

    public void onNewButtonClicked()
    {
        if (UserManager.instance.getCoinAmount() < Utils.convertToPeb(Const.MONARCH_REGIST_FEE))
        {
            newRegistText.color = Color.red;
            newRegistText.text = string.Format(LanguageManager.instance.getText("ID_ELECTION_REGIST_FEE_LEAK"), Const.MONARCH_REGIST_FEE);
            return;
        }

        candidateData = new CandidateData();
        candidateData.countryId = cid;
        candidateData.round = round;
        candidateData.id = -1;
        candidateData.tokenId = -1;
        candidateData.addr = UserManager.instance.getWalletAddress();
        candidateData.nickname = UserManager.instance.getNickname();
        updateEditPanel(candidateData);

        newRegistPanel.SetActive(false);
        candidatePanel.SetActive(true);
    }

    public void onReturnButtonClicked()
    {
        electionOfficeWindowController?.returnCandidateData(candidateData);
    }

    public void onAppointmentButtonClicked()
    {
        electionOfficeWindowController?.appointmentCandidateData(candidateData);
    }

    public void onCharacterAddButtonClicked()
    {
        electionOfficeWindowController?.showCharacterSelectPopup(candidateData);
    }

    public void onUrlButtonClicked()
    {
        if (candidateData != null && !string.IsNullOrEmpty(candidateData.url))
        {
            Application.ExternalEval("window.open(\"" + candidateData.url + "\")");
        }
    }

    public void setVotingRate(float _number)
    {
        initialNumber = currentNumber;
        desiredNumber = _number;
    }

    public void resetVotingRate()
    {
        initialNumber = desiredNumber = currentNumber = 0;
        voteRateText.text = string.Format(format, currentNumber);
        voteRateSlider.value = 0;
    }

    private void Update()
    {
        if (currentNumber != desiredNumber)
        {
            float delta = (animatedTime * Time.deltaTime) * (desiredNumber - initialNumber);
            if (initialNumber < desiredNumber)
            {
                currentNumber += delta;
                if (currentNumber >= desiredNumber)
                {
                    currentNumber = desiredNumber;
                }
            }
            else
            {
                currentNumber += delta;
                if (currentNumber <= desiredNumber)
                {
                    currentNumber = desiredNumber;
                }
            }

            voteRateText.text = string.Format(format, currentNumber * 100);
            voteRateSlider.value = currentNumber;
        }
    }
}