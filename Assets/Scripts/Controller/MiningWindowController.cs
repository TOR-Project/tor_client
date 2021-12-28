using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Numerics;

public class MiningWindowController : MonoBehaviour
{
    public const int MAX_TX_CHARACTER_SIZE = 100;

    [SerializeField]
    GlobalUIWindowController globalUIWindowController;

    [SerializeField]
    GameObject receiptPanel;
    [SerializeField]
    Animator receiptPanelAnimator;
    [SerializeField]
    GameObject characterCardPrefab;
    [SerializeField]
    GameObject waitingGridPanel;
    [SerializeField]
    GameObject workingGridPanel;
    [SerializeField]
    GameObject helpPanel;
    [SerializeField]
    Animator helpPanelAnimator;

    [SerializeField]
    Text[] amountTextList;
    [SerializeField]
    GameObject[] receiptRowPanelList;
    [SerializeField]
    Text receiptTitleText;
    [SerializeField]
    Text receiptSubText;
    [SerializeField]
    Button stakingButton;
    [SerializeField]
    Button unstakingButton;
    [SerializeField]
    Button receiptButton;

    [SerializeField]
    List<CharacterCardController> selectedWaitingCardControllerList = new List<CharacterCardController>();
    [SerializeField]
    List<CharacterCardController> selectedWorkingCardControllerList = new List<CharacterCardController>();

    private string dismissingTrigger = "dismissing";
    [SerializeField]
    int[] receiptIdList;
    bool unstakingReceipt = false;
    int txProcess = 0;
    int txMax = 0;

    private void OnEnable()
    {
        updateGrid();
        MiningManager.instance.startMiningAmountSyncronizer();
        ContractManager.instance.reqGetPassword();
    }

    private void OnDisable()
    {
        MiningManager.instance.stopMiningAmountSyncronizer();
    }

    public void showHelpPopup()
    {
        helpPanel.SetActive(true);
    }

    public void closeHelpPopup()
    {
        helpPanelAnimator.SetTrigger(dismissingTrigger);
    }

    public void dismissHelpPopup()
    {
        helpPanel.SetActive(false);
    }

    public void updateGrid()
    {
        selectedWaitingCardControllerList.Clear();
        selectedWorkingCardControllerList.Clear();

        List<CharacterData> list = CharacterManager.instance.getCharacterList();
        list.Sort(SortByTokenIdAscending);

        int waitingGridIdx = 0;
        int workingGridIdx = 0;

        for (int idx = 0; idx < list.Count; idx++)
        {
            CharacterData cd = list[idx];
            GameObject gridPanel;
            int gridIdx;
            int state;
            Func<CharacterCardController, bool> callback;
            if (cd.stakingData.purpose == StakingManager.PURPOSE_BREAK)
            {
                gridPanel = waitingGridPanel;
                gridIdx = waitingGridIdx++;
                state = CharacterCardController.STATE_WAITING_ROOM;
                callback = selectWatingCharacterCard;
            }
            else if (cd.stakingData.purpose == StakingManager.PURPOSE_MINING)
            {
                gridPanel = workingGridPanel;
                gridIdx = workingGridIdx++;
                state = CharacterCardController.STATE_WORKING_PLACE;
                callback = selectWorkingCharacterCard;
            }
            else
            {
                continue;
            }

            GameObject characterCard;
            if (gridIdx < gridPanel.transform.childCount)
            {
                characterCard = gridPanel.transform.GetChild(gridIdx).gameObject;
                characterCard.SetActive(true);
            }
            else
            {
                characterCard = Instantiate(characterCardPrefab, gridPanel.transform, true);
            }

            CharacterCardController cardController = characterCard.GetComponent<CharacterCardController>();
            cardController.setCharacterId(cd, state);
            cardController.setClickCallback(callback);
        }

        for (int idx = waitingGridIdx; idx < waitingGridPanel.transform.childCount; idx++)
        {
            waitingGridPanel.transform.GetChild(idx).gameObject.SetActive(false);
        }

        for (int idx = workingGridIdx; idx < workingGridPanel.transform.childCount; idx++)
        {
            workingGridPanel.transform.GetChild(idx).gameObject.SetActive(false);
        }

        stakingButton.interactable = waitingGridIdx > 0;
        receiptButton.interactable = workingGridIdx > 0;
        unstakingButton.interactable = workingGridIdx > 0;
    }

