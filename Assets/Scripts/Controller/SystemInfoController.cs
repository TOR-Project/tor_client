using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System.Numerics;

public class SystemInfoController : MonoBehaviour, BlockNumberObserever
{
    [SerializeField]
    Text calendarText;
    [SerializeField]
    Text blockText;

    private void Awake()
    {
        SystemInfoManager.instance.addBlockNumberObserver(this);
    }

    private void OnDestroy()
    {
        SystemInfoManager.instance.removeBlockNumberObserver(this);
    }

    public void onBlockNumberChanged(long _num)
    {
        calendarText.text = LanguageManager.instance.getText("ID_TOR_CALENDAR") + " " + Utils.getTorCalendarStr(_num);
        blockText.text = string.Format("({0:#,###} block)", _num);
    }
}