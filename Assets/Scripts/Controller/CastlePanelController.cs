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
    public const int IDX_RESEARCH = 2;
    public const int IDX_LOG = 3;

    public const int LOG_REQUEST_COUNT = 30;

    private int castleId = 0;

    [SerializeField]
    Text castleTitleText;

    [SerializeField]
    Button[] buttonList;
    [SerializeField]
    GameObject[] panelList;
    [SerializeField]
    GameObject loading;

    [Header("basic info")]
    [SerializeField]
    Text populationText;
    [SerializeField]
    Text miningTaxText;
    [SerializeField]
    GameObject miningTaxSettingRow;
    [SerializeField]
    Text treasuryText;
    [SerializeField]
    Text vaultText;
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

    public void setCastleId(int _cid)
    {
        castleId = _cid;

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
            case IDX_RESEARCH:
                updateResearchPanel();
                break;
            case IDX_LOG:
                updateLogPanel();
                break;
        }
    }


    private void updateBasicInfoPanel()
    {
        clearAllBasicInfo();
        CountryManager.instance.requestCountryData(castleId, updateBasicInfo);
    }
    private void updateMonarchPanel()
    {
        clearAllMonarchInfo();
        CountryManager.instance.requestCountryData(castleId, updateMonarchInfo);
    }

    private void updateResearchPanel()
    {
        // TBD
        loading.SetActive(false);
    }

    private void updateLogPanel()
    {
        clearAllLogInfo();
        CountryManager.instance.requestCountryData(castleId, updateLogInfo);
    }


    private void clearAllBasicInfo()
    {
        populationText.text = "";
        miningTaxText.text = "";
        miningTaxSettingRow.SetActive(false);
        vaultRow.SetActive(false);
        vaultSettingRow.SetActive(false);
        propertyCategoryRow.SetActive(false);
        for (int i = 0; i < propertyContainer.childCount; i++)
        {
            propertyContainer.GetChild(i).gameObject.SetActive(false);
        }
        treasuryText.text = "";
        vaultText.text = "";

    }

    private void clearAllMonarchInfo()
    {
        monarchNumText.text = "";
        vacancyGO.SetActive(false);
        foreach(GameObject go in characterInfoGO)
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
        if (_data.population == 0)
        {
            populationText.text = LanguageManager.instance.getText("ID_POPULATION_COUNTING_NOW");
        }

        miningTaxText.text = string.Format("{0:0.0000}%", _data.castleData.lastMiningTaxData.tax);

        if (_data.castleData.hasMonarch && CharacterManager.instance.hasCharacter(_data.castleData.monarchId))
        {
            miningTaxSettingRow.SetActive(true);
            vaultRow.SetActive(true);
            vaultSettingRow.SetActive(true);
        }

        treasuryText.text = Utils.convertPebToTorStr(_data.castleData.treasury) + " " + Const.TOR_COIN;
        vaultText.text = Utils.convertPebToTorStr(_data.castleData.personalSafe) + " " + Const.TOR_COIN;

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
        monarchNumText.text = string.Format(LanguageManager.instance.getText("ID_MONARCH_NUM"), _data.castleData.formerMonarchList.Count + 1);
        if (_data.castleData.hasMonarch)
        {
            nicknameText.text = string.IsNullOrEmpty(_data.castleData.monarchOwnerNickname) ? LanguageManager.instance.getText("ID_UNKNOWN") : _data.castleData.monarchOwnerNickname;
            CharacterManager.instance.getCharacterDataAsync(_data.castleData.monarchId, updateMonarchCharacterData);
        } else
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
}