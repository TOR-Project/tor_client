using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Numerics;
using Coffee.UIExtensions;

public class CastlePanelController : MonoBehaviour
{
    public const int IDX_BASIC_INFO = 0;
    public const int IDX_MONARCH = 1;
    public const int IDX_FR = 2;
    public const int IDX_FINANCE = 3;
    public const int IDX_LOG = 4;

    public const int LOG_REQUEST_COUNT = 30;

    private int countryId = 0;

    [SerializeField]
    Text castleTitleText;

    [SerializeField]
    Button[] buttonList;
    [SerializeField]
    GameObject[] panelList;
    [SerializeField]
    GameObject loading;

    [SerializeField]
    InputPopupController inputPopupController;

    [Header("basic info")]
    [SerializeField]
    Text populationText;
    [SerializeField]
    Text miningTaxText;
    [SerializeField]
    SlidingFloatingNumberController miningTaxNumberController;
    [SerializeField]
    GameObject miningTaxSettingRow;
    [SerializeField]
    Text treasuryText;
    [SerializeField]
    SlidingBigNumberController treasuryNumberController;
    [SerializeField]
    Text vaultText;
    [SerializeField]
    SlidingBigNumberController vaultNumberController;
    [SerializeField]
    GameObject vaultRow;
    [SerializeField]
    GameObject vaultSettingRow;
    [SerializeField]
    GameObject propertyCategoryRow;
    [SerializeField]
    RectTransform propertyContainer;
    [SerializeField]
    GameObject propertyAttributeRowPrefab;

    [Header("monarch")]
    [SerializeField]
    Text monarchNumText;
    [SerializeField]
    GameObject vacancyGO;
    [SerializeField]
    GameObject[] characterInfoGO;
    [SerializeField]
    Text nicknameText;
    [SerializeField]
    CharacterImageController characterImageController;
    [SerializeField]
    Text nameText;

    [Header("log")]
    [SerializeField]
    RectTransform logContainer;
    [SerializeField]
    GameObject logRowPrefab;
    [SerializeField]
    Scrollbar logScrollbar;
    bool refreshLog = false;

    private void Awake()
    {
        miningTaxNumberController.setFormat("{0:0.0000} %");
        treasuryNumberController.setFormat("{0} " + Const.TOR_COIN);
        vaultNumberController.setFormat("{0} " + Const.TOR_COIN);
    }

    public void setCastleId(int _cid)
    {
        countryId = _cid;

        castleTitleText.text = string.Format(LanguageManager.instance.getText("ID_COUNTRY_CASTLE"), CountryManager.instance.getCountryName(_cid));

        onButtonClicked(IDX_BASIC_INFO);
    }

    private void enableAllButtons()
    {
        foreach (Button button in buttonList)
        {
            button.interactable = true;
        }
    }

    private void hideAllPanel()
    {
        foreach (GameObject panel in panelList)
        {
            panel.gameObject.SetActive(false);
        }
    }

    public void onButtonClicked(int _idx)
    {
        enableAllButtons();
        hideAllPanel();

        buttonList[_idx].interactable = false;
        panelList[_idx].SetActive(true);
        loading.SetActive(true);

        switch (_idx)
        {
            case IDX_BASIC_INFO:
            default:
                updateBasicInfoPanel();
                break;
            case IDX_MONARCH:
                updateMonarchPanel();
                break;
            case IDX_FR:
                updateFRPanel();
                break;
            case IDX_FINANCE:
                updateFinancePanel();
                break;
            case IDX_LOG:
                updateLogPanel();
                break;
        }
    }

    private void updateBasicInfoPanel()
    {
        clearAllBasicInfo();
        CountryManager.instance.requestCountryData(countryId, updateBasicInfo);
    }
    private void updateMonarchPanel()
    {
        clearAllMonarchInfo();
        CountryManager.instance.requestCountryData(countryId, updateMonarchInfo);
    }

    private void updateFRPanel()
    {
        // TBD
        loading.SetActive(false);
    }

    private void updateFinancePanel()
    {
        // TBD
        loading.SetActive(false);
    }

    private void updateLogPanel()
    {
        clearAllLogInfo();
        CountryManager.instance.requestCountryData(countryId, updateLogInfo);
    }