    public int SortByTokenIdAscending(CharacterData cd1, CharacterData cd2)
    {
        return cd1.tokenId - cd2.tokenId;
    }

    public bool selectWatingCharacterCard(CharacterCardController _cardController)
    {
        if (_cardController.isSelected())
        {
            selectedWaitingCardControllerList.Remove(_cardController);
            _cardController.setSelected(false);
        } else
        {
            selectedWaitingCardControllerList.Add(_cardController);
            _cardController.setSelected(true);
        }
        return true;
    }

    public bool selectWorkingCharacterCard(CharacterCardController _cardController)
    {
        if (_cardController.isSelected())
        {
            selectedWorkingCardControllerList.Remove(_cardController);
            _cardController.setSelected(false);
        }
        else
        {
            selectedWorkingCardControllerList.Add(_cardController);
            _cardController.setSelected(true);
        }
        return true;
    }

    public void onStakingButtonClicked()
    {
        if (selectedWaitingCardControllerList.Count == 0)
        {
            globalUIWindowController.showPopupByTextKey("ID_WARNING_SELECT_STAKING", null);
            return;
        }

        txMax = selectedWaitingCardControllerList.Count / MAX_TX_CHARACTER_SIZE + (selectedWaitingCardControllerList.Count % MAX_TX_CHARACTER_SIZE == 0 ? 0 : 1);
        txProcess = 0;

        string msg = string.Format(LanguageManager.instance.getText("ID_PROCESS_STAKING"), selectedWaitingCardControllerList.Count);
        if (txMax > 1)
        {
            msg += string.Format(LanguageManager.instance.getText("ID_MINING_TOO_MANY_CHARACTER"), txProcess + 1, txMax);
        }
        globalUIWindowController.showPopup(msg, reqAddMiningStaking);
    }

    public void reqAddMiningStaking()
    {
        int startIdx = txProcess * MAX_TX_CHARACTER_SIZE;
        int endIdx = Math.Min((txProcess + 1) * MAX_TX_CHARACTER_SIZE, selectedWaitingCardControllerList.Count);
        int[] idList = new int[endIdx - startIdx];
        for (int idx = startIdx; idx < endIdx; idx++)
        {
            idList[idx - startIdx] = selectedWaitingCardControllerList[idx].getCharacterData().tokenId;
        }

        ContractManager.instance.reqAddMiningStaking(idList);
    }

    public void resAddMiningStaking()
    {
        txProcess++;
        if (txProcess < txMax)
        {
            string msg = string.Format(LanguageManager.instance.getText("ID_PROCESS_STAKING"), selectedWaitingCardControllerList.Count);
            if (txMax > 1)
            {
                msg += string.Format(LanguageManager.instance.getText("ID_MINING_TOO_MANY_CHARACTER"), txProcess + 1, txMax);
            }
            globalUIWindowController.showPopup(msg, reqAddMiningStaking);
            return;
        }

        for (int idx = 0; idx < selectedWaitingCardControllerList.Count; idx++)
        {
            selectedWaitingCardControllerList[idx].getCharacterData().stakingData.purpose = StakingManager.PURPOSE_MINING;
            selectedWaitingCardControllerList[idx].getCharacterData().stakingData.startBlock = SystemInfoManager.instance.blockNumber;
        }
        
        updateGrid();
    }

    public void onUnstakingButtonClicked()
    {
        if (selectedWorkingCardControllerList.Count == 0)
        {
            globalUIWindowController.showPopupByTextKey("ID_WARNING_SELECT_UNSTAKING", null);
            return;
        }

        txMax = selectedWorkingCardControllerList.Count / MAX_TX_CHARACTER_SIZE + (selectedWorkingCardControllerList.Count % MAX_TX_CHARACTER_SIZE == 0 ? 0 : 1);
        txProcess = 0;


        string msg = string.Format(LanguageManager.instance.getText("ID_PROCESS_UNSTAKING"), selectedWorkingCardControllerList.Count);
        if (txMax > 1)
        {
            msg += string.Format(LanguageManager.instance.getText("ID_MINING_TOO_MANY_CHARACTER"), txProcess + 1, txMax);
        }
        globalUIWindowController.showPopup(msg, reqGetBackMiningStaking);
    }

