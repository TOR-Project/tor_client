using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NovelWindowController : MonoBehaviour
{
    [SerializeField]
    RectTransform titleContainerRT;
    [SerializeField]
    GameObject titleRowPrefab;
    [SerializeField]
    Text prevText;
    [SerializeField]
    Text nextText;
    [SerializeField]
    Text leftText;
    [SerializeField]
    Text rightText;
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
    int subscribingLoadedCount = 0;
    NovelTitleRowController currentNovelTitleRowController;
    int pageNum = 0;
    int maxPageNum = 0;

    private void OnEnable()
    {
        loadTitleData();
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
        subscribingLoadedCount = 0;
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

            for (int i = 0; i < maxStoryCount; i++)
            {
                ContractManager.instance.reqIsSubscribed(i);
            }
        }
    }

    public void setSubscribingData(int _id, bool _subscribing)
    {
        subscribingLoadedCount++;
        novelList[_id].isSubscribed = _subscribing;

        if (subscribingLoadedCount >= maxStoryCount)
        {
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

    public void successSubscribing(int _id)
    {
        novelList[_id].isSubscribed = true;
        loadFullData(novelList[_id]);
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

    public void successFullDataLoaded(int _id, string _contents, string _illustrationUrl)
    {
        novelList[_id].contents = _contents;
        novelList[_id].illustrationUrl = _illustrationUrl;
        novelList[_id].isFullDataLoaded = true;
        bookLoading.SetActive(false);
        showStoryData(novelList[_id]);
    }

    public void failFullDataLoaded(int _id)
    {
        bookLoading.SetActive(false);
    }

    private void showStoryData(NovelData _nd)
    {
        pageNum = 0;



        showPage(pageNum);
    }

    private void showPage(int _page)
    {

    }

    public void onPrevPageClicked()
    {
        if (pageNum <= 0)
        {
            return;
        }

        pageNum--;
        showPage(pageNum);

        updatePagingButton();
    }

    public void onNextPageClicked()
    {
        if (pageNum >= maxPageNum - 1)
        {
            return;
        }

        pageNum++;
        showPage(pageNum);

        updatePagingButton();
    }

    public void onMiddleOfNextAnimation()
    {

    }

    public void onMiddleOfPrevAnimation()
    {

    }

    public void updatePagingButton()
    {
        prevButton.SetActive(pageNum > 0);
        nextButton.SetActive(pageNum < maxPageNum - 1);
    }
}