    private void clearAllBasicInfo()
    {
        populationText.text = "0";
        miningTaxText.text = "0 %";
        miningTaxNumberController.reset();
        miningTaxSettingRow.SetActive(false);
        vaultRow.SetActive(false);
        vaultSettingRow.SetActive(false);
        propertyCategoryRow.SetActive(false);
        for (int i = 0; i < propertyContainer.childCount; i++)
        {
            propertyContainer.GetChild(i).gameObject.SetActive(false);
        }
        treasuryText.text = "0 " + Const.TOR_COIN;
        vaultText.text = "0 " + Const.TOR_COIN;
        treasuryNumberController.reset();
        vaultNumberController.reset();
    }

    private void clearAllMonarchInfo()
    {
        monarchNumText.text = "";
        vacancyGO.SetActive(false);
        foreach (GameObject go in characterInfoGO)
        {
            go.SetActive(false);
        }
        nicknameText.text = "";
        nameText.text = "";
    }

    private void clearAllLogInfo()
    {
        for (int i = 0; i < logContainer.childCount; i++)
        {
            logContainer.GetChild(i).gameObject.SetActive(false);
        }
    }

    private bool updateBasicInfo(CountryData _data)
    {
        if (_data.id != countryId)
        {
            return false;
        }

        if (_data.population == 0)
        {
            populationText.text = LanguageManager.instance.getText("ID_POPULATION_COUNTING_NOW");
        }

        miningTaxNumberController.setNumber(_data.castleData.lastMiningTaxData.tax);

        if (_data.castleData.hasMonarch && CharacterManager.instance.hasCharacter(_data.castleData.monarchId))
        {
            miningTaxSettingRow.SetActive(true);
            vaultRow.SetActive(true);
            vaultSettingRow.SetActive(true);
        }

        treasuryNumberController.setNumber(_data.castleData.treasury);
        vaultNumberController.setNumber(_data.castleData.personalSafe);

        if (_data.propertyList.Count > 0)
        {
            propertyCategoryRow.SetActive(true);

            for (int i = 0; i < _data.propertyList.Count; i++)
            {
                PropertyData propertyData = _data.propertyList[i];

                PropertyRowController rowController;
                if (propertyContainer.childCount > i)
                {
                    GameObject childObject = propertyContainer.GetChild(i).gameObject;
                    rowController = childObject.GetComponent<PropertyRowController>();
                }
                else
                {
                    GameObject propertyRow = Instantiate(propertyAttributeRowPrefab, propertyContainer, true);
                    propertyRow.transform.localScale = UnityEngine.Vector3.one;
                    rowController = propertyRow.GetComponent<PropertyRowController>();
                }

                rowController.updatePropertyData(propertyData);
                rowController.gameObject.SetActive(propertyData.startBlock <= SystemInfoManager.instance.blockNumber);
            }
        }

        loading.SetActive(false);
        return true;
    }

    private bool updateMonarchInfo(CountryData _data)
    {
        if (_data.id != countryId)
        {
            return false;
        }

        monarchNumText.text = string.Format(LanguageManager.instance.getText("ID_MONARCH_NUM"), _data.castleData.formerMonarchList.Count);
        monarchNumText.gameObject.SetActive(_data.castleData.formerMonarchList.Count > 0);
        if (_data.castleData.hasMonarch)
        {
            nicknameText.text = string.IsNullOrEmpty(_data.castleData.monarchOwnerNickname) ? LanguageManager.instance.getText("ID_UNKNOWN") : _data.castleData.monarchOwnerNickname;
            CharacterManager.instance.getCharacterDataAsync(_data.castleData.monarchId, updateMonarchCharacterData);
        }
        else
        {
            vacancyGO.SetActive(true);
            loading.SetActive(false);
        }

        return true;
    }

    public bool updateMonarchCharacterData(CharacterData _data)
    {
        characterImageController.updateCharacterImage(_data);
        nameText.text = _data.name;

        foreach (GameObject go in characterInfoGO)
        {
            go.SetActive(true);
        }
        loading.SetActive(false);
        return true;
    }

