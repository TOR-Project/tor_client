using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AgendaPanelController : MonoBehaviour, BlockNumberObserever
{

    [SerializeField]
    int defulatEditItemCount = 2;

    [Header("Edit panel")]
    [SerializeField]
    GameObject editPanel;
    [SerializeField]
    TMP_InputField titleInputField;
    [SerializeField]
    TMP_InputField contentsInputField;
    [SerializeField]
    Text editProposerNicknameText;
    [SerializeField]
    GameObject editTorLogo;
    [SerializeField]
    GameObject editProposalItemPrefab;
    [SerializeField]
    RectTransform editProposalItemContentsRT;
    [SerializeField]
    TMP_InputField periodInputField;
    [SerializeField]
    Text periodExpText;
    [SerializeField]
    Toggle blindToggle;
    [SerializeField]
    Text stakingCountText;
    [SerializeField]
    GameObject stakingCharacterSelectBtn;
    [SerializeField]
    Text stakingExpText;
    [SerializeField]
    GameObject characterSelectPopup;
    [SerializeField]
    CharacterGridController characterGridController;
    [SerializeField]
    GameObject noCharacterText;
    [SerializeField]
    Animator characterSelctPopupAnimator;

    [Header("Show panel")]
    [SerializeField]
    GameObject showPanel;
    [SerializeField]
    Text titleText;
    [SerializeField]
    Text contentsText;
    [SerializeField]
    Text showProposerNicknameText;
    [SerializeField]
    GameObject showTorLogo;
    [SerializeField]
    GameObject showProposalItemPrefab;
    [SerializeField]
    RectTransform showProposalItemContentsRT;
    [SerializeField]
    GameObject blindExp;
    [SerializeField]
    Text votingCountText;

    [Header("Control panel")]
    [SerializeField]
    Text stateText;
    [SerializeField]
    GameObject returnButton;
    [SerializeField]
    GameObject cancelButton;
    [SerializeField]
    GameObject offerButton;
    [SerializeField]
    GameObject voteButton;

    [Header("Other")]
    [SerializeField]
    GlobalUIWindowController globalUIWindowController;

    private string dismissingTrigger = "dismissing";
    AgendaData agendaData;

    private void OnEnable()
    {
        int minDay = Const.GOVERNANCE_MIN_PROPOSER_PERIOD / 86400;
        int maxDay = Const.GOVERNANCE_MAX_PROPOSER_PERIOD / 86400;

        periodExpText.text = string.Format(LanguageManager.instance.getText("ID_VOTING_PERIOD_EXP"), minDay, Const.GOVERNANCE_MIN_PROPOSER_PERIOD, maxDay, Const.GOVERNANCE_MAX_PROPOSER_PERIOD);

        stakingExpText.text = string.Format(LanguageManager.instance.getText("ID_CHARACTER_STAKING_EXP"), Const.GOVERNANCE_CHARACTER_COUNT);

        SystemInfoManager.instance.addBlockNumberObserver(this);
    }

    private void OnDisable()
    {
        SystemInfoManager.instance.removeBlockNumberObserver(this);
    }

    internal void updateAgendaData(List<AgendaData> list)
    {
        if (editPanel.activeSelf)
        {
            return;
        }

        if (showPanel.activeSelf && agendaData.id >= 0)
        {
            foreach(AgendaData ad in list)
            {
                if (agendaData.id == ad.id)
                {
                    showShowingAgendaPanel(ad);
                    return;
                }
            }
        }
    }

    public void showEditingAgendaPanel(AgendaData _agendaData)
    {
        agendaData = _agendaData;

        stateText.text = "";

        titleInputField.text = _agendaData.title;
        contentsInputField.text = _agendaData.contents;
        editProposerNicknameText.text = string.Format(LanguageManager.instance.getText("ID_OFFER_NICKNAME"), _agendaData.nickname);
        editTorLogo.SetActive(string.Equals(_agendaData.nickname, Const.ADMIN_REP_NICKNAME));

        for (int idx = defulatEditItemCount; idx < editProposalItemContentsRT.childCount - 1; idx++)
        {
            Destroy(editProposalItemContentsRT.GetChild(idx).gameObject);
        }

        for (int idx = 0; idx < _agendaData.items.Count; idx++)
        {
            string itemContents = _agendaData.items[idx];
            AgendaEditItemRowController rowController;
            if (defulatEditItemCount > idx)
            {
                GameObject childObject = editProposalItemContentsRT.GetChild(idx).gameObject;
                rowController = childObject.GetComponent<AgendaEditItemRowController>();
                rowController.gameObject.SetActive(true);
            }
            else
            {
                GameObject propertyRow = Instantiate(editProposalItemPrefab, editProposalItemContentsRT, true);
                propertyRow.transform.localScale = UnityEngine.Vector3.one;
                propertyRow.transform.SetSiblingIndex(editProposalItemContentsRT.childCount - 2);
                rowController = propertyRow.GetComponent<AgendaEditItemRowController>();
                rowController.setCallback(removeEditItem);
            }
            rowController.updateItemContents(itemContents);
        }

        periodInputField.text = _agendaData.periodBlock.ToString();
        blindToggle.isOn = _agendaData.blind;
        stakingCountText.text = agendaData.proposalTokenIdList.Count + "/" + Const.GOVERNANCE_CHARACTER_COUNT;
        stakingCountText.gameObject.SetActive(!string.Equals(_agendaData.nickname, Const.ADMIN_REP_NICKNAME));
        stakingCharacterSelectBtn.SetActive(!string.Equals(_agendaData.nickname, Const.ADMIN_REP_NICKNAME));

        returnButton.SetActive(false);
        cancelButton.SetActive(false);
        offerButton.SetActive(true);
        voteButton.SetActive(false);

        editPanel.SetActive(true);
        showPanel.SetActive(false);
    }

    public void addEditItem()
    {
        GameObject propertyRow = Instantiate(editProposalItemPrefab, editProposalItemContentsRT, true);
        propertyRow.transform.localScale = UnityEngine.Vector3.one;
        propertyRow.transform.SetSiblingIndex(editProposalItemContentsRT.childCount - 2);
        AgendaEditItemRowController rowController = propertyRow.GetComponent<AgendaEditItemRowController>();
        rowController.setCallback(removeEditItem);
        agendaData.items.Add("");
    }

    public bool removeEditItem(AgendaEditItemRowController _controller)
    {
        for (int idx = 0; idx < editProposalItemContentsRT.childCount - 1; idx++)
        {
            GameObject itemObject = editProposalItemContentsRT.GetChild(idx).gameObject;
            if (itemObject == _controller.gameObject)
            {
                Destroy(itemObject);
                agendaData.items.RemoveAt(idx);
                return true;
            }
        }

        return false;
    }

    public void showShowingAgendaPanel(AgendaData _agendaData)
    {
        agendaData = _agendaData;

        titleText.text = _agendaData.title;
        contentsText.text = _agendaData.contents;
        showProposerNicknameText.text = string.Format(LanguageManager.instance.getText("ID_OFFER_NICKNAME"), _agendaData.nickname);
        showTorLogo.SetActive(string.Equals(_agendaData.nickname, Const.ADMIN_REP_NICKNAME));

        for (int idx = 0; idx < showProposalItemContentsRT.childCount; idx++)
        {
            showProposalItemContentsRT.GetChild(idx).gameObject.SetActive(false);
        }

        resetToggles(null);

        bool isFinishedAgenda = _agendaData.endBlock <= SystemInfoManager.instance.blockNumber;
        int maxVoteCount = 0;
        for (int idx = 0; idx < _agendaData.items.Count; idx++)
        {
            if (maxVoteCount < _agendaData.votingData[idx])
            {
                maxVoteCount = _agendaData.votingData[idx];
            }
        }

        for (int idx = 0; idx < _agendaData.items.Count; idx++)
        {
            string itemContents = _agendaData.items[idx];
            int voteCount = _agendaData.votingData[idx];
            AgendaItemRowController rowController;
            if (showProposalItemContentsRT.childCount > idx)
            {
                GameObject childObject = showProposalItemContentsRT.GetChild(idx).gameObject;
                rowController = childObject.GetComponent<AgendaItemRowController>();
                rowController.gameObject.SetActive(true);
            }
            else
            {
                GameObject propertyRow = Instantiate(showProposalItemPrefab, showProposalItemContentsRT, true);
                propertyRow.transform.localScale = UnityEngine.Vector3.one;
                rowController = propertyRow.GetComponent<AgendaItemRowController>();
                rowController.setResetToggleAction(resetToggles);
            }
            rowController.updateItem(itemContents, (_agendaData.blind && !isFinishedAgenda) || _agendaData.canceled, voteCount, maxVoteCount);
            rowController.setActiveToggle(!isFinishedAgenda && !_agendaData.canceled);
        }

        if (_agendaData.canceled)
        {
            stateText.text = LanguageManager.instance.getText("ID_PROGRESS_CANCELED");
            blindExp.SetActive(false);
            votingCountText.gameObject.SetActive(false);
        }
        else if (isFinishedAgenda)
        {
            stateText.text = LanguageManager.instance.getText("ID_PROGRESS_DONE");
            blindExp.SetActive(false);
            votingCountText.gameObject.SetActive(false);
        }
        else
        {
            long leftBlock = _agendaData.endBlock - SystemInfoManager.instance.blockNumber;
            if (leftBlock < 0)
            {
                leftBlock = 0;
            }
            stateText.text = string.Format(LanguageManager.instance.getText("ID_PROGRESS_ING_N_BLOCK"), leftBlock);
            votingCountText.gameObject.SetActive(true);
            blindExp.SetActive(_agendaData.blind);
            votingCountText.text = string.Format(LanguageManager.instance.getText("ID_AGENDA_VOTING_COUNT"), _agendaData.notVotedIdList.Length);
        }

        returnButton.SetActive(UserManager.instance.isMyAddress(_agendaData.address) && isFinishedAgenda && !_agendaData.nftReturned);
        cancelButton.SetActive(UserManager.instance.isMyAddress(_agendaData.address) && !_agendaData.canceled && !isFinishedAgenda && !_agendaData.nftReturned);
        offerButton.SetActive(false);
        voteButton.SetActive(!_agendaData.canceled && !isFinishedAgenda);

        editPanel.SetActive(false);
        showPanel.SetActive(true);
    }

    internal bool resetToggles(AgendaItemRowController _trigger)
    {
        for (int i = 0; i < showProposalItemContentsRT.childCount; i++)
        {
            GameObject childObject = showProposalItemContentsRT.GetChild(i).gameObject;
            AgendaItemRowController rowController = childObject.GetComponent<AgendaItemRowController>();
            if (rowController != null && rowController != _trigger)
            {
                rowController.resetToggle();
            }
        }

        return true;
    }

    public void onBlockNumberChanged(long _block)
    {
        if (!showPanel.activeSelf || agendaData == null || agendaData.canceled || agendaData.endBlock <= _block)
        {
            return;
        }

        long leftBlock = agendaData.endBlock - _block;
        if (leftBlock < 0)
        {
            leftBlock = 0;
        }
        stateText.text = string.Format(LanguageManager.instance.getText("ID_PROGRESS_ING_N_BLOCK"), leftBlock);
    }

    public void showCharacterSelectPopup()
    {
        characterGridController.updateCharacterData(data => data.stakingData.purpose == StakingManager.PURPOSE_BREAK);
        noCharacterText.SetActive(characterGridController.isCharacterEmpty());
        characterGridController.setOnButtonClickedCallback(list =>
        {
            agendaData.proposalTokenIdList.Clear();
            agendaData.proposalTokenIdList.AddRange(list.ConvertAll(cd => cd.tokenId));
            characterSelctPopupAnimator.SetTrigger(dismissingTrigger);
            stakingCountText.text = agendaData.proposalTokenIdList.Count + "/" + Const.GOVERNANCE_CHARACTER_COUNT;
            return true;
        });
        characterSelectPopup.SetActive(true);
    }

    public void saveAgendaDataFromEditPanel()
    {
        if (!editPanel.activeSelf)
        {
            return;
        }

        agendaData.title = titleInputField.text.Trim();
        agendaData.summary = "";
        agendaData.contents = contentsInputField.text.Trim();
        agendaData.items.Clear();
        for (int idx = 0; idx < editProposalItemContentsRT.childCount - 1; idx++)
        {
            GameObject childObject = editProposalItemContentsRT.GetChild(idx).gameObject;
            AgendaEditItemRowController rowController = childObject.GetComponent<AgendaEditItemRowController>();
            agendaData.items.Add(rowController.getContents());
        }
        agendaData.periodBlock = 0;
        long.TryParse(periodInputField.text, out agendaData.periodBlock);
        agendaData.blind = blindToggle.isOn;
    }

    public void onOfferButtonClicked()
    {
        saveAgendaDataFromEditPanel();

        if (string.IsNullOrEmpty(agendaData.title))
        {
            globalUIWindowController.showPopupByTextKey("ID_AGENDA_ALERT_ENTER_TITLE", null);
            return;
        }

        if (string.IsNullOrEmpty(agendaData.contents))
        {
            globalUIWindowController.showPopupByTextKey("ID_AGENDA_ALERT_ENTER_CONTENTS", null);
            return;
        }

        foreach (string item in agendaData.items)
        {
            if (string.IsNullOrEmpty(item))
            {
                globalUIWindowController.showPopupByTextKey("ID_AGENDA_ALERT_ENTER_ITEMS", null);
                return;
            }
        }

        if (agendaData.periodBlock == 0)
        {
            globalUIWindowController.showPopupByTextKey("ID_AGENDA_ALERT_ENTER_PERIOD", null);
            return;
        }

        if (agendaData.periodBlock < Const.GOVERNANCE_MIN_PROPOSER_PERIOD || Const.GOVERNANCE_MAX_PROPOSER_PERIOD < agendaData.periodBlock)
        {
            globalUIWindowController.showPopupByTextKey("ID_AGENDA_ALERT_INVALID_PERIOD", null);
            return;
        }

        if (!UserManager.instance.isAdminAccount() && Const.GOVERNANCE_CHARACTER_COUNT != agendaData.proposalTokenIdList.Count)
        {
            globalUIWindowController.showPopupByTextKey("ID_AGENDA_ALERT_LEAK_CHARACTER", null);
            return;
        }

        agendaData.notVotedIdList = CharacterManager.instance.getMyCharacterList().ConvertAll(cd => cd.tokenId).ToArray();
        globalUIWindowController.showConfirmPopup(LanguageManager.instance.getText("ID_AGENDA_CONFIRM_OFFER"), () => ContractManager.instance.reqOfferAgenda(agendaData));
    }

    public void onCancelButtonClicked()
    {
        globalUIWindowController.showConfirmPopup(LanguageManager.instance.getText("ID_AGENDA_CONFIRM_CANCEL"), () => ContractManager.instance.reqCancelAgenda(agendaData));
    }

    public void onReturnButtonClicked()
    {
        if (agendaData.nftReturned)
        {
            globalUIWindowController.showPopupByTextKey("ID_AGENDA_ALERT_ALREADY_RETURNED", null);
            return;
        }

        globalUIWindowController.showConfirmPopup(LanguageManager.instance.getText("ID_AGENDA_CONFIRM_RETURN"), () => ContractManager.instance.reqReturnCharacterFromAgenda(agendaData));
    }

    public void onVoteButtonClicked()
    {
        if (agendaData.notVotedIdList.Length <= 0)
        {
            globalUIWindowController.showPopupByTextKey("ID_AGENDA_ALERT_NO_VOTEING_COUNT", null);
            return;
        }

        int selectedIdx = -1;
        for (int idx = 0; idx < showProposalItemContentsRT.childCount; idx++)
        {
            GameObject childObject = showProposalItemContentsRT.GetChild(idx).gameObject;
            AgendaItemRowController rowController = childObject.GetComponent<AgendaItemRowController>();
            if (rowController.isOn())
            {
                selectedIdx = idx;
                break;
            }
        }

        if (selectedIdx == -1)
        {
            globalUIWindowController.showPopupByTextKey("ID_AGENDA_ALERT_SELECT_ITEM", null);
            return;
        }

        globalUIWindowController.showConfirmPopup(LanguageManager.instance.getText("ID_AGENDA_CONFIRM_VOTE"), () => ContractManager.instance.reqVoteAgenda(selectedIdx, agendaData));
    }
}
