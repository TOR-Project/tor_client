using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Numerics;
using Coffee.UIExtensions;

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
    GameObject helpPanel;
    [SerializeField]
    Animator helpPanelAnimator;

    [SerializeField]
    GameObject waitingGridPanel;
    [SerializeField]
    GridLayoutGroup waitingGridLayoutGroup;
    [SerializeField]
    RectTransform waitingGridLayoutGroupRT;
    [SerializeField]
    Button waitingPrevButton;
    [SerializeField]
    Button waitingNextButton;
    [SerializeField]
    Text waitingPageText;

    [SerializeField]
    GameObject workingGridPanel;
    [SerializeField]
    GridLayoutGroup workingGridLayoutGroup;
    [SerializeField]
    RectTransform workingGridLayoutGroupRT;
    [SerializeField]
    Button workingPrevButton;
    [SerializeField]
    Button workingNextButton;
    [SerializeField]
    Text workingPageText;

    [SerializeField]
    Text[] amountTextList;
    [SerializeField]
    GameObject[] receiptRowPanelList;
    [SerializeField]
    Text[] amountTaxTextList;
    [SerializeField]
    GameObject[] receiptSubRowTaxPanelList;
    [SerializeField]
    Text[] amountCountryTextList;
    [SerializeField]
    GameObject[] receiptSubRowCountryPanelList;
    [SerializeField]
    Text receiptTitleText;
    [SerializeField]
    Text receiptSubText;
    [SerializeField]
    Button stakingButton;
    [SerializeField]
    Button unstakingButton;
    [SerializeField]
    GameObject receiptLoading;
    [SerializeField]
    Button receiptAllButton;

    [SerializeField]
    List<int> selectedWaitingCharacterIdList = new List<int>();
    [SerializeField]
    List<int> selectedWorkingCharacterIdList = new List<int>();

    List<CharacterData> waitingAllCharacterDataList = new List<CharacterData>();
    List<CharacterData> workingAllCharacterDataList = new List<CharacterData>();

    List<CharacterCardController> waitingCharacterCardControllerList = new List<CharacterCardController>();
    List<CharacterCardController> workingCharacterCardControllerList = new List<CharacterCardController>();

    int waitingPageNum = 0;
    int workingPageNum = 0;
    int waitingMaxPageNum = 1;
    int workingMaxPageNum = 1;

    float waitingContentsViewWidth = 0;
    float waitingContentsViewHeight = 0;

    private string dismissingTrigger = "dismissing";
    [SerializeField]
    int[] receiptIdList;
    bool unstakingReceipt = false;
    int txProcess = 0;
    int txMax = 0;

    private void OnEnable()
    {
        ContractManager.instance.reqGetPassword();
        StartCoroutine(playMiningSound());
        MiningManager.instance.startMiningAmountSyncronizer(0);
    }

    private void OnDisable()
    {
        SoundManager.instance.stopSeNow();
        MiningManager.instance.stopMiningAmountSyncronizer();
    }

    private void updateAllLayout()
    {
        init();
        fillGrid();
        updateCharacterData();
        updateArrowButton();
        updateWaitingGrid(0);
        updateWorkingGrid(0);
    }

    private void init()
    {
        waitingAllCharacterDataList.Clear();
        workingAllCharacterDataList.Clear();
        selectedWaitingCharacterIdList.Clear();
        selectedWorkingCharacterIdList.Clear();
    }

    private void updateCharacterData()
    {
        List<CharacterData> list = CharacterManager.instance.getMyCharacterList();
        list.Sort(SortByTokenIdAscending);


        for (int idx = 0; idx < list.Count; idx++)
        {
            CharacterData cd = list[idx];
            
            if (cd.stakingData.purpose == StakingManager.PURPOSE_BREAK)
            {
                waitingAllCharacterDataList.Add(cd);
            }
            else if (cd.stakingData.purpose == StakingManager.PURPOSE_MINING)
            {
                workingAllCharacterDataList.Add(cd);
            }
        }

        Debug.Log("updateCharacterData() " + list.Count + " wait " + waitingAllCharacterDataList.Count + " work " + workingAllCharacterDataList.Count);
        waitingPageNum = workingPageNum = 0;
        if (waitingCharacterCardControllerList.Count > 0)
        {
            waitingMaxPageNum = waitingAllCharacterDataList.Count / waitingCharacterCardControllerList.Count + (waitingAllCharacterDataList.Count % waitingCharacterCardControllerList.Count == 0 ? 0 : 1);
        } else
        {
            waitingMaxPageNum = 0;
        }
        if (workingCharacterCardControllerList.Count > 0)
        {
            workingMaxPageNum = workingAllCharacterDataList.Count / workingCharacterCardControllerList.Count + (workingAllCharacterDataList.Count % workingCharacterCardControllerList.Count == 0 ? 0 : 1);
        } else
        {
            workingMaxPageNum = 0;
        }
    }

    private void updateArrowButton()
    {
        waitingPrevButton.interactable = waitingPageNum > 0;
        waitingNextButton.interactable = waitingPageNum < waitingMaxPageNum - 1;
        workingPrevButton.interactable = workingPageNum > 0;
        workingNextButton.interactable = workingPageNum < workingMaxPageNum - 1;
    }

    private void fillGrid()
    {
        if (waitingCharacterCardControllerList.Count == 0)
        {
            waitingContentsViewWidth = waitingGridLayoutGroupRT.rect.width;
            float waitingGridSpaceX = waitingGridLayoutGroup.spacing.x;
            float waitingGridCellX = waitingGridLayoutGroup.cellSize.x;
            int waitingGridMaxCellCol = (int)((waitingContentsViewWidth + waitingGridSpaceX) / (waitingGridCellX + waitingGridSpaceX));
            if (waitingGridMaxCellCol <= 0)
            {
                waitingGridMaxCellCol = 1;
            }

            waitingContentsViewHeight = waitingGridLayoutGroupRT.rect.height;
            float waitingGridSpaceY = waitingGridLayoutGroup.spacing.y;
            float waitingGridCellY = waitingGridLayoutGroup.cellSize.y;
            int waitingGridMaxCellRow = (int)((waitingContentsViewHeight + waitingGridSpaceY) / (waitingGridCellY + waitingGridSpaceY));
            if (waitingGridMaxCellRow <= 0)
            {
                waitingGridMaxCellRow = 1;
            }

            int maxCellCount = waitingGridMaxCellCol * waitingGridMaxCellRow;
            for (int i = 0; i < maxCellCount; i++)
            {
                GameObject characterCard = Instantiate(characterCardPrefab, waitingGridPanel.transform, true);
                characterCard.transform.localScale = UnityEngine.Vector3.one;
                characterCard.SetActive(false);
                CharacterCardController cardController = characterCard.GetComponent<CharacterCardController>();
                cardController.setClickCallback(selectWatingCharacterCard);
                waitingCharacterCardControllerList.Add(cardController);
            }

            Debug.Log("fillGrid() wait " + waitingCharacterCardControllerList.Count);
        }

        if (workingCharacterCardControllerList.Count == 0)
        {
            float workingContentsViewWidth = workingGridLayoutGroupRT.rect.width;
            float workingGridSpaceX = workingGridLayoutGroup.spacing.x;
            float workingGridCellX = workingGridLayoutGroup.cellSize.x;
            int workingGridMaxCellCol = (int)((workingContentsViewWidth + workingGridSpaceX) / (workingGridCellX + workingGridSpaceX));
            if (workingGridMaxCellCol <= 0)
            {
                workingGridMaxCellCol = 1;
            }

            float workingContentsViewHeight = workingGridLayoutGroupRT.rect.height;
            float workingGridSpaceY = workingGridLayoutGroup.spacing.y;
            float workingGridCellY = workingGridLayoutGroup.cellSize.y;
            int workingGridMaxCellRow = (int)((workingContentsViewHeight + workingGridSpaceY) / (workingGridCellY + workingGridSpaceY));
            if (workingGridMaxCellRow <= 0)
            {
                workingGridMaxCellRow = 1;
            }

            int maxCellCount = workingGridMaxCellCol * workingGridMaxCellRow;
            for (int i = 0; i < maxCellCount; i++)
            {
                GameObject characterCard = Instantiate(characterCardPrefab, workingGridPanel.transform, true);
                characterCard.transform.localScale = UnityEngine.Vector3.one;
                characterCard.SetActive(false);
                CharacterCardController cardController = characterCard.GetComponent<CharacterCardController>();
                cardController.setClickCallback(selectWorkingCharacterCard);
                workingCharacterCardControllerList.Add(cardController);
            }

            Debug.Log("fillGrid() work " + workingCharacterCardControllerList.Count);
        }
    }

    private void Update()
    {
        float newContentsViewWidth = waitingGridLayoutGroupRT.rect.width;
        float newContentsViewHeight = waitingGridLayoutGroupRT.rect.height;
        if (newContentsViewWidth != waitingContentsViewWidth || newContentsViewHeight != waitingContentsViewHeight)
        {
            clearAllCharacterCard();
            updateAllLayout();
        }
    }

    private void clearAllCharacterCard()
    {
        for (int i = waitingGridLayoutGroupRT.childCount - 1; i >= 0; i--)
        {
            Destroy(waitingGridLayoutGroupRT.GetChild(i).gameObject);
            waitingCharacterCardControllerList.Clear();
        }

        for (int i = workingGridLayoutGroupRT.childCount - 1; i >= 0; i--)
        {
            Destroy(workingGridLayoutGroupRT.GetChild(i).gameObject);
            workingCharacterCardControllerList.Clear();
        }
    }

    private IEnumerator playMiningSound()
    {
        while(gameObject.activeSelf)
        {
            if (workingGridPanel.transform.childCount > 0 && workingGridPanel.transform.GetChild(0).gameObject.activeSelf)
            {
                SoundManager.instance.playSoundEffect("mining");
            }

            yield return new WaitForSeconds(10);
        }
    }

    public void showHelpPopup()
    {
        helpPanel.SetActive(true);
        setAllParticleEnabled(false);
    }

    public void closeHelpPopup()
    {
        helpPanelAnimator.SetTrigger(dismissingTrigger);
    }

    public void dismissHelpPopup()
    {
        helpPanel.SetActive(false);
        setAllParticleEnabled(true);
    }

    public void onWaitingPagePrevButtonClicked()
    {
        if (waitingPageNum <= 0)
        {
            return;
        }

        waitingPageNum--;
        updateWaitingGrid(waitingPageNum);

        updateArrowButton();
    }

    public void onWaitingPageNextButtonClicked()
    {
        if (waitingPageNum >= waitingMaxPageNum - 1)
        {
            return;
        }

        waitingPageNum++;
        updateWaitingGrid(waitingPageNum);

        updateArrowButton();
    }

    public void onWorkingPagePrevButtonClicked()
    {
        if (workingPageNum <= 0)
        {
            return;
        }

        workingPageNum--;
        updateWorkingGrid(workingPageNum);

        updateArrowButton();
    }

    public void onWorkingPageNextButtonClicked()
    {
        if (workingPageNum >= workingMaxPageNum - 1)
        {
            return;
        }

        workingPageNum++;
        updateWorkingGrid(workingPageNum);

        updateArrowButton();
    }

    public void updateWaitingGrid(int _page)
    {
        for (int i = 0; i < waitingCharacterCardControllerList.Count; i++)
        {
            CharacterCardController controller = waitingCharacterCardControllerList[i];

            int characterDataIdx = _page * waitingCharacterCardControllerList.Count + i;
            if (characterDataIdx < waitingAllCharacterDataList.Count)
            {
                CharacterData data = waitingAllCharacterDataList[characterDataIdx];
                controller.gameObject.SetActive(true);
                controller.setCharacterId(data, CharacterCardController.CharacterCardState.STATE_WAITING_ROOM);
                controller.setSelected(selectedWaitingCharacterIdList.Contains(data.tokenId));
            } else
            {
                controller.gameObject.SetActive(false);
            }
        }

        waitingPageText.text = (waitingMaxPageNum == 0 ? 0 : (_page + 1)) + "/" + waitingMaxPageNum;
    }

    public void updateWorkingGrid(int _page)
    {
        for (int i = 0; i < workingCharacterCardControllerList.Count; i++)
        {
            CharacterCardController controller = workingCharacterCardControllerList[i];

            int characterDataIdx = _page * workingCharacterCardControllerList.Count + i;
            if (characterDataIdx < workingAllCharacterDataList.Count)
            {
                CharacterData data = workingAllCharacterDataList[characterDataIdx];
                controller.gameObject.SetActive(true);
                controller.setCharacterId(data, CharacterCardController.CharacterCardState.STATE_WORKING_PLACE);
                controller.setSelected(selectedWorkingCharacterIdList.Contains(data.tokenId));
            } else
            {
                controller.gameObject.SetActive(false);
            }
        }

        workingPageText.text = (workingMaxPageNum == 0 ? 0 : (_page + 1)) + "/" + workingMaxPageNum;
    }

    public int SortByTokenIdAscending(CharacterData cd1, CharacterData cd2)
    {
        return cd1.tokenId - cd2.tokenId;
    }

    public bool selectWatingCharacterCard(CharacterCardController _cardController)
    {
        if (_cardController.isSelected())
        {
            selectedWaitingCharacterIdList.Remove(_cardController.getCharacterData().tokenId);
            _cardController.setSelected(false);
        } else
        {
            selectedWaitingCharacterIdList.Add(_cardController.getCharacterData().tokenId);
            _cardController.setSelected(true);
        }
        return true;
    }

    public bool selectWorkingCharacterCard(CharacterCardController _cardController)
    {
        if (_cardController.isSelected())
        {
            selectedWorkingCharacterIdList.Remove(_cardController.getCharacterData().tokenId);
            _cardController.setSelected(false);
        }
        else
        {
            selectedWorkingCharacterIdList.Add(_cardController.getCharacterData().tokenId);
            _cardController.setSelected(true);
        }
        return true;
    }

    public void onStakingButtonClicked()
    {
        if (selectedWaitingCharacterIdList.Count == 0)
        {
            globalUIWindowController.showPopupByTextKey("ID_WARNING_SELECT_STAKING", null);
            return;
        }

        txMax = selectedWaitingCharacterIdList.Count / MAX_TX_CHARACTER_SIZE + (selectedWaitingCharacterIdList.Count % MAX_TX_CHARACTER_SIZE == 0 ? 0 : 1);
        txProcess = 0;

        string msg = string.Format(LanguageManager.instance.getText("ID_PROCESS_STAKING"), selectedWaitingCharacterIdList.Count);
        if (txMax > 1)
        {
            msg += string.Format(LanguageManager.instance.getText("ID_MINING_TOO_MANY_CHARACTER"), txProcess + 1, txMax);
        }
        globalUIWindowController.showAlertPopup(msg, reqAddMiningStaking);
    }

    public void reqAddMiningStaking()
    {
        int startIdx = txProcess * MAX_TX_CHARACTER_SIZE;
        int endIdx = Math.Min((txProcess + 1) * MAX_TX_CHARACTER_SIZE, selectedWaitingCharacterIdList.Count);
        int[] idList = new int[endIdx - startIdx];
        for (int idx = startIdx; idx < endIdx; idx++)
        {
            idList[idx - startIdx] = selectedWaitingCharacterIdList[idx];
        }

        ContractManager.instance.reqAddMiningStaking(idList);
    }

    public void resAddMiningStaking()
    {
        txProcess++;
        if (txProcess < txMax)
        {
            string msg = string.Format(LanguageManager.instance.getText("ID_PROCESS_STAKING"), selectedWaitingCharacterIdList.Count);
            if (txMax > 1)
            {
                msg += string.Format(LanguageManager.instance.getText("ID_MINING_TOO_MANY_CHARACTER"), txProcess + 1, txMax);
            }
            globalUIWindowController.showAlertPopup(msg, reqAddMiningStaking);
            return;
        }

        for (int idx = 0; idx < selectedWaitingCharacterIdList.Count; idx++)
        {
            int tokenId = selectedWaitingCharacterIdList[idx];
            CharacterData data = CharacterManager.instance.getMyCharacterData(tokenId);
            data.stakingData.purpose = StakingManager.PURPOSE_MINING;
            data.stakingData.startBlock = SystemInfoManager.instance.blockNumber;
        }

        updateAllLayout();
    }

    public void onUnstakingButtonClicked()
    {
        if (selectedWorkingCharacterIdList.Count == 0)
        {
            globalUIWindowController.showPopupByTextKey("ID_WARNING_SELECT_UNSTAKING", null);
            return;
        }

        txMax = selectedWorkingCharacterIdList.Count / MAX_TX_CHARACTER_SIZE + (selectedWorkingCharacterIdList.Count % MAX_TX_CHARACTER_SIZE == 0 ? 0 : 1);
        txProcess = 0;


        string msg = string.Format(LanguageManager.instance.getText("ID_PROCESS_UNSTAKING"), selectedWorkingCharacterIdList.Count);
        if (txMax > 1)
        {
            msg += string.Format(LanguageManager.instance.getText("ID_MINING_TOO_MANY_CHARACTER"), txProcess + 1, txMax);
        }
        globalUIWindowController.showAlertPopup(msg, reqGetBackMiningStaking);
    }

    public void reqGetBackMiningStaking()
    {
        int startIdx = txProcess * MAX_TX_CHARACTER_SIZE;
        int endIdx = Math.Min((txProcess + 1) * MAX_TX_CHARACTER_SIZE, selectedWorkingCharacterIdList.Count);
        int[] idList = new int[endIdx - startIdx];
        for (int idx = startIdx; idx < endIdx; idx++)
        {
            idList[idx - startIdx] = selectedWorkingCharacterIdList[idx];
        }

        ContractManager.instance.reqGetBackMiningStaking(idList);
    }

    public void resGetBackMiningStaking()
    {
        // confirmReceiptPopup();
        txProcess++;
        if (txProcess < txMax)
        {
            string msg = string.Format(LanguageManager.instance.getText("ID_PROCESS_UNSTAKING"), selectedWorkingCharacterIdList.Count);
            if (txMax > 1)
            {
                msg += string.Format(LanguageManager.instance.getText("ID_MINING_TOO_MANY_CHARACTER"), txProcess + 1, txMax);
            }
            globalUIWindowController.showAlertPopup(msg, reqGetBackMiningStaking);
            return;
        }

        for (int idx = 0; idx < selectedWorkingCharacterIdList.Count; idx++)
        {
            CharacterData data = CharacterManager.instance.getMyCharacterData(selectedWorkingCharacterIdList[idx]);
            data.stakingData.purpose = StakingManager.PURPOSE_BREAK;
            MiningManager.instance.getMiningData(data.tokenId).resetAmount();
        }

        updateAllLayout();
    }

    public void onAllReceivedButtonClicked()
    {
        MiningManager.instance.stopMiningAmountSyncronizer();
        showPopup(true);
    }

    public int[] getAllWorkingCharacterIdList()
    {
        int[] idList = new int[workingAllCharacterDataList.Count];
        for (int idx = 0; idx < idList.Length; idx++)
        {
            idList[idx] = workingAllCharacterDataList[idx].tokenId;
        }

        return idList;
    }

    private void setAllParticleEnabled(bool _set)
    {
        foreach (CharacterCardController controller in waitingCharacterCardControllerList)
        {
            controller.setAllParticleEnabled(_set);
        }

        foreach (CharacterCardController controller in workingCharacterCardControllerList)
        {
            controller.setAllParticleEnabled(_set);
        }
    }

    public void showPopup(bool _allWorkingCharacter)
    {
        setAllParticleEnabled(false);
        receiptPanel.SetActive(true);

        receiptTitleText.text = LanguageManager.instance.getText("ID_MINING_RECEIPT");
        if (_allWorkingCharacter)
        {
            receiptSubText.text = string.Format(LanguageManager.instance.getText("ID_N_CHARACTER"), workingAllCharacterDataList.Count);
            receiptIdList = getAllWorkingCharacterIdList();
            unstakingReceipt = false;
        }
        else
        {
            receiptTitleText.text = LanguageManager.instance.getText("ID_MINING_RECEIPT") + " (" + LanguageManager.instance.getText("ID_UNSTAKING") + ")";
            receiptSubText.text = string.Format(LanguageManager.instance.getText("ID_N_CHARACTER"), selectedWorkingCharacterIdList.Count);
            receiptIdList = selectedWorkingCharacterIdList.ToArray();
            unstakingReceipt = true;
        }

        amountTextList[MiningManager.IDX_BASIC].text = LanguageManager.instance.getText("ID_CALCULATING");
        amountTextList[MiningManager.IDX_FINAL].text = LanguageManager.instance.getText("ID_CALCULATING");
        receiptLoading.SetActive(true);
        receiptAllButton.interactable = false;

        for (int idx = 0; idx < amountTextList.Length; idx++)
        {
            if (idx != MiningManager.IDX_BASIC && idx != MiningManager.IDX_FINAL)
            {
                receiptRowPanelList[idx].SetActive(false);
            }
        }

        for (int idx = 0; idx < CountryManager.COUNTRY_MAX; idx++)
        {
            receiptSubRowTaxPanelList[idx].SetActive(false);
            receiptSubRowCountryPanelList[idx].SetActive(false);
        }

        MiningManager.instance.requestMiningData(receiptIdList, resMiningData);
    }

    private void resMiningData()
    {
        receiptLoading.SetActive(false);
        receiptAllButton.interactable = true;

        BigInteger[] amountList = new BigInteger[MiningManager.IDX_MAX];
        BigInteger[] amountTaxList = new BigInteger[CountryManager.COUNTRY_MAX];
        BigInteger[] amountCountryList = new BigInteger[CountryManager.COUNTRY_MAX];
        for (int idx = 0; idx < receiptIdList.Length; idx++)
        {
            CharacterData characterData = CharacterManager.instance.getMyCharacterData(receiptIdList[idx]);
            MiningData md = MiningManager.instance.getMiningData(receiptIdList[idx]);

            for (int amountIdx = 0; amountIdx < amountList.Length; amountIdx++)
            {
                amountList[amountIdx] += md.amount[amountIdx];
            }

            amountTaxList[characterData.country] += md.amount[MiningManager.IDX_TAX];
            amountCountryList[characterData.country] += md.amount[MiningManager.IDX_COUNTRY];
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

        for (int idx = 0; idx < amountTaxList.Length; idx++)
        {
            receiptSubRowTaxPanelList[idx].SetActive(amountTaxList[idx] != 0);
            string sign = amountTaxList[idx] > 0 ? "+ " : "";
            amountTaxTextList[idx].text = sign + Utils.convertPebToTorStr(amountTaxList[idx]) + " " + Const.TOR_COIN;

            receiptSubRowCountryPanelList[idx].SetActive(amountCountryList[idx] != 0);
            sign = amountCountryList[idx] > 0 ? "+ " : "";
            amountCountryTextList[idx].text = sign + Utils.convertPebToTorStr(amountCountryList[idx]) + " " + Const.TOR_COIN;
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
            BigInteger[] countryTax = new BigInteger[CountryManager.COUNTRY_MAX];
            for (int i = 0; i < CountryManager.COUNTRY_MAX; i++)
            {
                countryTax[i] = new BigInteger(0);
            }
            BigInteger finalAmount = new BigInteger(0);
            BigInteger commissionAmount = new BigInteger(0);
            int password = Utils.GetTransactionHash(UserManager.instance.getPassword());

            for (int i = 0; i < receiptIdList.Length; i++)
            {
                MiningData md = MiningManager.instance.getMiningData(receiptIdList[i]);
                CharacterData cd = CharacterManager.instance.getMyCharacterData(receiptIdList[i]);
                countryTax[cd.country] += md.amount[MiningManager.IDX_TAX];
                finalAmount += md.amount[MiningManager.IDX_FINAL];
                commissionAmount += md.amount[MiningManager.IDX_COMMISSION];
            }

            string[] countryTaxStr = new string[CountryManager.COUNTRY_MAX];
            for (int i = 0; i < CountryManager.COUNTRY_MAX; i++)
            {
                countryTaxStr[i] = (-countryTax[i]).ToString();
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
        MiningManager.instance.resetAllMiningData();
        MiningManager.instance.startMiningAmountSyncronizer(3);
        setAllParticleEnabled(true);
    }

    public void selectAllWaitingCharacter()
    {
        selectedWaitingCharacterIdList.Clear();
        foreach (CharacterData data in waitingAllCharacterDataList) {
            selectedWaitingCharacterIdList.Add(data.tokenId);
        }

        foreach (CharacterCardController controller in waitingCharacterCardControllerList)
        {
            controller.setSelected(true);
        }
    }

    public void deselectAllWaitingCharacter()
    {
        selectedWaitingCharacterIdList.Clear();

        foreach (CharacterCardController controller in waitingCharacterCardControllerList)
        {
            controller.setSelected(false);
        }
    }

    public void selectAllWorkingCharacter()
    {
        selectedWorkingCharacterIdList.Clear();
        foreach (CharacterData data in workingAllCharacterDataList)
        {
            selectedWorkingCharacterIdList.Add(data.tokenId);
        }

        foreach (CharacterCardController controller in workingCharacterCardControllerList)
        {
            controller.setSelected(true);
        }
    }

    public void deselectAllWorkingCharacter()
    {
        selectedWorkingCharacterIdList.Clear();

        updateWorkingGrid(workingPageNum);

        foreach (CharacterCardController controller in workingCharacterCardControllerList)
        {
            controller.setSelected(false);
        }
    }
}