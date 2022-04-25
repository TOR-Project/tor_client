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
    int maxSelectedCount = 1;
    [SerializeField]
    Text countText;
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
    Button allSelectButton;
    [SerializeField]
    Button allDeselectButton;
    [SerializeField]
    Button confirmButton;
    [SerializeField]
    Button cancelButton;
    [SerializeField]
    CharacterCardController.CharacterCardState state;

    List<CharacterData> selectedCharacterDataList = new List<CharacterData>();

    List<CharacterData> filteredCharacterDataList = new List<CharacterData>();
    List<CharacterCardController> characterCardControllerList = new List<CharacterCardController>();

    int pageNum = 0;
    int maxPageNum = 1;
    float contentsViewWidth = 0;
    float contentsViewHeight = 0;
    Predicate<CharacterData> lastAppliedFilter = null;

    Func<List<CharacterData>, bool> onButtonClickedCallback = null;
    Func<CharacterData, bool> characterSelectCallbck = null;
    private bool buttonEnabled = true;

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

        selectedCharacterDataList.Clear();
        // Debug.Log("updateCharacterData() count = " + filteredCharacterDataList.Count + ", maxPageNum = " + maxPageNum);

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
        // Debug.Log("updateGrid _page = " + _page);
        for (int i = 0; i < characterCardControllerList.Count; i++)
        {
            CharacterCardController controller = characterCardControllerList[i];

            int characterDataIdx = _page * characterCardControllerList.Count + i;
            // Debug.Log("updateGrid idx = " + characterDataIdx);
            if (characterDataIdx < filteredCharacterDataList.Count)
            {
                CharacterData data = filteredCharacterDataList[characterDataIdx];
                controller.gameObject.SetActive(true);
                controller.setCharacterId(data, state);
                bool select = selectedCharacterDataList.Contains(data);
                controller.setSelected(select);
            }
            else
            {
                controller.gameObject.SetActive(false);
            }
        }

        pageText.text = (maxPageNum == 0 ? 0 : (_page + 1)) + "/" + maxPageNum;

        if (setDefaultSelect && selectedCharacterDataList.Count == 0)
        {
            selectCharacterCard(filteredCharacterDataList.Count > 0 ? characterCardControllerList[0] : null);
        }

        countText.text = selectedCharacterDataList.Count + "/" + maxSelectedCount;
    }

    internal void setEnableAllButtons(bool enable)
    {
        buttonEnabled = enable;
        prevButton.interactable = enable;
        nextButton.interactable = enable;
        allDeselectButton.interactable = enable;
        allSelectButton.interactable = enable;
        confirmButton.interactable = enable;
        cancelButton.interactable = enable;
    }

    public void showDragonCheckEffect(int[] tokenIdArr)
    {
        List<int> tokenIdList = new List<int>(tokenIdArr);
        foreach(CharacterCardController controller in characterCardControllerList)
        {
            if (tokenIdList.Contains(controller.getCharacterData().tokenId)) {
                controller.showEnchantEffect();
            }
        }
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

    public void setMaxSelectedCount(int _count)
    {
        maxSelectedCount = _count;
    }

    private bool selectCharacterCard(CharacterCardController _cardController)
    {
        if (!buttonEnabled || isSameCharacterSelectedWhenSingleMode(_cardController))
        {
            // not reaction for same item selected when single mode
            return false;
        }

        if (maxSelectedCount == 1)
        {
            selectedCharacterDataList.Clear();
        }

        if (selectedCharacterDataList.Contains(_cardController.getCharacterData()))
        {
            selectedCharacterDataList.Remove(_cardController.getCharacterData());
        }
        else if (selectedCharacterDataList.Count >= maxSelectedCount)
        {
            return false;
        }
        else
        {
            selectedCharacterDataList.Add(_cardController.getCharacterData());
        }

        updateGrid(pageNum);
        countText.text = selectedCharacterDataList.Count + "/" + maxSelectedCount;

        characterSelectCallbck?.Invoke(_cardController.getCharacterData());
        return true;
    }

    private bool isSameCharacterSelectedWhenSingleMode(CharacterCardController _cardController)
    {
        return maxSelectedCount == 1 && selectedCharacterDataList.Contains(_cardController.getCharacterData());
    }

    public bool isCharacterEmpty()
    {
        return filteredCharacterDataList.Count == 0;
    }

    public void setOnButtonClickedCallback(Func<List<CharacterData>, bool> _callback)
    {
        onButtonClickedCallback = _callback;
    }

    public void setCharacterSelectCallback(Func<CharacterData, bool> _callback)
    {
        characterSelectCallbck = _callback;
    }

    public void onAllSelectButtonClicked()
    {
        foreach (CharacterData cd in filteredCharacterDataList)
        {
            if (selectedCharacterDataList.Count >= maxSelectedCount)
            {
                break;
            }

            if (!selectedCharacterDataList.Contains(cd))
            {
                selectedCharacterDataList.Add(cd);
            }
        }

        updateGrid(pageNum);
    }

    public void onAllDeselectButtonClicked()
    {
        selectedCharacterDataList.Clear();
        updateGrid(pageNum);
    }

    public void onConfirmButtonClicked()
    {
        onButtonClickedCallback?.Invoke(selectedCharacterDataList);
    }

}