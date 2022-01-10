using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NovelWindowController : MonoBehaviour
{
    private const int PREV = 0;
    private const int LEFT = 1;
    private const int RIGHT = 2;
    private const int NEXT = 3;

    public int defaultFontSize = 18;

    [SerializeField]
    RectTransform titleContainerRT;
    [SerializeField]
    GameObject titleRowPrefab;
    [SerializeField]
    Text[] contentsText;
    [SerializeField]
    Text[] pageText;
    [SerializeField]
    Text cacheText;
    [SerializeField]
    GameObject titleContainerLoading;
    [SerializeField]
    GameObject bookLoading;

    [SerializeField]
    GameObject prevButton;
    [SerializeField]
    GameObject nextButton;

    [SerializeField]
    GlobalUIWindowController globalUIWindowController;

    [SerializeField]
    Animator pageAnimator;
    private string nextTrigger = "next";
    private string prevTrigger = "prev";

    int maxStoryCount = 0;
    List<NovelData> novelList = new List<NovelData>();
    List<NovelFragmentsData> novelFregmentData = new List<NovelFragmentsData>();
    NovelTitleRowController currentNovelTitleRowController;
    int pageNum = 0;
    int maxPageNum = 0;

    private void OnEnable()
    {
        loadTitleData();
        cacheText.fontSize = defaultFontSize;
    }

    private void loadTitleData()
    {
        titleContainerLoading.SetActive(true);
        ContractManager.instance.reqGetStoryCount();
    }

    public void setStoryCount(int _count)
    {
        if (titleContainerRT.childCount == _count)
        {
            titleContainerLoading.SetActive(false);
            return;
        }
        removeAllTitleRow();

        maxStoryCount = _count;
        novelList.Clear();
        for (int i = 0; i < _count; i++)
        {
            ContractManager.instance.reqGetStorySummery(i);
        }
    }

    private void removeAllTitleRow()
    {
        for (int i = titleContainerRT.childCount - 1; i >= 0; i--)
        {
            Destroy(titleContainerRT.GetChild(i).gameObject);
        }
    }

    public void setNovelData(NovelData _nd)
    {
        novelList.Add(_nd);

        if (novelList.Count >= maxStoryCount)
        {
            novelList.Sort(SortByIdAscending);
            configureTitleContainer();
        }
    }

    private void configureTitleContainer()
    {
        for (int i = 0; i < novelList.Count; i++)
        {
            NovelData nd = novelList[i];
            GameObject titleRow = Instantiate(titleRowPrefab, titleContainerRT.transform, true);
            titleRow.transform.localScale = UnityEngine.Vector3.one;
            NovelTitleRowController rowController = titleRow.GetComponent<NovelTitleRowController>();
            rowController.setNovelData(nd);
            rowController.setClickCallback(onTitleItemClicked);
        }

        titleContainerLoading.SetActive(false);
    }

    public int SortByIdAscending(NovelData _nd1, NovelData _nd2)
    {
        return _nd1.id - _nd2.id;
    }

    public bool onTitleItemClicked(NovelTitleRowController _ndc)
    {
        NovelData nd = null;
        if (currentNovelTitleRowController == _ndc)
        {
            return false;
        }
        if (currentNovelTitleRowController != null)
        {
            currentNovelTitleRowController.setSelect(false);
        }
        if (_ndc != null)
        {
            _ndc.setSelect(true);
            nd = _ndc.getNovelData();
        }
        currentNovelTitleRowController = _ndc;

        if (nd == null)
        {
            return false;
        }

        // free or subscribing
        if (nd.isFreeOrSubscribe())
        {
            loadFullData(nd);
        }
        else // paid
        {
            showSubscribingPopup(nd);
        }

        return true;
    }

    private void showSubscribingPopup(NovelData _nd)
    {
        string message = string.Format(LanguageManager.instance.getText("ID_NOVEL_SUBSCRIBE_POPUP_MESSAGE"), _nd.mainTitle, Const.SUBSCRIBE_FEE);
        globalUIWindowController.showConfirmPopup(message, () => subscribeStory(_nd));
    }

    public void subscribeStory(NovelData _nd)
    {
        ContractManager.instance.reqSubscribeStory(_nd.id);
    }

    public void onSubscribingCompleted(int _id, bool _success)
    {
        if (_success)
        {
            novelList[_id].isSubscribed = true;
            loadFullData(novelList[_id]);
        }
        else
        {
            globalUIWindowController.showPopupByTextKey("ID_NOVEL_SUBSCRIBE_FAILED", null);
        }
    }

    public void loadFullData(NovelData _nd)
    {
        if (!_nd.isFullDataLoaded)
        {
            bookLoading.SetActive(true);
            ContractManager.instance.reqGetStoryDataFull(_nd.id);
        } else
        {
            showStoryData(_nd);
        }
    }

    public void onFullDataLoaded(bool _success, int _id, string _contents, string _illustrationUrl)
    {
        bookLoading.SetActive(false);

        if (_success)
        {
            novelList[_id].setFullContents(_contents, _illustrationUrl);
            showStoryData(novelList[_id]);
        }
        else
        {
            globalUIWindowController.showPopupByTextKey("ID_NOVEL_FULL_DOWNLOAD_FAILED", null);
        }
    }

    private void showStoryData(NovelData _nd)
    {
        pageNum = 2;

        makeNovelFregmentData(_nd.contents);

        showPage(pageNum, null);

        updatePagingButton();
    }

    private void makeNovelFregmentData(string _contents)
    {
        int page = 0;
        novelFregmentData.Clear();
        novelFregmentData.Add(new NovelFragmentsData(page++, "", NovelFragmentsType.CONTENTS)); // not use 0 page
        novelFregmentData.Add(new NovelFragmentsData(page++, "", NovelFragmentsType.CONTENTS)); // not use 1 page

        string contents = _contents;

        int titleStartIdx = contents.IndexOf(NovelData.TITLE_TAG) + NovelData.TITLE_TAG.Length;
        int titleEndIdx = contents.IndexOf(NovelData.TITLE_END_TAG);
        int length = titleEndIdx - titleStartIdx;
        novelFregmentData.Add(new NovelFragmentsData(page++, contents.Substring(titleStartIdx, length), NovelFragmentsType.TITLE));
        contents = contents.Remove(titleStartIdx - NovelData.TITLE_TAG.Length, length + NovelData.TITLE_TAG.Length + NovelData.TITLE_END_TAG.Length);

        while(contents.Length > 0)
        {
            int bestIndex = getBestContentsPosition(contents);
            novelFregmentData.Add(new NovelFragmentsData(page++, contents.Substring(0, bestIndex), NovelFragmentsType.CONTENTS));
            contents = contents.Remove(0, bestIndex).Trim();
        }

        maxPageNum = novelFregmentData.Count;

    }

    private int getBestContentsPosition(string _contents)
    {
        var extents = cacheText.cachedTextGenerator.rectExtents.size;
        RectTransform rectTransform = cacheText.gameObject.GetComponent<RectTransform>();
        float height = rectTransform.rect.height;

        int lastIndex = -1;
        int index;
        while (true)
        {
            index = _contents.IndexOf('\n', lastIndex + 1);
            if (index == -1)
            {
                index = _contents.Length;
                break;
            }

            float textHeight = cacheText.cachedTextGeneratorForLayout.GetPreferredHeight(_contents.Substring(0, index), cacheText.GetGenerationSettings(extents));
            if (textHeight > height)
            {
                index = lastIndex;
                break;
            }
            lastIndex = index;
        }

        return index;
    }

    private void showPage(int _page, string _animationTrigger)
    {
        if (_animationTrigger == null)
        {
            updatePage(contentsText[LEFT], pageText[LEFT], _page < novelFregmentData.Count ? novelFregmentData[_page] : null);
            updatePage(contentsText[RIGHT], pageText[RIGHT], _page + 1 < novelFregmentData.Count ? novelFregmentData[_page + 1] : null);
        } else
        {
            pageAnimator.SetTrigger(_animationTrigger);
        }
    }

    private void updatePage(Text _contentsText, Text _pageText, NovelFragmentsData _data)
    {
        if (_data == null)
        {
            _contentsText.text = "";
            _pageText.text = "";
            return;
        }

        _contentsText.text = _data.contents;
        _pageText.text = _data.page.ToString();

        switch (_data.type) {
            case NovelFragmentsType.TITLE:
                _contentsText.fontSize = defaultFontSize + 5;
                _contentsText.fontStyle = FontStyle.Bold;
                _contentsText.alignment = TextAnchor.MiddleCenter;
                break;
            case NovelFragmentsType.CONTENTS:
            default:
                _contentsText.fontSize = defaultFontSize;
                _contentsText.fontStyle = FontStyle.Normal;
                _contentsText.alignment = TextAnchor.UpperLeft;
                break;
        }
    }

    public void onPrevPageClicked()
    {
        if (pageNum <= 2)
        {
            return;
        }

        pageNum -= 2;
        showPage(pageNum, prevTrigger);

        updatePagingButton();
    }

    public void onNextPageClicked()
    {
        if (pageNum >= maxPageNum - 2)
        {
            return;
        }

        pageNum += 2;
        showPage(pageNum, nextTrigger);

        updatePagingButton();
    }

    public void onStartOfNextAnimation()
    {
        updatePage(contentsText[PREV], pageText[PREV], pageNum - 2 < novelFregmentData.Count ? novelFregmentData[pageNum - 2] : null);
        updatePage(contentsText[LEFT], pageText[LEFT], pageNum < novelFregmentData.Count ? novelFregmentData[pageNum] : null);
        updatePage(contentsText[RIGHT], pageText[RIGHT], pageNum - 1 < novelFregmentData.Count ? novelFregmentData[pageNum - 1] : null);
        updatePage(contentsText[NEXT], pageText[NEXT], pageNum + 1 < novelFregmentData.Count ? novelFregmentData[pageNum + 1] : null);
    }

    public void onStartOfPrevAnimation()
    {
        updatePage(contentsText[PREV], pageText[PREV], pageNum < novelFregmentData.Count ? novelFregmentData[pageNum] : null);
        updatePage(contentsText[LEFT], pageText[LEFT], pageNum + 2< novelFregmentData.Count ? novelFregmentData[pageNum + 2] : null);
        updatePage(contentsText[RIGHT], pageText[RIGHT], pageNum + 1 < novelFregmentData.Count ? novelFregmentData[pageNum + 1] : null);
        updatePage(contentsText[NEXT], pageText[NEXT], pageNum + 3 < novelFregmentData.Count ? novelFregmentData[pageNum + 3] : null);
    }

    public void onMiddleOfNextAnimation()
    {
        updatePage(contentsText[RIGHT], pageText[RIGHT], pageNum + 1 < novelFregmentData.Count ? novelFregmentData[pageNum + 1] : null);
    }

    public void onMiddleOfPrevAnimation()
    {
        updatePage(contentsText[LEFT], pageText[LEFT], pageNum < novelFregmentData.Count ? novelFregmentData[pageNum] : null);
    }

    public void updatePagingButton()
    {
        prevButton.SetActive(pageNum > 2);
        nextButton.SetActive(pageNum < maxPageNum - 2);
    }
}