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
    Text nonarchNumText;
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
        foreach(GameObject panel in panelList)
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

        switch(_idx)
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

    private void updateLogPanel()
    {
    }

    private void updateResearchPanel()
    {
    }

    private void updateMonarchPanel()
    {
    }

    private void updateBasicInfoPanel()
    {
    }
}