using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PollsPlaceController : MonoBehaviour, CandidateObserver
{
    [SerializeField]
    PollsPanelController[] pollsPanelControllerArr;

    [SerializeField]
    Text guideText;

    [SerializeField]
    GameObject loading;

    [SerializeField]
    GameObject votingButton;
    [SerializeField]
    Text votingButtonText;

    [SerializeField]
    GameObject confirmPopup;

    [SerializeField]
    GlobalUIWindowController globalUIWindowController;

    private int round = 0;
    private List<int> characterIdList = null;
    private int[] votingCount = new int[CountryManager.COUNTRY_MAX];

    private void OnEnable()
    {
        
        ElectionState state = ElectionManager.instance.getElectionState();
        if (state != ElectionState.ELECTION)
        {
            resetAllPollPanel();
            loading.SetActive(false);
            votingButton.SetActive(false);
            guideText.text = LanguageManager.instance.getText("ID_NO_ELECTION_POLLS_GUIDE");
            return;
        }

        guideText.text = "";
        foreach (PollsPanelController pollsPanelController in pollsPanelControllerArr)
        {
            pollsPanelController.gameObject.SetActive(false);
            pollsPanelController.resetToggles(null);
        }

        loading.SetActive(true);

        round = ElectionManager.instance.getElectionRound() + 1;
        votingButtonText.text = string.Format(LanguageManager.instance.getText("ID_ELECTION_POLLS_BUTTON"), round);

        ElectionManager.instance.addObserver(this);
        ElectionManager.instance.requestRoundCandidateList(round);
    }

    private void OnDisable()
    {
        ElectionManager.instance.removeObserver(this);
    }

    public void onCandidateListReceived(List<CandidateData> _candidateList)
    {
        ElectionManager.instance.requestNotVotedCharacterList(round, _characterIdList =>
        {
            updatePollsPanel(_characterIdList);
            return true;
        });
    }

    private void resetAllPollPanel()
    {
        foreach (PollsPanelController pollsPanelController in pollsPanelControllerArr)
        {
            pollsPanelController.gameObject.SetActive(false);
            pollsPanelController.resetToggles(null);
        }
    }

    public void updatePollsPanel(List<int> _characterIdList)
    {
        loading.SetActive(false);
        if (_characterIdList.Count == 0)
        {
            resetAllPollPanel();
            votingButton.SetActive(false);
            guideText.text = LanguageManager.instance.getText("ID_NO_ELECTION_VOTING_COUNT");
            return;
        }

        for (int i = 0; i < votingCount.Length; i++)
        {
            votingCount[i] = 0;
        }

        characterIdList = _characterIdList;
        foreach(int tokenId in _characterIdList)
        {
            CharacterData characterData = CharacterManager.instance.getMyCharacterData(tokenId);
            if (characterData == null)
            {
                continue;
            }

            votingCount[characterData.country]++;
        }

        bool voteBtnEnabled = false;
        for (int cid = 0; cid < pollsPanelControllerArr.Length; cid++)
        {
            pollsPanelControllerArr[cid].gameObject.SetActive(votingCount[cid] > 0);

            if (pollsPanelControllerArr[cid].gameObject.activeSelf)
            {
                pollsPanelControllerArr[cid].updatePanel(votingCount[cid], ElectionManager.instance.getCandidateListWithIdSorting(round, cid));
                voteBtnEnabled = true;
            } else
            {
                pollsPanelControllerArr[cid].resetToggles(null);
            }
        }

        votingButton.SetActive(voteBtnEnabled);
    }

    public void onVoteButtonClicked()
    {
        // TODO

        int[] candidateIds = new int[pollsPanelControllerArr.Length];
        int[] voteCounts = new int[pollsPanelControllerArr.Length];
        List<int> characterIdList = new List<int>();

        for (int cid = 0; cid < pollsPanelControllerArr.Length; cid++)
        {
            int selectedId = pollsPanelControllerArr[cid].getSelectedId();
            candidateIds[cid] = selectedId;
            voteCounts[cid] = selectedId <= 0 ? 0 : votingCount[cid];

            if (selectedId > 0)
            {
                foreach (CharacterData data in CharacterManager.instance.getMyCharacterList())
                {
                    if (data.country == cid)
                    {
                        characterIdList.Add(data.tokenId);
                    }
                }
            }
        }

        if (characterIdList.Count <= 0)
        {
            globalUIWindowController.showPopupByTextKey("ID_VOTING_AT_LEAT_ONE", null);
            return;
        }
        ContractManager.instance.reqVoteMonarchElection(round, candidateIds, voteCounts, characterIdList.ToArray());
    }

    internal void responceVoteCompleted(int[] _voteCompletedIdList)
    {
        foreach(int characterId in _voteCompletedIdList)
        {
            characterIdList.Remove(characterId);
        }

        updatePollsPanel(characterIdList);
    }
}
