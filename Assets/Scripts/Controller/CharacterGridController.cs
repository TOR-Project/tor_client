using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Numerics;
using Coffee.UIExtensions;

public class CharacterGridController : MonoBehaviour
{
    [SerializeField]
    bool setDefaultSelect = false;
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
    CharacterCardController.CharacterCardState state;

    CharacterCardController selectedCharacterController = null;

    List<CharacterData> filteredCharacterDataList = new List<CharacterData>();
    List<CharacterCardController> characterCardControllerList = new List<CharacterCardController>();

    int pageNum = 0;
    int maxPageNum = 1;
    float contentsViewWidth = 0;
    float contentsViewHeight = 0;
    Predicate<CharacterData> lastAppliedFilter = null;

    Func<CharacterCardController, bool> characterSelectCallbck = null;

    private void updateAllLayout()
    {
        init();
        fillGrid();
        updateCharacterData(lastAppliedFilter);
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

    public void updateCharacterData(Predicate<CharacterData> _filter)
    {
        lastAppliedFilter = _filter;
        filteredCharacterDataList = getFilteredCharacterList(_filter);
        filteredCharacterDataList.Sort(SortByTokenIdAscending);

        pageNum = 0;
        if (filteredCharacterDataList.Count > 0 && characterCardControllerList.Count > 0)
        {
            maxPageNum = filteredCharacterDataList.Count / characterCardControllerList.Count + (filteredCharacterDataList.Count % characterCardControllerList.Count == 0 ? 0 : 1);
        }
        else
        {
            maxPageNum = 0;
        }

        for (int i = 0; i < characterCardControllerList.Count; i++)
        {
            CharacterCardController controller = characterCardControllerList[i];
            controller.setSelected(false);
        }
        if (selectedCharacterController != null)
        {
            selectedCharacterController.setSelected(false);
            selectedCharacterController = null;
        }
        Debug.Log("updateCharacterData() count = " + filteredCharacterDataList.Count + ", maxPageNum = " + maxPageNum);

        updateArrowButton();
        updateGrid(0);
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
                controller.setCharacterId(data, state);
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

        if (setDefaultSelect)
        {
            if (filteredCharacterDataList.Count > 0)
            {
                selectCharacterCard(characterCardControllerList[0], true);
            }
            else
            {
                selectCharacterCard(null);
            }
        }
    }

    public CharacterCardController getSelectedCharacterCardController()
    {
        return selectedCharacterController;
    }

    private List<CharacterData> getFilteredCharacterList(Predicate<CharacterData> _filter)
    {
        List<CharacterData> allList = CharacterManager.instance.getMyCharacterList();
        if (_filter == null)
        {
            return allList;
        }
        return allList.FindAll(_filter);
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

    private bool selectCharacterCard(CharacterCardController _cardController)
    {
        return selectCharacterCard(_cardController, false);
    }

    private bool selectCharacterCard(CharacterCardController _cardController, bool _forced)
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

        characterSelectCallbck?.Invoke(_cardController);
        return true;
    }

    public bool isCharacterEmpty()
    {
        return filteredCharacterDataList.Count == 0;
    }

    public void setCharacterSelectCallback(Func<CharacterCardController, bool> _callback)
    {
        characterSelectCallbck = _callback;
    }

}