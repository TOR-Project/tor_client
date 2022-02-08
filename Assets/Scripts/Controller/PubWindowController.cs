using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class PubWindowController : MonoBehaviour
{
    [SerializeField]
    Button[] filterRegionButton;

    [SerializeField]
    GameObject infoPanel;
    [SerializeField]
    CharacterImageController characterImageController;

    [SerializeField]
    Text nameText;
    [SerializeField]
    Text regionText;
    [SerializeField]
    Text raceText;
    [SerializeField]
    Text classText;
    [SerializeField]
    Text levelValueText;
    [SerializeField]
    Text attValueText;
    [SerializeField]
    Text defValueText;
    [SerializeField]
    Text attBoostValueText;
    [SerializeField]
    Text defBoostValueText;
    [SerializeField]
    Text[] equipValueText;

    [SerializeField]
    GameObject boostsCategoryRow;
    [SerializeField]
    GameObject attBoostsAttrRow;
    [SerializeField]
    GameObject defBoostsAttrRow;

    [SerializeField]
    GameObject[] equipStatusRowContainer;

    [SerializeField]
    GameObject equipStatusPrefab;

    [SerializeField]
    CharacterGridController characterCardGridController;

    private void OnEnable()
    {
        characterCardGridController.setCharacterSelectCallback(updateInfoPanel);
    }

    public void onFilterButtonClicked(int _regionFilter)
    {
        for(int i = 0; i < filterRegionButton.Length; i++)
        {
            filterRegionButton[i].interactable = (i != _regionFilter);
        }

        int filter = _regionFilter;
        if (filter == 5)
        {
            characterCardGridController.updateCharacterData(null);
        }
        else
        {
            characterCardGridController.updateCharacterData(data => data.country == filter);
        }
    }

    private bool updateInfoPanel(CharacterData _data)
    {
        if (_data == null)
        {
            infoPanel.SetActive(false);
            return false;
        }
        infoPanel.SetActive(true);

        characterImageController.updateCharacterImage(_data);

        nameText.text = _data.name;
        regionText.text = CountryManager.instance.getCountryName(_data.country);
        raceText.text = CharacterManager.instance.getRaceText(_data.race);
        classText.text = CharacterManager.instance.getJobText(_data.job);
        levelValueText.text = _data.level.ToString();
        attValueText.text = _data.statusData.att.ToString();
        defValueText.text = _data.statusData.def.ToString();

        EquipItemData[] equipItemDataList = new EquipItemData[]
        {
            ItemManager.instance.getEquipItem(_data.equipData.head),
            ItemManager.instance.getEquipItem(_data.equipData.weapon),
            ItemManager.instance.getEquipItem(_data.equipData.accessory),
            ItemManager.instance.getEquipItem(_data.equipData.armor),
            ItemManager.instance.getEquipItem(_data.equipData.pants),
            ItemManager.instance.getEquipItem(_data.equipData.shoes),
        };

        int attBoost = 0;
        int defBoost = 0;
        for (int i = 0; i < equipItemDataList.Length; i++)
        {
            EquipItemData ed = equipItemDataList[i];

            if (ed == null)
            {
                equipValueText[i].text = LanguageManager.instance.getText("ID_NONE");
                equipStatusRowContainer[i].SetActive(false);
                continue;
            }
            equipValueText[i].text = LanguageManager.instance.getText(ed.nameKey);
            equipStatusRowContainer[i].SetActive(true);
            for (int c = equipStatusRowContainer[i].transform.childCount - 1; c >= 0; c--)
            {
                Destroy(equipStatusRowContainer[i].transform.GetChild(c).gameObject);
            }
            GameObject equipStatusRow = Instantiate(equipStatusPrefab, equipStatusRowContainer[i].transform);
            Text equipStatusValue = equipStatusRow.transform.GetChild(0).GetComponent<Text>();
            equipStatusValue.text = (ed.att != 0 ? "Attack +" + ed.att.ToString() : "Defense +" + ed.def.ToString());
            if (ed.attBoost != 0 || ed.defBoost != 0)
            {
                GameObject equipBoostStatusRow = Instantiate(equipStatusPrefab, equipStatusRowContainer[i].transform);
                Text equipBoostStatusValue = equipBoostStatusRow.transform.GetChild(0).GetComponent<Text>();
                equipBoostStatusValue.text = (ed.attBoost != 0 ? "Attack Boost +" + ed.attBoost.ToString() + "%" : "Defense Boost +" + ed.defBoost.ToString() + "%");
            }

            attBoost += ed.attBoost;
            defBoost += ed.defBoost;
        }

        if (attBoost == 0 && defBoost == 0)
        {
            boostsCategoryRow.SetActive(false);
            attBoostsAttrRow.SetActive(false);
            defBoostsAttrRow.SetActive(false);
        } else
        {
            boostsCategoryRow.SetActive(true);
            attBoostsAttrRow.SetActive(attBoost != 0);
            defBoostsAttrRow.SetActive(defBoost != 0);

            attBoostValueText.text = attBoost.ToString() + "%";
            defBoostValueText.text = defBoost.ToString() + "%";
        }

        return true;
    }
}