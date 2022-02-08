using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Numerics;

public class NovelTitleRowController : MonoBehaviour
{
    public static Color NORMAL_COLOR = new Color(255, 255, 255);
    public static Color PRESSED_COLOR = new Color(100, 100, 100);
    public static Color SELECTED_COLOR = new Color(200, 200, 0);
    public static Color SUBSCRIVED_COLOR = new Color(0, 200, 0);
    public static Color PAID_COLOR = new Color(200, 200, 0);

    [SerializeField]
    Text titleText;
    [SerializeField]
    Text stateText;
    Color32 stateTextColor;
    bool selected;
    Func<NovelTitleRowController, bool> onClickCallback;
    NovelData novelData;

    public void setNovelData(NovelData _nd)
    {
        novelData = _nd;
        titleText.text = _nd.mainTitle;

        if (_nd.freeBlock == 0) // free
        {
            stateText.text = "";
            stateTextColor = SUBSCRIVED_COLOR;
        }
        else // paid
        {
            if (_nd.isSubscribed)
            {
                stateText.text = LanguageManager.instance.getText("ID_NOVEL_STATE_SUBSCRRIBE");
                stateTextColor = SUBSCRIVED_COLOR;
            }
            else
            {
                stateText.text = LanguageManager.instance.getText("ID_NOVEL_STATE_PAID");
                stateTextColor = PAID_COLOR;
            }
        }
        stateText.color = stateTextColor;
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
        titleText.color = _set ? SELECTED_COLOR : NORMAL_COLOR;
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
        titleText.color = PRESSED_COLOR;
        stateText.color = PRESSED_COLOR;
    }

    public void onEventUp()
    {
        if (selected)
        {
            return;
        }
        titleText.color = NORMAL_COLOR;
        stateText.color = stateTextColor;
    }
}