    private bool updateLogInfo(CountryData _data)
    {
        if (_data.id != countryId)
        {
            return false;
        }

        for (int i = 0; i < _data.logList.Count; i++)
        {
            LogData logData = _data.logList[i];

            LogRowController rowController;
            if (logContainer.childCount > i)
            {
                GameObject childObject = logContainer.GetChild(i).gameObject;
                childObject.SetActive(true);
                rowController = childObject.GetComponent<LogRowController>();
            }
            else
            {
                GameObject logRow = Instantiate(logRowPrefab, logContainer, true);
                logRow.transform.localScale = UnityEngine.Vector3.one;
                rowController = logRow.GetComponent<LogRowController>();
            }

            rowController.updateLogData(logData, _data);
        }

        loading.SetActive(false);
        return true;
    }

    public void onDonateButtonClicked()
    {
        inputPopupController.show(LanguageManager.instance.getText("ID_DONATION_POPUP_CONTENTS"), "0.0000", TMPro.TMP_InputField.ContentType.DecimalNumber, onDonateConfirmed);
    }

    public void onMiningTaxSettingButtonClicked()
    {
        inputPopupController.show(string.Format(LanguageManager.instance.getText("ID_MINING_TAX_POPUP_CONTENTS"), Const.MINING_TAX_SETTLING_DELAY) , string.Format("{0:0.0000}", CountryManager.instance.getCountryData(countryId).castleData.lastMiningTaxData.tax), TMPro.TMP_InputField.ContentType.DecimalNumber, onMiningTaxSettingConfirmed);
        if (SystemInfoManager.instance.blockNumber < CountryManager.instance.getCountryData(countryId).castleData.nextTaxSettableBlock)
        {
            inputPopupController.setFeedbackText(string.Format(LanguageManager.instance.getText("ID_MINING_TAX_POPUP_ERROR_TERM"), CountryManager.instance.getCountryData(countryId).castleData.nextTaxSettableBlock));
        }
    }

    public void onDepositButtonClicked()
    {
        inputPopupController.show(LanguageManager.instance.getText("ID_MONARCH_SAFE_DEPOSIT_POPUP_CONTENTS"), "0.0000", TMPro.TMP_InputField.ContentType.DecimalNumber, onDepositConfirmed);
    }

    public void onWithdrawButtonClicked()
    {
        inputPopupController.show(LanguageManager.instance.getText("ID_MONARCH_SAFE_WITHDROW_POPUP_CONTENTS"), Utils.convertPebToTorStr(CountryManager.instance.getCountryData(countryId).castleData.personalSafe), TMPro.TMP_InputField.ContentType.DecimalNumber, onWithdrawConfirmed);
    }

    private bool onDonateConfirmed(string _input)
    {
        float.TryParse(_input, out float outValue);
        BigInteger value = Utils.convertToPeb(outValue);
        if (value <= 0 || value > UserManager.instance.getCoinAmount())
        {
            inputPopupController.setFeedbackText(LanguageManager.instance.getText("ID_POPUP_ERROR_INVALID"));
            return false;
        }

        if (!CharacterManager.instance.isCitizenOfCountry(countryId))
        {
            inputPopupController.setFeedbackText(LanguageManager.instance.getText("ID_DONATION_POPUP_ERROR_NOT_CITIZEN"));
            return false;
        }

        ContractManager.instance.reqDonate(countryId, value);
        return true;
    }

    public void onDonateSuccessed(int _cid, BigInteger _value)
    {
        if (_cid != countryId)
        {
            return;
        }

        CountryManager.instance.getCountryData(_cid).castleData.treasury += _value;
        treasuryNumberController.setNumber(CountryManager.instance.getCountryData(_cid).castleData.treasury);
        UserManager.instance.setCoinAmount(UserManager.instance.getCoinAmount() - _value);
        inputPopupController.dismiss();

        LogData logData = new LogData();
        logData.logType = CountryManager.LOG_TYPE_DONATION;
        logData.whoNickName = UserManager.instance.getNickname();
        logData.dataInt = _value;
        CountryManager.instance.addLog(_cid, logData);
    }

