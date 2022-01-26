using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class SquareBorderWindowController : MonoBehaviour, CandidateObserver
{
    private int selectedRound = 0;
    private int selectedCountryId = -1;
    private int selectedIdx = 0;
    private List<CandidateData> filteredCandidateList;

    [SerializeField]
    Dropdown roundDropdown;
    [SerializeField]
    Button[] countryButton;
    [SerializeField]
    GameObject mapLoading;

    [SerializeField]
    Button prevButton;
    [SerializeField]
    Button nextButton;

    [SerializeField]
    PosterController posterController;


    private void OnEnable()
    {
        ElectionManager.instance.addObserver(this);

        selectedRound = 0;
        selectedCountryId = -1;
        selectedIdx = 0;

        addRoundInfo();
        resetRegionButton();
        posterController.resetAllPosterPanel();
        updatePosterControlButton();
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

        if (selectedRound > ElectionManager.instance.getElectionRound())
        {
            filteredCandidateList = ElectionManager.instance.getCandidateListWithIdSorting(selectedRound, selectedCountryId);
        } else
        {
            filteredCandidateList = ElectionManager.instance.getCandidateListWithRankSorting(selectedRound, selectedCountryId);
        }

        selectedIdx = 0;
        updatePoster();
        updatePosterControlButton();
    }

    public void onCandidateListReceived(List<CandidateData> _list)
    {
        mapLoading.SetActive(false);
    }

    private void updatePoster()
    {
        if (filteredCandidateList == null || filteredCandidateList.Count <= 0)
        {
            posterController.updatePoster(null, PosterController.PosterMode.BORDER, selectedRound, selectedCountryId);
        }
        else
        {
            posterController.updatePoster(filteredCandidateList[selectedIdx], PosterController.PosterMode.BORDER, selectedRound, selectedCountryId);
        }
    }

    public void onPosterPrevButtonClicked()
    {
        selectedIdx--;
        if (selectedIdx < 0)
        {
            selectedIdx = 0;
        }

        updatePoster();
        updatePosterControlButton();
    }

    public void onPosterNextButtonClicked()
    {
        selectedIdx++;
        int maxCount = filteredCandidateList == null ? 0 : filteredCandidateList.Count;
        if (selectedIdx > maxCount - 1)
        {
            selectedIdx = maxCount - 1;
        }

        updatePoster();
        updatePosterControlButton();
    }

    public void updatePosterControlButton()
    {
        if (filteredCandidateList == null)
        {
            prevButton.interactable = false;
            nextButton.interactable = false;
            return;
        }

        prevButton.interactable = selectedIdx > 0;
        nextButton.interactable = selectedIdx < filteredCandidateList.Count - 1;
    }

    private void Update()
    {
        if (string.IsNullOrEmpty(roundDropdown.captionText.text))
        {
            roundDropdown.captionText.text = LanguageManager.instance.getText("ID_NO_REGIST_ELECTION");
        }
    }
}