    public void reqGetBackMiningStaking()
    {
        int startIdx = txProcess * MAX_TX_CHARACTER_SIZE;
        int endIdx = Math.Min((txProcess + 1) * MAX_TX_CHARACTER_SIZE, selectedWorkingCardControllerList.Count);
        int[] idList = new int[endIdx - startIdx];
        for (int idx = startIdx; idx < endIdx; idx++)
        {
            idList[idx - startIdx] = selectedWorkingCardControllerList[idx].getCharacterData().tokenId;
        }

        ContractManager.instance.reqGetBackMiningStaking(idList);
    }

    public void resGetBackMiningStaking()
    {
        // confirmReceiptPopup();
        txProcess++;
        if (txProcess < txMax)
        {
            string msg = string.Format(LanguageManager.instance.getText("ID_PROCESS_UNSTAKING"), selectedWorkingCardControllerList.Count);
            if (txMax > 1)
            {
                msg += string.Format(LanguageManager.instance.getText("ID_MINING_TOO_MANY_CHARACTER"), txProcess + 1, txMax);
            }
            globalUIWindowController.showPopup(msg, reqGetBackMiningStaking);
            return;
        }

        for (int idx = 0; idx < selectedWorkingCardControllerList.Count; idx++)
        {
            CharacterData data = selectedWorkingCardControllerList[idx].getCharacterData();
            data.stakingData.purpose = StakingManager.PURPOSE_BREAK;
            MiningManager.instance.getMiningData(data.tokenId).resetAmount();
        }

        updateGrid();
    }

    public void onAllReceivedButtonClicked()
    {
        MiningManager.instance.stopMiningAmountSyncronizer();
        showPopup(true);
    }

    public int[] getSelectedWorkingCharacterIdList()
    {
        int[] idList = new int[selectedWorkingCardControllerList.Count];
        for (int idx = 0; idx < selectedWorkingCardControllerList.Count; idx++)
        {
            idList[idx] = selectedWorkingCardControllerList[idx].getCharacterData().tokenId;
        }

        return idList;
    }

    public int[] getAllWorkingCharacterIdList()
    {
        int idx;
        for (idx = workingGridPanel.transform.childCount - 1; idx >= 0 ; idx--)
        {
            if (workingGridPanel.transform.GetChild(idx).gameObject.activeSelf)
            {
                break;
            }
        }
        int[] idList = new int[idx + 1];
        for (idx = 0; idx < idList.Length; idx++)
        {
            CharacterCardController cardController = workingGridPanel.transform.GetChild(idx).GetComponent<CharacterCardController>();
            idList[idx] = cardController.getCharacterData().tokenId;
        }

        return idList;
    }

    public void showPopup(bool _allWorkingCharacter)
    {
        receiptPanel.SetActive(true);

        receiptTitleText.text = LanguageManager.instance.getText("ID_MINING_RECEIPT");
        if (_allWorkingCharacter)
        {
            receiptSubText.text = string.Format(LanguageManager.instance.getText("ID_N_CHARACTER"), workingGridPanel.transform.childCount);
            receiptIdList = getAllWorkingCharacterIdList();
            unstakingReceipt = false;
        }
        else
        {
            receiptTitleText.text = LanguageManager.instance.getText("ID_MINING_RECEIPT") + " (" + LanguageManager.instance.getText("ID_UNSTAKING") + ")";
            receiptSubText.text = string.Format(LanguageManager.instance.getText("ID_N_CHARACTER"), selectedWorkingCardControllerList.Count);
            receiptIdList = getSelectedWorkingCharacterIdList();
            unstakingReceipt = true;
        }

        BigInteger[] amountList = new BigInteger[MiningManager.IDX_MAX];
        for(int idx = 0; idx < receiptIdList.Length; idx++)
        {
            MiningData md = MiningManager.instance.getMiningData(receiptIdList[idx]);

            for (int amountIdx = 0; amountIdx < amountList.Length; amountIdx++)
            {
                amountList[amountIdx] += md.amount[amountIdx];
            }
        }

        for (int idx = 0; idx < amountTextList.Length; idx++)
        {
            if (idx != MiningManager.IDX_BASIC && idx != MiningManager.IDX_FINAL)
            {
                receiptRowPanelList[idx].SetActive(amountList[idx] != 0);
            }

            string sign = amountList[idx] > 0 ? "+ " : "";
            amountTextList[idx].text = sign + Utils.convertPebToTorStr(amountList[idx]) + " " + Const.TOR_COIN;
        }
    }

