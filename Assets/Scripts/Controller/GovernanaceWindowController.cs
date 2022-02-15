using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class GovernanaceWindowController : MonoBehaviour, GovernanceObserver
{
    [SerializeField]
    Text expText;
    [SerializeField]
    GameObject loading;
    [SerializeField]
    GameObject agendaTitleRowPrefab;
    [SerializeField]
    RectTransform agendaListContentsRT;
    [SerializeField]
    AgendaPanelController agendaPanelController;

    AgendaData tempAgendaData = null;

    private void OnEnable()
    {
        expText.text = string.Format(LanguageManager.instance.getText("ID_GOVERNANCE_EXP"), Const.GOVERNANCE_CHARACTER_COUNT, Const.GOVERNANCE_CHARACTER_COUNT);
        loading.SetActive(true);
        GovernanceManager.instance.addObserver(this);
        GovernanceManager.instance.requestAgendaList();
    }

    private void OnDisable()
    {
        GovernanceManager.instance.removeObserver(this);
    }

    public void showAgenda(AgendaData _data)
    {
        agendaPanelController.showShowingAgendaPanel(_data);
        agendaPanelController.gameObject.SetActive(true);
    }

    public void showOfferPanel()
    {
        if (tempAgendaData == null)
        {
            tempAgendaData = createEmptyAgenda();
        }

        agendaPanelController.showEditingAgendaPanel(tempAgendaData);
        agendaPanelController.gameObject.SetActive(true);
    }

    private void clearAgendaTitleContainer()
    {
        for (int i = 0; i < agendaListContentsRT.childCount; i++)
        {
            agendaListContentsRT.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void onAgendaListReceived(List<AgendaData> _list)
    {
        clearAgendaTitleContainer();

        for (int idx = 0; idx < _list.Count; idx++)
        {
            AgendaData ad = _list[idx];
            AgendaTitleRowController rowController;
            if (agendaListContentsRT.childCount > idx)
            {
                GameObject childObject = agendaListContentsRT.GetChild(idx).gameObject;
                rowController = childObject.GetComponent<AgendaTitleRowController>();
                rowController.gameObject.SetActive(true);
            }
            else
            {
                GameObject propertyRow = Instantiate(agendaTitleRowPrefab, agendaListContentsRT, true);
                propertyRow.transform.localScale = UnityEngine.Vector3.one;
                rowController = propertyRow.GetComponent<AgendaTitleRowController>();
                rowController.setGovernanceWindowController(this);
            }
            rowController.updateAgendaData(ad);
        }

        agendaPanelController.updateAgendaData(_list);

        loading.SetActive(false);
    }

    internal void responseOfferAgenda(AgendaData _agendaData)
    {
        agendaPanelController.showShowingAgendaPanel(_agendaData);
        tempAgendaData = createEmptyAgenda();
    }

    private AgendaData createEmptyAgenda()
    {
        AgendaData agendaData = new AgendaData();
        agendaData.id = -1;
        agendaData.address = UserManager.instance.getWalletAddress();
        agendaData.nickname = UserManager.instance.isAdminAccount() ? Const.ADMIN_REP_NICKNAME : UserManager.instance.getNickname();
        agendaData.items = new List<string> { "", "" };
        agendaData.votingData = new int[] { 0, 0 };
        agendaData.proposalTokenIdList = new List<int> {};
        agendaData.notVotedIdList = new int[0];
        return agendaData;
    }
}