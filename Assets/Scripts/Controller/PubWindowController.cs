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
    GameObject characterCardPrefab;
    [SerializeField]
    GameObject gridPanel;

    [SerializeField]
    GameObject infoPanel;
    [SerializeField]
    Image characterImage;
    [SerializeField]
    EquipItemCardController[] equipItemCardControllerList;

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

    CharacterCardController lastSelectedCharacterCard;

    private void OnEnable()
    {
        onFilterButtonClicked(5);
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
            filter = CharacterManager.COUNTRY_ALL;
        }
        updateGrid(filter);
    }

    private void updateGrid(int _regionFilter) {
        List<CharacterData> list = getFilteredCharacterList(_regionFilter);
        list.Sort(SortByTokenIdAscending);

        for (int idx = 0; idx < list.Count; idx++)
        {
            GameObject characterCard;
            if (idx < gridPanel.transform.childCount)
            {
                characterCard = gridPanel.transform.GetChild(idx).gameObject;
                characterCard.SetActive(true);
            }
            else
            {
                characterCard = Instantiate(characterCardPrefab, gridPanel.transform);
            }

            CharacterCardController cardController = characterCard.GetComponent<CharacterCardController>();
            cardController.setCharacterId(list[idx], CharacterCardController.STATE_PUB);
            cardController.setClickCallback(selectCharacterCard);
        }

        for (int idx = list.Count; idx < gridPanel.transform.childCount; idx++)
        {
            gridPanel.transform.GetChild(idx).gameObject.SetActive(false);
        }

        if (list.Count > 0)
        {
            selectCharacterCard(gridPanel.transform.GetChild(0).GetComponent<CharacterCardController>(), true);
        } else
        {
            selectCharacterCard(null);
        }
    }

    private List<CharacterData> getFilteredCharacterList(int _regionFilter)
    {
        List<CharacterData> allList = CharacterManager.instance.getCharacterList();
        if (_regionFilter == CharacterManager.COUNTRY_ALL)
        {
            return allList;
        }

        List<CharacterData> filteredList = new List<CharacterData>();
        foreach (CharacterData data in allList)
        {
            if (data.country == _regionFilter)
            {
                filteredList.Add(data);
            }
        }
        return filteredList;
    }

    public int SortByTokenIdAscending(CharacterData cd1, CharacterData cd2)
    {
        return cd1.tokenId - cd2.tokenId;
    }

    public bool selectCharacterCard(CharacterCardController _cardController)
    {
        return selectCharacterCard(_cardController, false);
    }

    public bool selectCharacterCard(CharacterCardController _cardController, bool _forced)
    {
        if (!_forced && lastSelectedCharacterCard == _cardController)
        {
            return false;
        }

        if (lastSelectedCharacterCard != null)
        {
            lastSelectedCharacterCard.setSelected(false);
        }

        if (_cardController != null)
        {
            _cardController.setSelected(true);
        }

        lastSelectedCharacterCard = _cardController;

        updateInfoPanel(_cardController);
        return true;
    }

    private void updateInfoPanel(CharacterCardController _cardController)
    {
        if (_cardController == null)
        {
            infoPanel.SetActive(false);
            return;
        }
        infoPanel.SetActive(true);

        CharacterData data = _cardController.getCharacterData();

        nameText.text = data.name;
        regionText.text = CharacterManager.instance.getCountryText(data.country);
        raceText.text = CharacterManager.instance.getRaceText(data.race);
        classText.text = CharacterManager.instance.getJobText(data.job);
        levelValueText.text = data.level.ToString();
        attValueText.text = data.statusData.att.ToString();
        defValueText.text = data.statusData.def.ToString();

        EquipItemData[] equipItemDataList = new EquipItemData[]
        {
            ItemManager.instance.getEquipItem(EquipItemCategory.HELMET, data),
            ItemManager.instance.getEquipItem(EquipItemCategory.WEAPON, data),
            ItemManager.instance.getEquipItem(EquipItemCategory.ACCESSORY, data),
            ItemManager.instance.getEquipItem(EquipItemCategory.ARMOR, data),
            ItemManager.instance.getEquipItem(EquipItemCategory.PANTS, data),
            ItemManager.instance.getEquipItem(EquipItemCategory.SHOES, data),
        };

        int attBoost = 0;
        int defBoost = 0;
        for (int i = 0; i < equipItemDataList.Length; i++)
        {
            EquipItemData ed = equipItemDataList[i];

            EquipItemCardController eicc = equipItemCardControllerList[i];
            eicc.setEquipItem(ed);

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


    }
}