    private bool onMiningTaxSettingConfirmed(string _input)
    {
        float.TryParse(_input, out float tax);
        if (tax < 0 || tax > 30 || float.IsNaN(tax))
        {
            inputPopupController.setFeedbackText(LanguageManager.instance.getText("ID_POPUP_ERROR_INVALID"));
            return false;
        }

        if (SystemInfoManager.instance.blockNumber < CountryManager.instance.getCountryData(countryId).castleData.nextTaxSettableBlock)
        {
            inputPopupController.setFeedbackText(string.Format(LanguageManager.instance.getText("ID_MINING_TAX_POPUP_ERROR_TERM"), CountryManager.instance.getCountryData(countryId).castleData.nextTaxSettableBlock));
            return false;
        }

        ContractManager.instance.reqSetMiningTax(countryId, (int)(tax * 10000));
        return true;
    }

    public void onMiningTaxSettingSuccessed(int _cid, int _tax)
    {
        if (_cid != countryId)
        {
            return;
        }

        float tax = _tax / 10000f;
        CountryManager.instance.getCountryData(_cid).castleData.lastMiningTaxData.tax = tax;
        CountryManager.instance.getCountryData(_cid).castleData.nextTaxSettableBlock = SystemInfoManager.instance.blockNumber + Const.MINING_TAX_SETTLING_DELAY;
        miningTaxNumberController.setNumber(tax);
        inputPopupController.dismiss();

        LogData logData = new LogData();
        logData.logType = CountryManager.LOG_TYPE_ADJUST_MINING_TAX;
        logData.dataInt = new BigInteger(_tax);
        CountryManager.instance.addLog(_cid, logData);
    }

    private bool onDepositConfirmed(string _input)
    {
        BigInteger value = Utils.convertToPeb(float.Parse(_input));
        if (value <= 0 || value > UserManager.instance.getCoinAmount())
        {
            inputPopupController.setFeedbackText(LanguageManager.instance.getText("ID_POPUP_ERROR_INVALID"));
            return false;
        }

        ContractManager.instance.reqDepositMonarchSafe(countryId, value);
        return true;
    }

    public void onDepositSuccssed(int _cid, BigInteger _value)
    {
        if (_cid != countryId)
        {
            return;
        }

        CountryManager.instance.getCountryData(_cid).castleData.personalSafe += _value;
        vaultNumberController.setNumber(CountryManager.instance.getCountryData(_cid).castleData.personalSafe);
        UserManager.instance.setCoinAmount(UserManager.instance.getCoinAmount() - _value);
        inputPopupController.dismiss();
    }

    private bool onWithdrawConfirmed(string _input)
    {
        BigInteger value = Utils.convertToPeb(float.Parse(_input));
        if (value <= 0 || value > CountryManager.instance.getCountryData(countryId).castleData.personalSafe)
        {
            inputPopupController.setFeedbackText(LanguageManager.instance.getText("ID_POPUP_ERROR_INVALID"));
            return false;
        }

        ContractManager.instance.reqWithdrawMonarchSafe(countryId, value);
        return true;
    }

    public void onWithdrawSuccssed(int _cid, BigInteger _value)
    {
        if (_cid != countryId)
        {
            return;
        }

        CountryManager.instance.getCountryData(_cid).castleData.personalSafe -= _value;
        vaultNumberController.setNumber(CountryManager.instance.getCountryData(_cid).castleData.personalSafe);
        UserManager.instance.setCoinAmount(UserManager.instance.getCoinAmount() + _value);
        inputPopupController.dismiss();
    }

    public void onLogScrollValueChanged()
    {
        if (logScrollbar.value <= 0)
        {
            List<LogData> logList = CountryManager.instance.getCountryData(countryId).logList;
            if (!refreshLog && logList.Count > 0 && logList[logList.Count - 1].id > 0)
            {
                refreshLog = true;

                Debug.Log("0 = " + logList[0].id + ", last = " + logList[logList.Count - 1].id);
                getMoreLogList(countryId, logList[logList.Count - 1].id - 1, LOG_REQUEST_COUNT);
            }

        }
        else
        {
            refreshLog = false;
        }
    }

    private void getMoreLogList(int _cid, int _fromId, int _count)
    {
        loading.SetActive(true);
        CountryManager.instance.requestMoreLogData(_cid, _fromId, _count, updateLogInfo);
    }
}