    public void onReceiptButtonClicked()
    {
        if (unstakingReceipt)
        {
            ContractManager.instance.reqGetBackMiningStaking(receiptIdList);
        }
        else
        {
            BigInteger[] countryTax = new BigInteger[CharacterManager.COUNTRY_MAX];
            for (int i = 0; i < CharacterManager.COUNTRY_MAX; i++)
            {
                countryTax[i] = new BigInteger(0);
            }
            BigInteger finalAmount = new BigInteger(0);
            BigInteger commissionAmount = new BigInteger(0);
            int password = Utils.GetTransactionHash(UserManager.instance.getPassword());

            string[] countryTaxStr = new string[CharacterManager.COUNTRY_MAX];
            for (int i = 0; i < CharacterManager.COUNTRY_MAX; i++)
            {
                countryTaxStr[i] = countryTax[i].ToString();
            }

            for (int i = 0; i < receiptIdList.Length; i++)
            {
                MiningData md = MiningManager.instance.getMiningData(receiptIdList[i]);
                CharacterData cd = CharacterManager.instance.getCharacterData(receiptIdList[i]);
                countryTax[cd.country] += md.amount[MiningManager.IDX_TAX];
                finalAmount += md.amount[MiningManager.IDX_FINAL];
                commissionAmount += md.amount[MiningManager.IDX_COMMISSION];
            }
            commissionAmount = -commissionAmount;
            ContractManager.instance.reqReceiveMiningAmount(receiptIdList, countryTaxStr, finalAmount.ToString(), commissionAmount.ToString(), password);
        }
    }

    public void resReceiveMiningAmount()
    {
        ContractManager.instance.reqGetPassword();
        ContractManager.instance.reqCoinAmount();
        MiningManager.instance.resetAllMiningData();
        confirmReceiptPopup();
    }

    public void confirmReceiptPopup()
    {
        receiptPanelAnimator.SetTrigger(dismissingTrigger);
    }

    public void dismissReceiptPopup()
    {
        receiptIdList = null;
        receiptPanel.SetActive(false);
        MiningManager.instance.startMiningAmountSyncronizer();
    }

    public void selectAllWaitingCharacter()
    {
        selectCharacter(waitingGridPanel, true, selectWatingCharacterCard);
    }

    public void deselectAllWaitingCharacter()
    {
        selectCharacter(waitingGridPanel, false, selectWatingCharacterCard);
    }

    public void selectAllWorkingCharacter()
    {
        selectCharacter(workingGridPanel, true, selectWorkingCharacterCard);
    }

    public void deselectAllWorkingCharacter()
    {
        selectCharacter(workingGridPanel, false, selectWorkingCharacterCard);
    }

    public void selectCharacter(GameObject _gridPanel, bool _isSelect, Func<CharacterCardController, bool> _selectFunc)
    {
        int idx;
        for (idx = _gridPanel.transform.childCount - 1; idx >= 0; idx--)
        {
            if (_gridPanel.transform.GetChild(idx).gameObject.activeSelf)
            {
                break;
            }
        }

        int max = idx + 1;
        for (idx = 0; idx < max; idx++)
        {
            CharacterCardController cardController = _gridPanel.transform.GetChild(idx).GetComponent<CharacterCardController>();
            if (cardController.isSelected() != _isSelect)
            {
                _selectFunc(cardController);
            }
        }
    }
}