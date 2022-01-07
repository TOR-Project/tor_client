using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Numerics;

public class NovelTitleRowController : MonoBehaviour
{
    [SerializeField]
    Text titleText;
    [SerializeField]
    Text stateText;
    bool selected;
    Func<NovelTitleRowController, bool> onClickCallback;
    NovelData novelData;

    public void setNovelData(NovelData _nd)
    {
        novelData = _nd;
        titleText.text = _nd.mainTitle;

        if (_nd.freeBlock == 0) // free
        {
            stateText.text = LanguageManager.instance.getText("ID_NOVEL_STATE_FREE");
            stateText.color = new Color32(0, 200, 0, 255);
        } else // paid
        {
            if (_nd.isSubscribed)
            {
                stateText.text = LanguageManager.instance.getText("ID_NOVEL_STATE_SUBSCRRIBE");
                stateText.color = new Color32(0, 200, 0, 255);
            } else
            {
                stateText.text = LanguageManager.instance.getText("ID_NOVEL_STATE_PAID");
                stateText.color = new Color32(200, 200, 0, 255);
            }
        }
    }

    public NovelData getNovelData()
    {
        return novelData;
    }

    public void setClickCallback(Func<NovelTitleRowController, bool> _onClickCallback)
    {
        onClickCallback = _onClickCallback;
    }

    public void setSelect(bool _set)
    {
        selected = _set;
        titleText.color = _set ? new Color32(200, 200, 0, 255) : new Color32(255, 255, 255, 255);
    }

    public void onClickItem()
    {
        if (selected)
        {
            return;
        }
        onClickCallback(this);
    }

    public void onEventDown()
    {
        if (selected)
        {
            return;
        }
        titleText.color = new Color32(100, 100, 100, 255);
        stateText.color = new Color32(100, 100, 100, 255);
    }

    public void onEventUp()
    {
        if (selected)
        {
            return;
        }
        titleText.color = new Color32(255, 255, 255, 255);
        stateText.color = new Color32(255, 255, 255, 255);
    }
}