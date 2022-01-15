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
    GridLayoutGroup gridLayoutGroup;
    [SerializeField]
    RectTransform gridLayoutGroupRT;
    [SerializeField]
    Button prevButton;
    [SerializeField]
    Button nextButton;
    [SerializeField]
    Text pageText;

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

    CharacterCardController selectedCharacterController = null;

    List<CharacterData> filteredCharacterDataList = new List<CharacterData>();
    List<CharacterCardController> characterCardControllerList = new List<CharacterCardController>();

    int pageNum = 0;
    int maxPageNum = 1;
    float contentsViewWidth = 0;
    float contentsViewHeight = 0;

    private void updateAllLayout()
    {
        init();
        fillGrid();
        updateCharacterData(CountryManager.COUNTRY_ALL);
        updateArrowButton();
        updateGrid(0);
    }

    private void init()
    {
        filteredCharacterDataList.Clear();
    }

    private void fillGrid()
    {
        if (characterCardControllerList.Count == 0)
        {
            contentsViewWidth = gridLayoutGroupRT.rect.width;
            float gridSpaceX = gridLayoutGroup.spacing.x;
            float gridCellX = gridLayoutGroup.cellSize.x;
            int gridMaxCellCol = (int)(contentsViewWidth / (gridCellX + gridSpaceX));
            if (gridMaxCellCol <= 0)
            {
                gridMaxCellCol = 1;
            }

            contentsViewHeight = gridLayoutGroupRT.rect.height;
            float gridSpaceY = gridLayoutGroup.spacing.y;
            float gridCellY = gridLayoutGroup.cellSize.y;
            int gridMaxCellRow = (int)((contentsViewHeight + gridSpaceY) / (gridCellY + gridSpaceY));
            if (gridMaxCellRow <= 0)
            {
                gridMaxCellRow = 1;
            }

            int maxCellCount = gridMaxCellCol * gridMaxCellRow;
            for (int i = 0; i < maxCellCount; i++)
            {
                GameObject characterCard = Instantiate(characterCardPrefab, gridPanel.transform, true);
                characterCard.transform.localScale = UnityEngine.Vector3.one;
                characterCard.SetActive(false);
                CharacterCardController cardController = characterCard.GetComponent<CharacterCardController>();
                cardController.setClickCallback(selectCharacterCard);
                characterCardControllerList.Add(cardController);
            }

            Debug.Log("fillGrid() wait " + characterCardControllerList.Count);
        }
    }

    private void Update()
    {
        float newContentsViewWidth = gridLayoutGroupRT.rect.width;
        float newContentsViewHeight = gridLayoutGroupRT.rect.height;
        if (newContentsViewWidth != contentsViewWidth || newContentsViewHeight != contentsViewHeight)
        {
            clearAllCharacterCard();
            updateAllLayout();
        }
    }

    private void clearAllCharacterCard()
    {
        for (int i = gridLayoutGroupRT.childCount - 1; i >= 0; i--)
        {
            Destroy(gridLayoutGroupRT.GetChild(i).gameObject);
            characterCardControllerList.Clear();
        }
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
            filter = CountryManager.COUNTRY_ALL;
        }

        updateCharacterData(filter);
        updateArrowButton();
        updateGrid(0);
    }

    private void updateCharacterData(int _regionFilter)
    {
        filteredCharacterDataList = getFilteredCharacterList(_regionFilter);
        filteredCharacterDataList.Sort(SortByTokenIdAscending);

        pageNum = 0;
        if (filteredCharacterDataList.Count > 0)
        {
            maxPageNum = filteredCharacterDataList.Count / characterCardControllerList.Count + (filteredCharacterDataList.Count % characterCardControllerList.Count == 0 ? 0 : 1);
        }
        else
        {
            maxPageNum = 0;
        }
        Debug.Log("updateCharacterData() count = " + filteredCharacterDataList.Count + ", maxPageNum = " + maxPageNum);

    }

    private void updateArrowButton()
    {
        prevButton.interactable = pageNum > 0;
        nextButton.interactable = pageNum < maxPageNum - 1;
    }

    private void updateGrid(int _page)
    {
        Debug.Log("updateGrid _page = " + _page);
        for (int i = 0; i < characterCardControllerList.Count; i++)
        {
            CharacterCardController controller = characterCardControllerList[i];

            int characterDataIdx = _page * characterCardControllerList.Count + i;
            Debug.Log("updateGrid idx = " + characterDataIdx);
            if (characterDataIdx < filteredCharacterDataList.Count)
            {
                CharacterData data = filteredCharacterDataList[characterDataIdx];
                controller.gameObject.SetActive(true);
                controller.setCharacterId(data, CharacterCardController.STATE_WAITING_ROOM);
                bool select = selectedCharacterController != null && selectedCharacterController.getCharacterData().tokenId == data.tokenId;
                controller.setSelected(select);
                if (select)
                {
                    selectedCharacterController = controller;
                }
            }
            else
            {
                controller.gameObject.SetActive(false);
            }
        }

        pageText.text = (maxPageNum == 0 ? 0 : (_page + 1)) + "/" + maxPageNum;


        if (filteredCharacterDataList.Count > 0)
        {
            selectCharacterCard(characterCardControllerList[0], true);
        }
        else
        {
            selectCharacterCard(null);
        }
    }

    private List<CharacterData> getFilteredCharacterList(int _regionFilter)
    {
        List<CharacterData> allList = CharacterManager.instance.getMyCharacterList();
        if (_regionFilter == CountryManager.COUNTRY_ALL)
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

    public void onPagePrevButtonClicked()
    {
        if (pageNum <= 0)
        {
            return;
        }

        pageNum--;
        updateGrid(pageNum);

        updateArrowButton();
    }

    public void onPageNextButtonClicked()
    {
        if (pageNum >= maxPageNum - 1)
        {
            return;
        }

        pageNum++;
        updateGrid(pageNum);

        updateArrowButton();
    }

    public bool selectCharacterCard(CharacterCardController _cardController)
    {
        return selectCharacterCard(_cardController, false);
    }

    public bool selectCharacterCard(CharacterCardController _cardController, bool _forced)
    {
        if (!_forced && selectedCharacterController == _cardController)
        {
            return false;
        }

        if (selectedCharacterController != null)
        {
            selectedCharacterController.setSelected(false);
        }

        if (_cardController != null)
        {
            _cardController.setSelected(true);
        }

        selectedCharacterController = _cardController;

        updateInfoPanel(_cardController);
        return true;
    }

    private void updateInfoPanel(CharacterCardController _cardController)
    {
        if (_cardController == null || _cardController.getCharacterData() == null)
        {
            infoPanel.SetActive(false);
            return;
        }
        infoPanel.SetActive(true);

        CharacterData data = _cardController.getCharacterData();
        characterImageController.updateCharacterImage(data);

        nameText.text = data.name;
        regionText.text = CountryManager.instance.getCountryName(data.country);
        raceText.text = CharacterManager.instance.getRaceText(data.race);
        classText.text = CharacterManager.instance.getJobText(data.job);
        levelValueText.text = data.level.ToString();
        attValueText.text = data.statusData.att.ToString();
        defValueText.text = data.statusData.def.ToString();

        EquipItemData[] equipItemDataList = new EquipItemData[]
        {
            ItemManager.instance.getEquipItem(data.equipData.head),
            ItemManager.instance.getEquipItem(data.equipData.weapon),
            ItemManager.instance.getEquipItem(data.equipData.accessory),
            ItemManager.instance.getEquipItem(data.equipData.armor),
            ItemManager.instance.getEquipItem(data.equipData.pants),
            ItemManager.instance.getEquipItem(data.equipData.shoes),
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


    }
}